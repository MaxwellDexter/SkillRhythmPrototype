using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : InteractableThing
{
    public AudioClip destroyClip;
    public GameObject destroyObject;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0.5f, 0));
        if (transform.position.z < player.transform.position.z - 10)
            GetDoneSon();
    }

    public void DestroyEnemy()
    {
        GameObject.Find("Score Manager").GetComponent<ScoreManager>().AddBonus();
        SFXTempPlayer.Create(destroyClip, 0.9f);
        GameObject fx = Instantiate(destroyObject);
        fx.transform.position = transform.position;
        GetDoneSon();
    }
}
