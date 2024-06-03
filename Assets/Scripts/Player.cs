using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Player : MonoBehaviour
{
    public float speed = 10f;
    public float acceleration = 30.0f;
    public float cur_speed = 0f;
    public float damping = 0.985f;

    public float rotation_degrees = 0.0f;

    // public Projectile laserPrefab;

    // private Projectile laser;

    private void Update()
    {
        Vector3 position = transform.position;

        // Update the position of the player based on the input
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            cur_speed -= acceleration * Time.deltaTime;
            if (cur_speed > 0) {
                cur_speed -= acceleration * Time.deltaTime;
            }
            // position.x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            cur_speed += acceleration * Time.deltaTime;
            if (cur_speed < 0) {
                cur_speed += acceleration * Time.deltaTime;
            }
            // position.x += speed * Time.deltaTime;
        }

        // Apply damping to the speed
        cur_speed *= damping;

        // If cur_speed is less than 0.1, set it to 0
        if (Mathf.Abs(cur_speed) < 0.0f) {
            cur_speed = 0;
        }

        // Clamp the speed to the maximum speed
        cur_speed = Mathf.Clamp(cur_speed, -speed, speed);

        // Rotation depends on cur_speed. -45 degrees when cur_speed is -speed, 45 degrees when cur_speed is speed
        float rotation_degrees_current = -45 * cur_speed / speed;

        // Rotate the player
        Quaternion localRotation = Quaternion.Euler(0f, rotation_degrees_current - rotation_degrees, 0f);
        transform.rotation = transform.rotation * localRotation;
        rotation_degrees = rotation_degrees_current;

        // Update the position of the player
        position.x += cur_speed * Time.deltaTime;
        
        float zcam = (float) this.transform.position.z - Camera.main.transform.position.z;

        // Clamp the position of the character so they do not go out of bounds
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, zcam));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, zcam));
        position.x = Mathf.Clamp(position.x, leftEdge.x, rightEdge.x);

        // Set the new position
        transform.position = position;

        // Only one laser can be active at a given time so first check that
        // there is not already an active laser
        // if (laser == null && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) {
        //     laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        // }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.layer == LayerMask.NameToLayer("Missile") ||
    //         other.gameObject.layer == LayerMask.NameToLayer("Invader")) {
    //         GameManager.Instance.OnPlayerKilled(this);
    //     }
    // }

}

