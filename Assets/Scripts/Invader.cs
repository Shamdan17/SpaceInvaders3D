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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating("ChangeSprite", this.animationTime, this.animationTime);
    }

    private void ChangeSprite()
    {
        spriteRenderer.sprite = animationSprites[currentSpriteIndex];
        currentSpriteIndex = (currentSpriteIndex + 1) % animationSprites.Length;
    }


    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
