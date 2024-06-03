using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroid;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float maximumScale = 1f;
    [SerializeField] private float minimumScale = 0.2f;

    private Vector3 screenCenter;

    private float minimumY;
    private float maximumY;
    private float minimumX;
    private float maximumX;

    // Start is called before the first frame update
    void Start()
    {
        // Grab main camera properties
        var mainCamera = Camera.main;
        var cameraTransformPosition = mainCamera.transform.position;
        screenCenter = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Store screen boundaries
        minimumY = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, -cameraTransformPosition.z)).y;
        maximumY = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, -cameraTransformPosition.z)).y;
        minimumX = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, -cameraTransformPosition.z)).x;
        maximumX = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, -cameraTransformPosition.z)).x;

        StartCoroutine(SpawnAsteroids());
    }

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            InstantiateRandomAsteroid();
        }
    }
    
    // Can be considered as Procedural Generation
    private void InstantiateRandomAsteroid()
    {
        var asteroidsOverlap = true;

        float spawnX = 0;
        float spawnY = 0;

        var scale = UnityEngine.Random.Range(minimumScale, maximumScale);

        do
        {
            var randomValue = UnityEngine.Random.value;
            if (randomValue > 0.75f)
            {
                spawnX = UnityEngine.Random.Range(minimumX - maximumScale - scale, minimumX - minimumScale - scale);
                spawnY = UnityEngine.Random.Range(minimumY, maximumY);
            }
            else if (randomValue > 0.5f)
            {
                spawnX = UnityEngine.Random.Range(maximumX + minimumScale + scale, maximumX + maximumScale + scale);
                spawnY = UnityEngine.Random.Range(minimumY, maximumY);
            }
            else if (randomValue > 0.25f)
            {
                spawnX = UnityEngine.Random.Range(minimumX, maximumX);
                spawnY = UnityEngine.Random.Range(minimumY - maximumScale - scale, minimumY - minimumScale - scale);
            }
            else
            {
                spawnX = UnityEngine.Random.Range(minimumX, maximumX);
                spawnY = UnityEngine.Random.Range(maximumY + minimumScale + scale, maximumY + maximumScale + scale);
            }

            // Avoiding spawning 2 asteroids on top of each other
            var collidersBuffer = new Collider[16]; // TODO: move to field
            var size = Physics.OverlapBoxNonAlloc(
                new Vector3(spawnX, spawnY, 0), new Vector3(1, 1, 1), collidersBuffer);
            asteroidsOverlap = size > 0;
        } while (asteroidsOverlap);

        var asteroidObject = Instantiate(asteroid, new Vector3(spawnX, spawnY, 0), Quaternion.Euler(0, 0, 0));
        // TODO: maybe look at the player...
        asteroidObject.transform.LookAt(screenCenter);
        asteroidObject.transform.localScale = new Vector3(scale, scale, scale);
        
        // scaling health of the asteroid with the scale of the asteroid linearly
        if (asteroidObject.gameObject.TryGetComponent<HealthController>(out var healthController))
        {
            healthController.health = (int)(healthController.health * scale);
            healthController.currentHealth = healthController.health;
        }
    }
}