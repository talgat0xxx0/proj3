using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class PlayerCharacter : Character
{
    //public Animator animator;
    
    // Update is called once per frame
    
    protected override void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput,  0,verticalInput).normalized;
        MovementComponent.Move(moveDirection*Time.fixedDeltaTime);
        
    }
    void Start()
    {
        Initialize();
        
        
    }
    
    public override void Initialize()
    {
        base.Initialize();
        HealthComponent healthComponent = new HealthComponent();
        //MovementComponent movecomponent=new DefaultMovementComponent();
    }

}
