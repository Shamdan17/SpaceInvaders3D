using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    public Sprite[] animationSprites;

    public float animationTime;

    private SpriteRenderer spriteRenderer;

    private int currentSpriteIndex = 0;

    private float timeSinceLastSpriteChange = 0;

    [SerializeField] private GameObject laser;
    [SerializeField] private float laserInterval = 3f;
    [SerializeField] private float shootingChance = 0.1f;
    
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating("ChangeSprite", this.animationTime, this.animationTime);
        StartCoroutine(ShootLaser());
    }

    private IEnumerator ShootLaser()
    {
        while (true)
        {
            yield return new WaitForSeconds(laserInterval);
            InstantiateLaser();
        }
    }

    private void InstantiateLaser()
    {
        var randomValue = UnityEngine.Random.value;
        if (randomValue < 1 - shootingChance) return;
        var laserOriginTransform = transform;
        Instantiate(laser,
            new Vector3(transform.position.x, transform.position.y, 0),
            Quaternion.Euler(90f, 0, 0));
    }

    private void ChangeSprite()
    {
        spriteRenderer.sprite = animationSprites[currentSpriteIndex];
        currentSpriteIndex = (currentSpriteIndex + 1) % animationSprites.Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
    }
}