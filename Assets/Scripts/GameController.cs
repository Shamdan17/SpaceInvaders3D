using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject scoreValue;

    private int score;

    public void incrementScore()
    {
        score += 10;
        scoreValue.GetComponent<TMP_Text>().text = score.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
