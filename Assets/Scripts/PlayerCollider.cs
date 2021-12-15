using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    public float suckCooldownTime;
    public float suckActiveTime;
    private float suckingTime;
    private float suckCooldown;

    private bool holdingSomething;
    private bool inputHeldDown = false;
    private float timeAtKeyDown;
    [SerializeField] private float tapSpeed;

    private AudioManagerMini audioMan;

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
        audioMan = GetComponent<AudioManagerMini>();
    }

    private void Update()
    {
        bool wasSucking = Sucking;
        suckingTime -= Time.deltaTime;
        suckCooldown -= Time.deltaTime;

        if (CanSuck && (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)))
        {
            suckingTime = suckActiveTime;
            sucker.SetActive(true);
        }
        if (wasSucking && !Sucking)
        {
            suckCooldown = suckCooldownTime;
            sucker.SetActive(false);
        }

        bool wasPreviouslyHeldDown = inputHeldDown;
        inputHeldDown = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space);
        if (!wasPreviouslyHeldDown && inputHeldDown && Sucking) // started
        {
            timeAtKeyDown = Time.time;
        }
        else if (wasPreviouslyHeldDown && !inputHeldDown) // ended
        {
            if (Time.time - timeAtKeyDown < tapSpeed)
            {
                // tap
                audioMan.Play("Hit");
            }
            else
            {
                // held
                audioMan.Play("Pickup");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            audioMan.Play("Hit");
            other.GetComponent<InteractableThing>().GetDoneSon();
        }
        else if (other.CompareTag("Pickup"))
        {
            audioMan.Play("Pickup");
            other.GetComponent<InteractableThing>().GetDoneSon();
        }
        else if (other.CompareTag("Trash"))
        {
            if (Sucking)
            {
                audioMan.Play("Pickup");
                holdingSomething = true;
            }
            else
            other.GetComponent<InteractableThing>().GetDoneSon();
        }
    }
}
