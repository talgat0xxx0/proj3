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
    private bool is_grounded;
    private bool isGrounded;
    [SerializeField]
    //public Rigidbody rb;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public float jumpForce = 7f;
    public Vector3 jump;

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
        //this.rb = characterData.rb;
        jump=new Vector3(0.0f,2.0f,0.0f);

    }

    public Vector3 Position =>  characterData.CharacterTransform.position;

    public void Move(Vector3 direction)
    {
        
        //Debug.Log(characterData.CharacterController.isGrounded);
        float targetAngle=Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg;
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        
        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Debug.Log("mover"+move);
        if (Input.GetButtonDown("Jump") )
        {
            Debug.Log("movexxxxxxxxxxxxxxxxxxxxxxxxx");
            //Debug.Log(characterData.CharacterController.isGrounded);
            


            characterData.rb.linearVelocity = new Vector3(characterData.rb.linearVelocity.x, 0f, characterData.rb.linearVelocity.z); // reset Y//rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // reset Y
            characterData.rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);//rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);


        }
        move = characterData.CharacterTransform.TransformDirection(move);
        Vector3 forward = characterData.CharacterTransform.TransformDirection(Vector3.forward);
        characterData.CharacterController.Move(move * Speed * Time.deltaTime);
        
    }

    public void Rotation(Vector3 direction)
    {
        float turnSmoothTime = 0.1f;
        float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(characterData.CharacterTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        characterData.CharacterTransform.rotation = Quaternion.Euler(0, angle, 0);

    }
    


}
