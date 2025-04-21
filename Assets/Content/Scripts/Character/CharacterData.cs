using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CharacterData : MonoBehaviour
{

    [SerializeField]
    private CharacterController characterController;
    

    [SerializeField]
    private Transform characterTransform;
    [SerializeField]
    public CharacterController CharacterController => characterController;
    [SerializeField]
    private float defaultspeed;

    public float DefaultSpeed=>defaultspeed;

    public Transform CharacterTransform => characterTransform;
   
    private Animator animator;
    public float speed;
    public int baseHealth;
    public float baseDamage;
    public Animator Animator => animator;
    public Rigidbody rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    //animator = GetComponent<Animator>();
}
