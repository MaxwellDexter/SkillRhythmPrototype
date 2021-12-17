using UnityEngine;
using DG.Tweening;

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

    public GameObject projectilePrefab;
    public GameObject projectileVisualiser;

    private AudioManagerMini audioMan;

    private ScoreManager score;

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
        score = GameObject.Find("Score Manager").GetComponent<ScoreManager>();
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
        else if (wasPreviouslyHeldDown && !inputHeldDown && holdingSomething) // ended
        {
            holdingSomething = false;
            if (Time.time - timeAtKeyDown < tapSpeed)
            {
                // tap
                //audioMan.Play("Swallow");
            }
            else
            {
                // held
                audioMan.Play("Shoot");
                GameObject projectile = Instantiate(projectilePrefab);
                projectile.transform.position = transform.position;
                projectile.GetComponent<Projectile>().SetPlayer(gameObject);
            }
        }

        projectileVisualiser.SetActive(holdingSomething);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            DoHit();
            other.GetComponent<InteractableThing>().DestroyMe();
        }
        else if (other.CompareTag("Pickup"))
        {
            if (Sucking)
                DoPickup();
            else
                DoHit();
            other.GetComponent<InteractableThing>().DestroyMe();
        }
        else if (other.CompareTag("Trash"))
        {
            if (Sucking)
            {
                DoPickup();
                holdingSomething = true;
            }
            else
                DoHit();
            other.GetComponent<InteractableThing>().DestroyMe();
        }
    }

    private void DoPickup()
    {
        audioMan.Play("Pickup");
        score.Add();
    }

    private void DoHit()
    {
        Camera.main.DOShakePosition(0.2f, 0.1f, 20);
        audioMan.Play("Hit");
        score.ResetStreak();
    }
}
