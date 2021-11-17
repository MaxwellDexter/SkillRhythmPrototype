using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject focus;
    public Vector3 offset;

    private void Update()
    {
        transform.position = focus.transform.position - offset;
    }
}
