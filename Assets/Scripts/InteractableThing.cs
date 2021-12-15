using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableThing : MonoBehaviour
{
    protected GameObject player;

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    private void Update()
    {
        if (transform.position.z < player.transform.position.z - 10)
            GetDoneSon();
    }

    public virtual void GetDoneSon()
    {
        Destroy(gameObject);
    }
}
