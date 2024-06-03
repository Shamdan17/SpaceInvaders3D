using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private bool hurtPlayer = false;
    
    
    void Update()
    {
        transform.
            Translate(Vector3.forward 
                      * (speed * Time.deltaTime));

        if (transform.position.y > 35 || transform.position.z < -35)
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("PlayerShip") && !hurtPlayer) return;
        if (other.gameObject.CompareTag("Invader") && hurtPlayer) return;
        // print other.gameObject.tag
        if (other.gameObject.CompareTag("Asteroid")) return;
        print(other.gameObject.tag); 
        if (other.gameObject.TryGetComponent<HealthController>(out var healthController))
        {
            healthController.DealDamage(damage);
        }
        Destroy(gameObject);
    }
}
