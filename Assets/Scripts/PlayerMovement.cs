using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    public int currentLane = 0;

    private Rigidbody rigidBody;
    private LaneThing laneThing;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        laneThing = GetComponent<LaneThing>();
    }

    private void Update()
    {
        rigidBody.velocity = new Vector3(0f, 0f, moveSpeed) * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            currentLane = laneThing.ChangeLanes(false);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            currentLane = laneThing.ChangeLanes(true);
    }
}
