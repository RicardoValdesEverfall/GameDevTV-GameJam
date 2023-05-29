using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private PlayerController _playerControllerRef;

    // Start is called before the first frame update
    void Start()
    {
        _playerControllerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player2D"))
        {
            if (_playerControllerRef.numberOfCrabs_2D >= 20)
            {
                _playerControllerRef.HandleGameWin();
            }
        }
    }
}
