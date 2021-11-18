using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    public int currentLane = 0;

    public float rotateSpeed = 0.1f;
    private float currentRotation;

    private Rigidbody rigidBody;
    private LaneThing laneThing;
    public ThingSpawner spawner;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        laneThing = GetComponent<LaneThing>();
        currentRotation = 0f;
        moveSpeed = 19.8f;//(float)(spawner.spawnDistance / (TempoUtils.FlipBpmInterval(45) * 2));
    }

    private void Update()
    {
        // move forward
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime);

        if (GetRotateInput())
            DoRotation();
    }

    private bool GetRotateInput()
    {
        bool moved = false;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentRotation -= rotateSpeed * Time.deltaTime;
            moved = true;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentRotation += rotateSpeed * Time.deltaTime;
            moved = true;
        }

        // clamp value from 0 to tau
        // maybe

        return moved;
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
        transform.eulerAngles = new Vector3(0f, 0f, GetAngleFromRadians(a));
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
}
