using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int health = 100;
    public int currentHealth;

    [SerializeField] private GameObject explosion;

    private bool isExploding = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DealDamage(int damage)
    {
        currentHealth = Math.Max(0, currentHealth - damage);

        if (currentHealth <= 0 && isExploding == false)
        {
            isExploding = true;

            var explosionGO = Instantiate(explosion,
                transform.position,
                Quaternion.Euler(0, 0, 0));
            
            Destroy(gameObject, 0.5f);
            Destroy(explosionGO, 1f);
        }
    }
}