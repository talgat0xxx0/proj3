using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class DefaultMovementComponent : IMovementComponent
{
    [SerializeField]
    private Transform characterTransform;
    private CharacterData characterData;
    private Character character;
    private float speed;
    private float turnSmoothVelocity = 0.1f;
    public float jumpPower = 1f;
    //private bool is_grounded;
    
    private bool isGrounded;
    [SerializeField]
    public LayerMask groundLayer;
    [SerializeField]
    
    public float groundCheckRadius = 0.2f;
    public float jumpForce = 7f;
    public Vector3 jump;
    public Vector3 velo;
    
    public LayerMask groundMask;//
    

    public float Speed { 
        get=>speed;
        set
        {
            speed = value;
        } 
    }
    
    
    public void Initialize(CharacterData characterData)
    {
        this.characterData = characterData;
        Speed = characterData.DefaultSpeed;
        
        jump=new Vector3(0.0f,2.0f,0.0f);
        groundMask = LayerMask.GetMask("Water");//groundMask = LayerMask.GetMask("Ground");
        

    }

    public Vector3 Position =>  characterData.CharacterTransform.position;
    
    
    public void Move(Vector3 direction)
    {
        
        if(characterData.groundCheck==null){
            Debug.Log("err");
            return;
        }

        isGrounded=Physics.CheckSphere(characterData.groundCheck.position,0.5f,groundMask);
        characterData.speed=2;
        
        if (Input.GetButtonDown("Jump") && isGrounded )
        {
    
            characterData.animator.SetBool("is_moving",true);
            //characterData.animator.SetTrigger("walk")   ;
            Debug.Log("jump");
            velo.y =Mathf.Sqrt(2f*9.81f*1.5f);//to fixed update                   
            
        }
        if(direction.magnitude>0.01){
            Rotation(direction);
        }
        characterData.animator.SetBool("is_moving",true);
        characterData.animator.SetTrigger("walk")   ;
        
        Vector3 move = characterData.CharacterTransform.TransformDirection(direction);
        float current_speed= move.magnitude*3f;
        characterData.animator.SetFloat("speed", current_speed);
        Debug.Log("Speed = " + current_speed);
        characterData.CharacterController.Move(move *3+velo * Time.deltaTime);
        
    }
    

    public void Rotation(Vector3 direction)
    {
        float turnSmoothTime = 0.1f;
        float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(characterData.CharacterTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        characterData.CharacterTransform.rotation = Quaternion.Euler(0, angle, 0);

    }
    


}
