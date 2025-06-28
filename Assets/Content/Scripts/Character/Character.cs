using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public IHealthComponent HealthComponent { get; protected set; }
    public IMovementComponent MovementComponent { get; protected set; }
    public IAttackComponent AttackComponent { get; protected set; }
    
    

    [SerializeField]
    private CharacterData characterData;
    public CharacterData CharacterData => characterData;
    void Start()
    {    
        Initialize();
           
    }

    //public abstract Character Target { get; }

    public virtual void Initialize(){
        MovementComponent=new DefaultMovementComponent();
        MovementComponent.Initialize(characterData);
        AttackComponent = new AttackComponent();
        AttackComponent.Initialize(characterData);

    }
    
    protected abstract void Update();
   

    

    
}
