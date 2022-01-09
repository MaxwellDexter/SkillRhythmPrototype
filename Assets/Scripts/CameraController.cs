using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject focus;
    public Vector3 offset;
    public float leanAmount;
    public float leanSpeed;
    public bool rotateWith;
    public float rotateTweenSpeed = 0.1f;

    private void LateUpdate()
    {
        Vector3 middle = new Vector3(0f, 0f, focus.transform.position.z);
        Vector3 heading = focus.transform.position - middle;

        Vector3 lean = middle + (heading.normalized * leanAmount);
        Vector3 current = new Vector3(transform.position.x, transform.position.y, focus.transform.position.z - offset.z);
        //Vector3 desired = new Vector3(focus.transform.position.x, focus.transform.position.y, focus.transform.position.z - offset.z);

        Vector3 newpos = Vector3.Lerp(current, lean, leanSpeed * Time.deltaTime);
        newpos.z = focus.transform.position.z - offset.z;
        transform.position = newpos;
        transform.LookAt(middle);

        if (rotateWith)
        {
            // rotate camera based on player position
            Vector3 currentRot = transform.rotation.eulerAngles;
            currentRot.z = focus.transform.rotation.eulerAngles.z + 90;
            //Vector3 newRot = Vector3.Lerp(transform.rotation.eulerAngles, currentRot, rotateTweenSpeed * Time.deltaTime);
            //transform.rotation = Quaternion.Euler(newRot);
            transform.rotation = Quaternion.Euler(currentRot);
        }
    }
}
