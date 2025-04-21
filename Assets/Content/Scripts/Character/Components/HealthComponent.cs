using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : IHealthComponent
{
    private float health;
    private float maxHealth=100;

    public float MaxHealth => maxHealth;
    
    public float Health {
        get
        {
            return health;
        }
        private set
        {
            health = Mathf.Clamp(value,min:0,MaxHealth);
        }
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDamage(int damage)
    {
        Debug.Log("damage!!!");
        Health -= damage;
    }
}
