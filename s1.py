!pip install transformers datasets evaluate sacrebleu bert-score sentencepiece
pip install rouge_score
!huggingface-cli login --token=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
pip install sacrebleu
pip install bert_score
pip install evaluate
pip install -U bitsandbytes
# -*- coding: utf-8 -*-vot67

import torch
from transformers import AutoTokenizer, AutoModelForCausalLM
from datasets import load_from_disk, load_dataset  # <--- Добавил load_dataset
import evaluate
import pandas as pd
import re
from peft import PeftModel

# ===== ПУТИ =====
VAL_DATA = "/content/gdrive/MyDrive/trns/gpt_dataset"
VAL_PATH = "/content/gdrive/MyDrive/tengri_test.jsonl" # <--- Исправил комментарий
BASE_MODEL = "google/gemma-3-4b-it" # Убедитесь, что модель существует (обычно gemma-2-9b-it)
LORA_PATH  = "talgatzh/gemma-lora-merged0405zzz1"
SAVE_CSV   = "/content/gdrive/MyDrive/evaluation_results.csv"

# 1. ЗАГРУЗКА ДАТАСЕТА
print(f"⏳ Загрузка датасета из: {VAL_PATH}")
# Загружаем jsonl, берем сплит train и выбираем первые 50 примеров
val_dataset = load_dataset("json", data_files=VAL_PATH)["train"].select(range(500))#val_dataset = load_dataset("json", data_files=VAL_PATH)["train"].select(range(50))

# 2. ЗАГРУЗКА ТОКЕНИЗЕРА
print(f"⏳ Загрузка токенизера: {BASE_MODEL}")
tokenizer = AutoTokenizer.from_pretrained(BASE_MODEL)
if tokenizer.pad_token is None:
    tokenizer.pad_token = tokenizer.eos_token

# 3. ЗАГРУЗКА МОДЕЛИ
print(f"⏳ Загрузка модели: {BASE_MODEL}")
model = AutoModelForCausalLM.from_pretrained(
    BASE_MODEL,
    device_map="auto",
    torch_dtype=torch.bfloat16,
    load_in_4bit=True
)

print(f"⏳ Подключение LoRA: {LORA_PATH}")
#model = PeftModel.from_pretrained(model, LORA_PATH)
#model.eval()
print("🔥 Модель готова к работе!")

# ================================================================

def clean_text(text: str) -> str:
    text = re.sub(r"[\n\r]+", " ", text)
    text = re.sub(r"\s{2,}", " ", text)
    return text.strip()

def build_prompt(doc: str):
    messages = [
        {
            "role": "user",
            "content": (
                "Мәтіннің мазмұнын толық және егжей-тегжейлі сипаттап бер. "
                "Мағынаны өзгертпе, сөйлемдерді мүмкіндігінше бастапқы түрінде сақта.\n\n"
                f"{doc.strip()}\n\nТолық мазмұны:"
            )
        }
    ]

    prompt_text = tokenizer.apply_chat_template(
        messages,
        tokenize=False,
        add_generation_prompt=True
    )

    enc = tokenizer(
        prompt_text,
        return_tensors="pt",
        truncation=True,
        max_length=2048
    )

    return {k: v.to(model.device) for k, v in enc.items()}

# ===== НАСТРОЙКИ ГЕНЕРАЦИИ =====
gen_cfg = dict(
    max_new_tokens=512,   # <--- Увеличил для "полного" описания
    min_new_tokens=20,
    temperature=0.5,
    do_sample=True,
    repetition_penalty=1.1, # Можно поставить 1.1, если будут повторы
    eos_token_id=tokenizer.eos_token_id,
    pad_token_id=tokenizer.pad_token_id or tokenizer.eos_token_id
)

# ================================================================

preds, refs, docs = [], [], []
print("🚀 Старт генерации...")

for idx, example in enumerate(val_dataset):
    # ВНИМАНИЕ: Проверьте, что в jsonl ключи именно "text" и "title"
    doc = clean_text(example["text"])
    ref = example["title"].strip()

    batch = build_prompt(doc)

    with torch.no_grad():
        output = model.generate(
            input_ids=batch["input_ids"],
            attention_mask=batch["attention_mask"],
            **gen_cfg
        )

    # === ИСПРАВЛЕНИЕ НАЧИНАЕТСЯ ЗДЕСЬ ===

    # 1. Считаем длину входного промпта в токенах
    input_len = batch["input_ids"].shape[1]

    # 2. Отрезаем входные токены, оставляя только сгенерированные
    generated_ids = output[0][input_len:]

    # 3. Декодируем только новые токены
    # skip_special_tokens=True автоматически уберет <eos>, <pad> и прочие спецсимволы
    decoded = tokenizer.decode(generated_ids, skip_special_tokens=True)

    # Очистка от лишних пробелов (твоя функция)
    generated = clean_text(decoded)

    # === КОНЕЦ ИСПРАВЛЕНИЯ ===

    preds.append(generated)
    refs.append(ref)
    docs.append(doc)

    if (idx + 1) % 10 == 0:
        print(f"✔ {idx+1}/{len(val_dataset)}")

print("✔ Генерация завершена. Считаем метрики...")

# ===== МЕТРИКИ =====
rouge = evaluate.load("rouge")
scores = rouge.compute(predictions=preds, references=refs, use_stemmer=True)

bert = evaluate.load("bertscore").compute(
    predictions=preds,
    references=refs,
    lang="kk"
)

P = sum(bert["precision"]) / len(bert["precision"])
R = sum(bert["recall"]) / len(bert["recall"])
F1 = sum(bert["f1"]) / len(bert["f1"])

print("\n📊 ROUGE:")
for k, v in scores.items():
    print(f"{k}: {v:.4f}")

chrf = evaluate.load("chrf")
chrf_scores = chrf.compute(predictions=preds, references=refs, word_order=2)

print("\n🔤 chrF:")
for k, v in chrf_scores.items():
    print(f"{k}: {v:.4f}")

print("\n🤖 BERTScore:")
print(f"P: {P:.4f}\nR: {R:.4f}\nF1: {F1:.4f}")

# Вывод первых примеров для проверки
for i in range(min(3, len(docs))):
    print(f"\n🔹 Example {i+1}")
    print(f"📝 Document (start): {docs[i][:100]}...")
    print(f"✅ Reference: {refs[i]}")
    print(f"🧠 Generated: {preds[i]}")

# ===== СОХРАНЕНИЕ =====
pd.DataFrame({
    "document": docs,
    "reference": refs,
    "generated": preds
}).to_csv(SAVE_CSV, index=False)

print("\n✅ Results saved:", SAVE_CSV)