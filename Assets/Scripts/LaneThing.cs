using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LaneThing : MonoBehaviour
{
    public int laneAmount;

    public float maxWidth;

    /// <summary>The XY position of the lanes</summary>
    private List<Vector2> lanes;

    private void Start()
    {
        lanes = new List<Vector2>();
        UpdateLanes();
    }

    private void UpdateLanes()
    {
        lanes.Clear();
        // from https://stackoverflow.com/questions/839899/how-do-i-calculate-a-point-on-a-circle-s-circumference
        // x = cx + r * cos(a)
        // y = cy + r * sin(a)
        // Where r is the radius, cx,cy the origin, and a the angle (in radians).
        float r = maxWidth / 2;
        float cx = 0;
        float cy = 0;
        // circle
        float laneWidthRadians = 2 * Mathf.PI / laneAmount;
        // half circle
        //float laneWidthRadians = Mathf.PI / laneAmount;

        for (int i = 0; i < laneAmount; i++)
        {
            float a = laneWidthRadians * i;
            float x = cx + r * Mathf.Cos(a);
            float y = cy + r * Mathf.Sin(a);
            lanes.Add(new Vector2(x, y));
        }
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        // draw lanes
        float laneLength = 15f;
        foreach(Vector2 startPos in lanes)
            Debug.DrawLine(
                new Vector3(startPos.x, startPos.y, pos.z),
                new Vector3(startPos.x, startPos.y, pos.z + laneLength),
                Color.red, 0f, false);

        if (laneAmount != lanes.Count)
            UpdateLanes();
    }

    private int GetNewLane(int laneNum, bool wantToMoveRight)
    {
        int newLane = laneNum + (wantToMoveRight ? 1 : -1);
        if (newLane >= laneAmount)
            newLane = 0;
        else if (newLane < 0)
            newLane = laneAmount - 1;
        return newLane;
    }

    public int ChangeLanes(bool wantToMoveRight)
    {
        int newLane = GetNewLane(GetComponent<PlayerMovement>().currentLane, wantToMoveRight);
        Vector2 pos = lanes[newLane];
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
        return newLane;
    }

    public Vector2 GetLanePos(int index)
    {
        if (index < 0 || index >= laneAmount)
        {
            Debug.LogWarning("Something tried to get a bad lane index");
            return lanes[0];
        }
        else
        {
            return lanes[index];
        }
    }
}
