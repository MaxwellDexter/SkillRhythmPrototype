using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject focus;
    public Vector3 offset;

    private void LateUpdate()
    {
        transform.position = new Vector3(offset.x, offset.y, focus.transform.position.z - offset.z);
    }
}
