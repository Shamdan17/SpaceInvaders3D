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

        if (gameObject.CompareTag("PlayerShip"))
        {
            var explosionGO = Instantiate(explosion,
                transform.position,
                Quaternion.Euler(0, 0, 0));
            Destroy(explosionGO, 1f);
        }

        if (currentHealth <= 0 && isExploding == false)
        {
            isExploding = true;
            var explosionGO = Instantiate(explosion,
                transform.position,
                Quaternion.Euler(0, 0, 0));

            Destroy(gameObject, 0.5f);
            Destroy(explosionGO, 1f);

            // TODO: @gsoykan - if it is playership - show game over label...

            if (!gameObject.CompareTag("PlayerShip"))
            {
                GameObject
                    .FindWithTag("GameController")?
                    .GetComponent<GameController>()?
                    .incrementScore();
            }
        }
    }
}