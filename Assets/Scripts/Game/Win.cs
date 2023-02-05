using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Win : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(other.name + " WINS!");
            animator.SetTrigger("CloseLid");
        }
    }
}
