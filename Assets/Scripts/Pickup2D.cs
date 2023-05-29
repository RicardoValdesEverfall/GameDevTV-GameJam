using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup2D : MonoBehaviour
{
    [SerializeField] private PlayerController _playerControllerRef;
    [SerializeField] private int ID;
    [SerializeField] private float SelfDestructTime;

    // Start is called before the first frame update
    void Start()
    {
        _playerControllerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player2D"))
        {
            _playerControllerRef.Handle2DPickup(ID);
            Destroy(this.gameObject);
        }
    }

    public void SetStartPosition(Vector3 position)
    {
        this.transform.localPosition = position;
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(SelfDestructTime);
        Destroy(this.gameObject);
    }
}
