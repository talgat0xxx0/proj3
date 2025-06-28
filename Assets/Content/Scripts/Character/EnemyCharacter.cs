
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EnemyCharacter : Character
{
    [SerializeField] private float targetCheckingDistance = 0.5f;
    [SerializeField]
    private Character characterTarget;
     
    [SerializeField]
    private AiState aiState;
    private Rigidbody rigBody;
    private Vector3 tempvec;
    private int Speed = 2;

    public override void Initialize()
    {
        base.Initialize();
        HealthComponent healthComponent = new HealthComponent();
        rigBody = GetComponent<Rigidbody>();
    }
    void Start(){
        Initialize();
    }

    protected override void Update()

    {
        
        switch (aiState) {
            case AiState.Idle:
            return;

            case AiState.MoveToTarget:
                Vector3 direction = characterTarget.transform.position - transform.position;
                direction.Normalize();
                float fdis = float.MaxValue;
                tempvec = tempvec.normalized * Speed * Time.deltaTime;
                float distance = Vector3.Distance(characterTarget.transform.position, transform.position);
                
                rigBody.MovePosition(characterTarget.transform.position * Time.fixedTime  );
                if ((distance < fdis))
                {
                    AttackComponent.MakeAttack();
                }
                return;

        }
        
    }
}
