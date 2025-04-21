using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : IAttackComponent
{
    public float Damage => 10;
    public float AttackRange =>3.0f;
    public CharacterData characterData;
    [SerializeField]
    Character characterTarget,attackTarget;
    
    public void MakeDamage()
    {//public void MakeDamage(Character attackTarget) {
        if (Vector3.Distance(characterData.CharacterTransform.position, attackTarget.transform.position) <= AttackRange)
        {
            attackTarget.HealthComponent.SetDamage((int)Damage);
        }
    }
    public void Initialize(CharacterData characterData)
    {
        this.characterData = characterData;

    }
}
