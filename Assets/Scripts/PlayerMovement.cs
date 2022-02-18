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
    private bool invert;

    [SerializeField] private float tiltMod;

    public Tempo subdivisionTempo;
    private SequenceContainer audioClip;

    private void Start()
    {
        laneThing = GetComponent<LaneThing>();
        currentRotation = 0f;
        moveSpeed = 19.8f;//(float)(spawner.spawnDistance / (TempoUtils.FlipBpmInterval(45) * 2));
        subdivisionTempo.OnTempoBeat += FakeMethod;
        audioClip = GetComponent<SequenceContainer>();

        currentRotation = 0f;
        DoRotation();
    }

    private void FakeMethod()
    {

    }

    private void Update()
    {
        // demo stuff
        if (Input.GetKeyDown(KeyCode.I))
            invert = !invert;
        // end demo stuff

        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    // flip rotation degress
        //    currentRotation -= Mathf.PI;
        //    DoRotation();
        //    currentLane = laneThing.GetClosestLane(transform.position);
        //}

        GetRotateInput();

        RotatePlayer();

        Snap();
    }

    private void FixedUpdate()
    {
        // move forward
        transform.Translate(transform.forward * moveSpeed * Time.fixedDeltaTime);
    }

    private void GetRotateInput()
    {
        bool wasMoving = rotationInput != 0f;
        rotationInput = 0f;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotationInput = rotateSpeed * (invert ? -1f : 1f);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotationInput = rotateSpeed * (invert ? 1f : -1f);
        }

        if (wasMoving && rotationInput == 0f)
        {
            shouldSnapNow = true;
        }

        // clamp value from 0 to tau
        // maybe
    }

    private void RotatePlayer()
    {
        if (rotationInput != 0f)
        {
            currentRotation = GetDirection();
            currentRotation += rotationInput * Time.fixedDeltaTime;

            DoRotation();
        }
        RotatePlayerTowardsCenter();
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
    }

    private void RotatePlayerTowardsCenter()
    {
        float inputOffset = -rotationInput * tiltMod;
        transform.eulerAngles = new Vector3(0f, 0f, GetAngleFromRadians(currentRotation) + inputOffset);
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
                DoSnapToLane();
            }
        }
    }

    private void DoSnapToLane()
    {
        Vector2 newpos = laneThing.GetLanePos(currentLane);
        float time = (float)TempoUtils.GetSecondsFromMilliseconds(60);
        transform.DOMoveX(newpos.x, time);
        transform.DOMoveY(newpos.y, time);
    }
}
