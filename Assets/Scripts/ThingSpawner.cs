using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ThingSpawner : MonoBehaviour
{
    public enum LaneOptions
    {
        Obstacle,
        Pickup,
        Empty,
        Trash,
        Enemy
    }

    public GameObject player;
    public GameObject obstaclePrefab;
    public GameObject pickupPrefab;
    public GameObject trashPrefab;
    public GameObject enemyPrefab;
    public AudioSource music;
    public Tempo tempo;
    public Tempo subdivisionTempo;

    private bool spawnEnemy = false;

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
        subdivisionTempo.SetTempo(TempoUtils.FlipBpmInterval(180 * 4));
        music.Play();
#if UNITY_EDITOR
        StartCoroutine(StartTempo());
#endif
#if UNITY_STANDALONE
        tempo.StartTempo();
        subdivisionTempo.StartTempo();
#endif
    }

    private IEnumerator StartTempo()
    {
        yield return new WaitForSeconds(.01f);
        tempo.StartTempo();
        subdivisionTempo.StartTempo();
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

        if (!music.isPlaying)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void SpawnWave()
    {
        List<LaneOptions> options = new List<LaneOptions>();

        if (spawnEnemy)
        {
            options.Add(LaneOptions.Enemy);
            options.Add(LaneOptions.Empty);
            spawnEnemy = false;
        }
        else
        {
            float randVal = Random.Range(0f, 1f);
            if (randVal > 0.7)
            {
                options.Add(LaneOptions.Trash);
                spawnEnemy = true;
            }
            else
            {
                options.Add(LaneOptions.Pickup);
            }
        }

        // add obstacles to fill the space
        int obstacleAmount = laneThing.laneAmount - options.Count;
        for (int i = 0; i < obstacleAmount; i++)
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
                case LaneOptions.Obstacle:
                    Spawn(obstaclePrefab, i);
                    break;
                case LaneOptions.Pickup:
                    Spawn(pickupPrefab, i);
                    break;
                case LaneOptions.Trash:
                    Spawn(trashPrefab, i);
                    break;
                case LaneOptions.Enemy:
                    Spawn(enemyPrefab, i);
                    break;
                default:
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
