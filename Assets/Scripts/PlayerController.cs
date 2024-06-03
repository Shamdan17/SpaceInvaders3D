using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private bool moveOnlyHorizontal = true;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject[] cannons;
    [SerializeField] private float maxTiltAngle = 60f;

    private Rigidbody rigidbody;
    private int currentCannon;
    private float time = 0f;
    private Collider[] _colliders;
    private Camera _camera;
    private Quaternion initialRotation;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _colliders = GetComponents<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0f)
        {
            time -= Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            var laserOriginTransform = transform;
            if (cannons.Length > 0)
            {
                laserOriginTransform = cannons[currentCannon++ % cannons.Length].transform;
            }

            Instantiate(laser,
                laserOriginTransform.TransformPoint(Vector3.forward * 2),
                transform.rotation);
            time = cooldown;
        }

        // Player
        float tilt = 0f;
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow) && !moveOnlyHorizontal)
        {
            movement += transform.forward * (movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.DownArrow) && !moveOnlyHorizontal)
        {
            movement = transform.forward * (-movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (moveOnlyHorizontal)
            {
                movement = transform.right * (-movementSpeed * Time.deltaTime);
                tilt = maxTiltAngle;
            }
            else
            {
                transform.Rotate(Vector3.down * (rotationSpeed * Time.deltaTime));
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (moveOnlyHorizontal)
            {
                movement = transform.right * (movementSpeed * Time.deltaTime);
                tilt = -maxTiltAngle;
            }
            else
            {
                transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
            }
        }

        rigidbody.AddForce(movement);

        // Apply tilt effect based on horizontal movement
        float targetTilt = Mathf.Lerp(0, tilt,
            Mathf.Abs(movementSpeed * Time.deltaTime));
        Quaternion tiltRotation = Quaternion.Euler(0, 0, targetTilt);
        transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation * tiltRotation,
            Time.deltaTime * rotationSpeed);


        // Move through screen borders
        if (!PlayerVisible())
        {
            var playerTransform = transform.position;
            var screenCenter = _camera.ScreenToWorldPoint(
                new Vector3(Screen.width / 2,
                    Screen.height / 2,
                    0));
            transform.position = new Vector3(screenCenter.x - playerTransform.x,
                screenCenter.y - playerTransform.y,
                0);
        }
    }

    private bool PlayerVisible()
    {
        var collider = _colliders.First(it => it.isTrigger);
        var planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        return GeometryUtility.TestPlanesAABB(planes, collider.bounds);
    }
}