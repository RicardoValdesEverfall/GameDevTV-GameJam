using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float MoveSpeed;
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, target, MoveSpeed);
    }

    public void SetStartPosition(Vector3 Position)
    {
        this.transform.localPosition = Position;
    }

    public void SetTargetPosition(Vector3 Target)
    {
        target = Target;
    }
}
