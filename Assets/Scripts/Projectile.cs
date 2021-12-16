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
        InteractableThing interact = other.GetComponent<InteractableThing>();
        if (interact != null)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
                enemy.DestroyEnemy();
            else
                interact.GetDoneSon();
            GetDoneSon();
        }
    }

    private void Update()
    {
        if (transform.position.z > player.transform.position.z + 60)
            GetDoneSon();
    }
}
