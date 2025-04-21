using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public interface IHealthComponent 
{
    float Health { get; }
    float MaxHealth { get; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDamage(int damage);

}
