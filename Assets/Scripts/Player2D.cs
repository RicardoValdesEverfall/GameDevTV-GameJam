using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2D : MonoBehaviour
{
    private PlayerController _playerControllerRef;

    // Start is called before the first frame update
    void Start()
    {
        _playerControllerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void AnimFinish2D()
    {
        _playerControllerRef.Handle2DGameOver();
    }
}
