using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject scoreValue;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject player;

    private int score;
    private HealthController playerHealthController;


    private void Start()
    {
        playerHealthController = player.GetComponent<HealthController>();
        healthSlider.maxValue = playerHealthController.health;
        healthSlider.value = playerHealthController.health;
    }

    private void Update()
    {
        healthSlider.value = playerHealthController.currentHealth;
    }

    public void incrementScore()
    {
        score += 10;
        scoreValue.GetComponent<TMP_Text>().text = score.ToString();
    }
}