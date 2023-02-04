using System.Linq;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [HideInInspector] public bool onGround = true;

    private void OnCollisionEnter(Collision collision)
    {
    }

    private void OnCollisionExit(Collision collision)
    {
    }
}
