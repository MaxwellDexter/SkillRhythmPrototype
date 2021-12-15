using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : InteractableThing
{
    public float moveSpeed;

    private void FixedUpdate()
    {
        transform.Translate(transform.forward * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var interact = other.GetComponent<InteractableThing>();
        if (interact != null)
        {
            interact.GetDoneSon();
            GetDoneSon();
            // todo spawn audio player?
        }
    }

    private void Update()
    {
        if (transform.position.z > player.transform.position.z + 60)
            GetDoneSon();
    }
}
