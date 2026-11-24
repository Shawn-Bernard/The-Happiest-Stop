using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] Animator animator;
    private  List<Collider> insideCollider = new List<Collider>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Customer"))
        {
            insideCollider.Add(other);
            SetDoor(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Customer"))
        {
            insideCollider.Remove(other);
            SetDoor(false);
        }
    }

    public void SetDoor(bool doorStatus)
    {
        if (!doorStatus && insideCollider.Count > 0) return;
        animator.SetBool("isOpen", doorStatus);
    }
}
