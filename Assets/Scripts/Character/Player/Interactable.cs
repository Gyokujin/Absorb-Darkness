using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interact")]
    public float interactRadius = 1f;
    public string interactableText;

    public virtual void Interact(PlayerManager playerManager)
    {
        Debug.Log("Interact");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}