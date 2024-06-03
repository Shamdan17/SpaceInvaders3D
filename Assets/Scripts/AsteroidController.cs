using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public int damage;
    [SerializeField] private float damageCooldown = 0.5f;
    private float currentTime;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * 100f);
    }

    private void OnCollisionStay(Collision other)
    {
        if (!other.gameObject.CompareTag("PlayerShip")) return;

        if (currentTime <= 0f)
        {
            other.gameObject.GetComponent<HealthController>()?.DealDamage(damage);
            currentTime = damageCooldown;
        }
        else
        {
            currentTime -= Time.deltaTime;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (!other.gameObject.CompareTag("PlayerShip")) return;

        currentTime = 0f;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}