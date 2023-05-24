using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardButter : MonoBehaviour
{
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3.MoveTowards(transform.position, target, 1f);
    }

    public void SetStartPosition(Vector3 Position)
    {
        this.transform.position = Position;
    }
}
