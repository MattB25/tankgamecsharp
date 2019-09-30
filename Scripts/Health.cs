using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health;
    public float damage;

    public void RemoveHealth()
    {
        health -= damage; 
        if (health == 0) 
        {
            Destroy(gameObject); 
        }
    }
}
