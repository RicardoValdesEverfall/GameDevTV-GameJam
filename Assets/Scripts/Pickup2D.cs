using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup2D : MonoBehaviour
{
    [SerializeField] private PlayerController _playerControllerRef;
    [SerializeField] private int ID;
     
    // Start is called before the first frame update
    void Start()
    {
        _playerControllerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player2D"))
        {
            _playerControllerRef.Handle2DPickup(ID);
            this.gameObject.SetActive(false);
        }
    }
}
