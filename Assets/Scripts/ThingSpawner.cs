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
    public AudioSource music;
    public Tempo tempo;
    private bool musicStarted = false;

    public double spawnDistance;

    public double timeBetweenSpawns;
    private double timeAtLastSpawn;

    private LaneThing laneThing;
    private PlayerMovement movement;

    private void Start()
    {
        laneThing = player.GetComponent<LaneThing>();
        movement = player.GetComponent<PlayerMovement>();
        timeAtLastSpawn = AudioSettings.dspTime;

        tempo.OnTempoBeat += SpawnWave;
        tempo.SetTempo(TempoUtils.FlipBpmInterval(45));
        music.Play();
        tempo.StartTempo();
    }

    private int GetRandomLane()
    {
        return Random.Range(0, laneThing.laneAmount);
    }

    void Update()
    {
        if (timeAtLastSpawn + timeBetweenSpawns < AudioSettings.dspTime)
        {
            

            // update time
            timeAtLastSpawn = AudioSettings.dspTime;
        }
    }

    private void SpawnWave()
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
        for (int i = 0; i < laneThing.laneAmount; i++)
        {
            LaneOptions option = shuffled[i];
            switch (option)
            {
                case LaneOptions.Empty:
                    break;
                case LaneOptions.Obstacle:
                    Spawn(obstaclePrefab, i);
                    break;
                case LaneOptions.Pickup:
                    Spawn(pickupPrefab, i);
                    break;
            }
        }
    }

    private void Spawn(GameObject prefab, int lane)
    {
        GameObject obj = Instantiate(prefab);
        Vector3 pos = player.transform.position;
        Vector2 circlePos = laneThing.GetLanePos(lane);
        obj.transform.position = new Vector3(circlePos.x, circlePos.y, (float)(pos.z + spawnDistance));
        obj.GetComponent<InteractableThing>().SetPlayer(player);
    }
}
