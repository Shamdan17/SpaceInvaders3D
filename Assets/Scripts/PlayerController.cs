using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private GameObject laser;
    [SerializeField] private GameObject[] cannons;

    private Rigidbody rigidbody;
    private int currentCannon;
    private float time = 0f;
    private Collider[] _colliders;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _colliders = GetComponents<Collider>();
        rigidbody = GetComponent<Rigidbody>();
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
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody.AddForce(transform.forward * (movementSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            rigidbody.AddForce(transform.forward * (-movementSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.down * (rotationSpeed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
        }

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