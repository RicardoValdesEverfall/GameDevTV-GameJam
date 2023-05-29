using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player2D : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CrabCounter;
    [SerializeField] private Image[] PlayerLives;
    [SerializeField] private SpriteRenderer crabGFX;
    [SerializeField] private Sprite goldenCrab;

    [SerializeField] private GameObject gameOver;

    private Sprite normalCrab;
    private PlayerController _playerControllerRef;

    void Start()
    {
        _playerControllerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        normalCrab = crabGFX.sprite;
    }

    private void Update()
    {
        int crabCount = _playerControllerRef.numberOfCrabs_2D;

       

        CrabCounter.text = crabCount.ToString();
        if (crabCount >= 20) { crabGFX.sprite = goldenCrab; }
        else
        {
            crabGFX.sprite = normalCrab;

            if (crabCount >= 15) { CrabCounter.color = Color.green; }
            if (crabCount <= 10) { CrabCounter.color = Color.yellow; }
            if (crabCount <= 5) { CrabCounter.color = Color.red; }
        }  
    }

    public void LoseALife()
    {
        int i = _playerControllerRef.playerLives_2D;
        if (i < 0) { return; }

        PlayerLives[i].enabled = false;
    }

    public void RestartGame()
    {
        _playerControllerRef.Handle2DGameOver();
    }
}
