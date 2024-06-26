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

    [SerializeField] private GameObject explosion;

    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _colliders = GetComponents<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        initialRotation = transform.rotation;

        // Spawn the ship at the bottom of the screen
        var cameraTransformPosition = _camera.transform.position;
        var spawnPosition =
            _camera.ScreenToWorldPoint(new Vector3(UnityEngine.Device.Screen.width / 2,
                UnityEngine.Device.Screen.height / 10,
                -cameraTransformPosition.z));
        transform.position = spawnPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead){
            // Find direction to the center of the screen
            startDeathSequence();
            return;
        }

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
                playerTransform.y,
                0);
        }
    }

    private void startDeathSequence()
    {
        var screenCenter = _camera.ScreenToWorldPoint(
            new Vector3(Screen.width / 2,
                Screen.height / 2,
                1));
        var direction = screenCenter - transform.position;
        this.transform.position += direction.normalized * 0.1f;

        // Rotate the ship around z axis to simulate explosion
        transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));

        // If close enough to screen, explode
        if (Vector3.Distance(transform.position, screenCenter) < 2)
        {
            var explosionGO = Instantiate(explosion,
                transform.position,
                Quaternion.Euler(0, 0, 0));
            Destroy(explosionGO, 1f);

            // Get the label GameOverLabel, set the x position at center of the screen
            var gameOverLabel = GameObject.Find("GameOverLabel");

            // Decrement x of the label by 5000
            gameOverLabel.transform.position = new Vector3(
                gameOverLabel.transform.position.x - 5000,
                gameOverLabel.transform.position.y,
                gameOverLabel.transform.position.z);

            Destroy(gameObject);
        }
    }

    private bool PlayerVisible()
    {
        var collider = _colliders.First(it => it.isTrigger);
        var planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        return GeometryUtility.TestPlanesAABB(planes, collider.bounds);
    }

    public void Die()
    {
        isDead = true;
    }

}