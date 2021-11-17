using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableThing : MonoBehaviour
{
    private GameObject player;

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    void Update()
    {
        if (transform.position.z < player.transform.position.z - 10)
            GetDoneSon();
    }

    public void GetDoneSon()
    {
        Destroy(gameObject);
    }
}
