using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] public int ID; //0 = butter, 1 = Cleaver

    public float MoveSpeed;
    private Vector3 target;
    private PlayerController _playerControllerRef;

    // Start is called before the first frame update
    void Start()
    {
        _playerControllerRef = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ID == 0)
        {
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, target, MoveSpeed);
        }
       
    }

    public void SetStartPosition(Vector3 Position)
    {
        this.transform.localPosition = Position;
    }

    public void SetTargetPosition(Vector3 Target)
    {
        target = Target;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player2D"))
        {
            _playerControllerRef.Handle2DHazard(ID);
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
