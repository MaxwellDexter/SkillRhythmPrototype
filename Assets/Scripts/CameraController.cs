using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject focus;
    public Vector3 offset;
    public float leanAmount;
    public float leanSpeed;

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
    }
}
