using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private float step;

    [SerializeField] public AudioSource Environment2D;
    [SerializeField, Range(0.0f, 1.0f)] private float MaxVolume2D;
    [SerializeField, Range(0.0f, 0.8f)] private float MinVolume2D;

    [SerializeField] public AudioSource Environment3D;
    [SerializeField, Range(0.0f, 1.0f)] private float MaxVolume3D;
    [SerializeField, Range(0.0f, 0.8f)] private float MinVolume3D;

    private PlayerController _playerControllerRef;

    // Start is called before the first frame update
    void Start()
    {
        if (_playerControllerRef == null) { _playerControllerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerControllerRef.PlayerCurrentState == PlayerController.PlayerState.is2D)
        {
            Handle2DAudio(1);
            Handle3DAudio(-1);
        }

        if (_playerControllerRef.PlayerCurrentState == PlayerController.PlayerState.is3D)
        {
            Handle2DAudio(-1);
            Handle3DAudio(1);
        }
    }

    private void Handle2DAudio(int increaseOrDecrease) //1 = increase, -1 = decrease
    {
        if (increaseOrDecrease > 0)
        {
            if (Environment2D.volume < MaxVolume2D) { Environment2D.volume = Mathf.Lerp(Environment2D.volume, MaxVolume2D, step * Time.deltaTime); }
        }
        else { Environment2D.volume = MinVolume2D; }
    }

    private void Handle3DAudio(int increaseOrDecrease) //1 = increase, -1 = decrease
    {
        if (increaseOrDecrease > 0)
        {
            if (Environment3D.volume < MaxVolume3D) { Environment3D.volume = Mathf.Lerp(Environment3D.volume, MaxVolume3D, step * Time.deltaTime); }
        }
        else
        {
            if (Environment3D.volume > MinVolume2D) { Environment3D.volume = Mathf.Lerp(Environment3D.volume, MinVolume3D, step * Time.deltaTime); }
        }
    }
}
