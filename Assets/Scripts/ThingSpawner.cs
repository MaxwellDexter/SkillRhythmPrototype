using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingSpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject obstaclePrefab;

    public float spawnDistance;
    public int obstacleSpawnAmount;

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
            for (int i = 0; i < obstacleSpawnAmount; i++)
                Spawn(obstaclePrefab);
            timeAtLastSpawn = Time.time;
        }
    }

    private void Spawn(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        Vector3 pos = player.transform.position;
        obj.transform.position = new Vector3(laneThing.GetLanePos(GetRandomLane()), pos.y, pos.z + spawnDistance);
        obj.GetComponent<InteractableThing>().SetPlayer(player);
    }
}
