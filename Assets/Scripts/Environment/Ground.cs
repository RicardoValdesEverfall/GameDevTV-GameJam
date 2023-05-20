using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool OnGround;
    public float Friction;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnGround = false;
        Friction = 0;
    }

    private void EvaluateCollision(Collision2D collision)
    {
        for (int i=0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            OnGround |= normal.y >= 0.9f;
        }
    }

    private void RetrieveFriction(Collision2D collision)
    {
        PhysicsMaterial2D material = collision.rigidbody.sharedMaterial;
        Friction = 0;

        if (material != null) { Friction = material.friction; }
    }
}
