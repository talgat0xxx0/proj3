using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerCharacter : Character
{
    private float speed;
    private float turnSmoothVelocity = 0.1f;
    public float jumpPower = 1f;
    private bool is_grounded;
    private bool isGrounded;
    [SerializeField]
    public LayerMask groundLayer;
    
    public float groundCheckRadius = 0.2f;
    public float jumpForce = 7f;
    public Vector3 jump;    
    public Animator animator;
    public float debugWireSphere_Radius;
    public Color debugWireSphere_Color;
    
    // Update is called once per frame
    public ScrollView scrollv;
    void OnDrawGizmos()
    {   
        Gizmos.color = debugWireSphere_Color;
        Gizmos.DrawWireSphere(transform.position, debugWireSphere_Radius);
       
    }
    
    protected override void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        DebugExtension.DebugWireSphere(transform.position, debugWireSphere_Color, debugWireSphere_Radius);
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput,  0,verticalInput).normalized;
        
        if(moveDirection.magnitude>0.01f){
            MovementComponent.Move(moveDirection*Time.fixedDeltaTime);
        }else{
            animator.SetFloat("speed",0f);
        }
           
        
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
