using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneThing : MonoBehaviour
{
    public int laneAmount;

    public float maxWidth;

    /// <summary>The X position of the lanes</summary>
    private List<float> lanes;

    private void Start()
    {
        lanes = new List<float>();
        UpdateLanes();
    }

    private void UpdateLanes()
    {
        lanes.Clear();
        float laneWidth = maxWidth / laneAmount;
        float laneStart = -(maxWidth / 2);

        for (int i = 0; i < laneAmount; i++)
        {
            float startPos = laneStart + (laneWidth * i) + (laneWidth / 2);
            lanes.Add(startPos);
        }
    }

    private void Update()
    {
        float halfWidth = maxWidth / 2;
        float laneStart = -halfWidth;
        float laneEnd = halfWidth;

        Vector3 pos = transform.position;
        // draw width of lanes
        Debug.DrawLine(new Vector3(laneStart, pos.y, pos.z), new Vector3(laneEnd, pos.y, pos.z), Color.red, 0f, false);
        // draw lanes
        float laneLength = 15f;
        foreach(float startPos in lanes)
        {
            Debug.DrawLine(new Vector3(startPos, pos.y, pos.z), new Vector3(startPos, pos.y, pos.z + laneLength), Color.red, 0f, false);
        }

        if (laneAmount != lanes.Count)
            UpdateLanes();
    }

    private int GetNewLane(int laneNum, bool wantToMoveRight)
    {
        return laneNum + (wantToMoveRight ? 1 : -1);
    }

    private bool CanMove(int laneNum, bool wantToMoveRight)
    {
        int newLane = GetNewLane(laneNum, wantToMoveRight);
        if (newLane < 0 || newLane >= laneAmount)
            return false;
        return true;
    }

    public int ChangeLanes(bool wantToMoveRight)
    {
        int currentLane = GetComponent<PlayerMovement>().currentLane;
        if (CanMove(currentLane, wantToMoveRight))
        {
            int newLane = GetNewLane(currentLane, wantToMoveRight);
            transform.position = new Vector3(lanes[newLane], transform.position.y, transform.position.z);
            return newLane;
        }
        else
            return currentLane;
    }
}
