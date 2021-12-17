using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    public int currentLane = 0;

    public float rotateSpeed = 0.1f;
    private float currentRotation;
    private float rotationInput = 0f;

    private LaneThing laneThing;
    public ThingSpawner spawner;

    [SerializeField]
    private bool snap;
    private bool shouldSnapNow;
    [SerializeField]
    private float snapSpeed;

    [SerializeField]
    private float tapSpeed;
    private float tapTime;

    [SerializeField]
    private bool quantise;
    private float inputDirection;
    public Tempo subdivisionTempo;
    private SequenceContainer audioClip;

    private void Start()
    {
        laneThing = GetComponent<LaneThing>();
        currentRotation = 0f;
        moveSpeed = 19.8f;//(float)(spawner.spawnDistance / (TempoUtils.FlipBpmInterval(45) * 2));
        subdivisionTempo.OnTempoBeat += DoQuantiseMovement;
        audioClip = GetComponent<SequenceContainer>();
    }

    private void Update()
    {
        // demo stuff
        if (Input.GetKeyDown(KeyCode.S))
            snap = !snap;
        if (Input.GetKeyDown(KeyCode.Q))
            quantise = !quantise;
        // end demo stuff

        if (quantise)
        {
            float rawInput = Input.GetAxisRaw("Horizontal");
            if (rawInput != 0f)
                inputDirection = rawInput;
            return;
        }

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    // flip rotation degress
        //    currentRotation -= Mathf.PI;
        //    DoRotation();
        //    currentLane = laneThing.GetClosestLane(transform.position);
        //}

        if (!snap)
        {
            GetRotateInput();
            return;
        }

        // on key down start timer
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
            tapTime = Time.time;

        bool shouldRotate = true;
        // else check for key up and if less than speed do tap
        if (Time.time - tapTime < tapSpeed)
            shouldRotate = TapReleaseCheck();
        // but rotate anyway
        if (shouldRotate)
            GetRotateInput();
    }

    private void FixedUpdate()
    {
        // move forward
        transform.Translate(transform.forward * moveSpeed * Time.fixedDeltaTime);

        if (quantise) return;

        RotatePlayer();

        Snap();
    }

    private void GetRotateInput()
    {
        bool wasMoving = rotationInput != 0f;
        rotationInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotationInput = -rotateSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotationInput = rotateSpeed;
        }

        if (wasMoving && rotationInput == 0f)
        {
            shouldSnapNow = true;
        }

        // clamp value from 0 to tau
        // maybe
    }

    private bool TapReleaseCheck()
    {
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            currentLane = laneThing.ChangeLanes(currentLane, false);
            shouldSnapNow = false;
            rotationInput = 0f;
            return false;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            currentLane = laneThing.ChangeLanes(currentLane, true);
            shouldSnapNow = false;
            rotationInput = 0f;
            return false;
        }
        return true;
    }

    private void RotatePlayer()
    {
        if (rotationInput == 0f)
            return;
        currentRotation = GetDirection();
        currentRotation += rotationInput * Time.fixedDeltaTime;

        DoRotation();
    }

    private void DoRotation()
    {
        // from https://stackoverflow.com/questions/839899/how-do-i-calculate-a-point-on-a-circle-s-circumference
        // x = cx + r * cos(a)
        // y = cy + r * sin(a)
        // Where r is the radius, cx,cy the origin, and a the angle (in radians).
        float cx = 0f; // get this from somewhere else
        float cy = 0f;
        float r = laneThing.maxWidth / 2;
        float a = currentRotation;
        float x = cx + r * Mathf.Cos(a);
        float y = cy + r * Mathf.Sin(a);
        transform.position = new Vector3(x, y, transform.position.z);
        RotatePlayerTowardsCenter();
    }

    private void RotatePlayerTowardsCenter()
    {
        transform.eulerAngles = new Vector3(0f, 0f, GetAngleFromRadians(currentRotation));
    }

    private float GetDirection()
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 centre = Vector2.zero;
        var heading = pos - centre;
        return Mathf.Deg2Rad * Angle(heading);
    }

    private float Angle(Vector3 v)
    {
        // normalize the vector: this makes the x and y components numerically
        // equal to the sine and cosine of the angle:
        v.Normalize();
        // get the basic angle:
        var ang = Mathf.Asin(v.y) * Mathf.Rad2Deg;
        // fix the angle for 2nd and 3rd quadrants:
        if (v.x < 0)
        {
            ang = 180 - ang;
        }
        else // fix the angle for 4th quadrant:
        if (v.y < 0)
        {
            ang = 360 + ang;
        }
        return ang;
    }

    private float GetAngleFromRadians(float radians)
    {
        // clamp radians
        float clamped = radians % (2 * Mathf.PI);
        // convert to positive
        if (clamped < 0)
            clamped *= -1;
        // convert to degrees
        return clamped * Mathf.Rad2Deg;
    }

    private void Snap()
    {
        if (snap)
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            if (shouldSnapNow)
            {
                currentLane = laneThing.GetClosestLane(pos);
                shouldSnapNow = false;
            }
            if (rotationInput == 0f) // if not moving
            {
                Vector3 newpos = Vector3.Lerp(pos, laneThing.GetLanePos(currentLane), snapSpeed * Mathf.Clamp01(Time.fixedDeltaTime));
                newpos.z = transform.position.z;
                transform.position = newpos;
            }
        }
    }

    private void DoQuantiseMovement()
    {
        if (inputDirection != 0f)
        {
            audioClip.Play();
            bool goingRight = inputDirection < 0;
            currentLane = laneThing.ChangeLanes(currentLane, goingRight);
            Vector3 newpos = laneThing.GetLanePos(currentLane);
            //newpos.z = transform.position.z;
            float time = (float)TempoUtils.GetSecondsFromMilliseconds(60);
            transform.DOMoveX(newpos.x, time);
            transform.DOMoveY(newpos.y, time);

            // rotate
            currentRotation = GetDirection();
            RotatePlayerTowardsCenter();

            // reset it
            inputDirection = 0f;
        }
    }
}
