using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
}
