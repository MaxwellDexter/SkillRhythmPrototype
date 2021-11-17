using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("Ouch!");
            other.GetComponent<InteractableThing>().GetDoneSon();
        }
        else if (other.CompareTag("Pickup"))
        {
            Debug.Log("Yummy!!");
            other.GetComponent<InteractableThing>().GetDoneSon();
        }
    }
}
