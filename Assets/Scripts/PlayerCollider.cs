using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public float suckCooldownTime;
    public float suckActiveTime;
    private float suckingTime;
    private float suckCooldown;

    private AudioManagerMini audio;

    public bool Sucking
    {
        get { return suckingTime > 0; }
    }

    public bool CanSuck
    {
        get { return suckCooldown <= 0 && !Sucking; }
    }

    public GameObject sucker;

    private void Start()
    {
        audio = GetComponent<AudioManagerMini>();
    }

    private void Update()
    {
        bool wasSucking = Sucking;
        suckingTime -= Time.deltaTime;
        suckCooldown -= Time.deltaTime;

        if (CanSuck && Input.GetKeyDown(KeyCode.Space))
        {
            suckingTime = suckActiveTime;
            sucker.SetActive(true);
        }
        if (wasSucking && !Sucking)
        {
            suckCooldown = suckCooldownTime;
            sucker.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            audio.Play("Hit");
            other.GetComponent<InteractableThing>().GetDoneSon();
        }
        else if (other.CompareTag("Pickup"))
        {
            audio.Play("Pickup");
            other.GetComponent<InteractableThing>().GetDoneSon();
        }
        else if (other.CompareTag("Trash"))
        {
            if (Sucking)
                audio.Play("Pickup");
            else
                audio.Play("Hit");
            other.GetComponent<InteractableThing>().GetDoneSon();
        }
    }
}
