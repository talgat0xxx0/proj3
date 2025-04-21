using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;

public class objectsControl : MonoBehaviour
{
    public GameObject myobject,secondObject;
    
    GameObject[] objects = new GameObject[2];
    
    public Transform objtransform;
    public float speed = 0;
    

    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        float horisontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(vertical, 0, horisontal);

        

        myobject = GameObject.Find("Cylinder");
        secondObject = GameObject.Find("Cube");
        objects[0] = myobject;
        objects[1] = secondObject;
        for (int i = 0; i < 2; i++)
        {
            
            direction.Normalize();
            objects[i].transform.position = objects[i].transform.position + direction*speed;
        }


    }
}
