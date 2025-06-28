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
    [SerializeField]
    public Animator animator;
    public float speed;
    public int baseHealth;
    public float baseDamage;
    public Animator Animator => animator;
    public Rigidbody rb;
    
    
    [SerializeField]
    public Transform groundCheck;
    
}
