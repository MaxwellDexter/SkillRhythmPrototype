using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThingSpawner : MonoBehaviour
{
    public enum LaneOptions
    {
        Obstacle,
        Pickup,
        Empty
    }

    public GameObject player;
    public GameObject obstaclePrefab;
    public GameObject pickupPrefab;

    public float spawnDistance;

    public float timeBetweenSpawns;
    private float timeAtLastSpawn;

    private LaneThing laneThing;
    private PlayerMovement movement;

    private void Start()
    {
        laneThing = player.GetComponent<LaneThing>();
        movement = player.GetComponent<PlayerMovement>();
        timeAtLastSpawn = Time.time;
    }

    private int GetRandomLane()
    {
        return Random.Range(0, laneThing.laneAmount);
    }

    void Update()
    {
        if (timeAtLastSpawn + timeBetweenSpawns < Time.time)
        {
            // make a list with an empty space and a pick up
            List<LaneOptions> options = new List<LaneOptions> { LaneOptions.Pickup, LaneOptions.Empty };
            // add obstacles to fill the space
            for (int i = 0; i < laneThing.laneAmount - 2; i++)
                options.Add(LaneOptions.Obstacle);
            // shuffle it
            // from https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/
            List<LaneOptions> shuffled = options.OrderBy(x => Random.value).ToList();

            // sanity check
            if (shuffled.Count != laneThing.laneAmount)
                Debug.LogError($"The shuffled list had a different amount ({shuffled.Count}) to the lane amount ({laneThing.laneAmount})!");

            // spawn the things
            foreach (LaneOptions option in shuffled)
            {
                switch (option)
                {
                    case LaneOptions.Empty:
                        break;
                    case LaneOptions.Obstacle:
                        Spawn(obstaclePrefab);
                        break;
                    case LaneOptions.Pickup:
                        Spawn(pickupPrefab);
                        break;
                }
            }

            // update time
            timeAtLastSpawn = Time.time;
        }
    }

    private void Spawn(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        Vector3 pos = player.transform.position;
        Vector2 circlePos = laneThing.GetLanePos(GetRandomLane());
        obj.transform.position = new Vector3(circlePos.x, circlePos.y, pos.z + spawnDistance);
        obj.GetComponent<InteractableThing>().SetPlayer(player);
    }
}
