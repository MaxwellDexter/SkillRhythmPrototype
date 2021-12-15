using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : InteractableThing
{
    public AudioClip destroyClip;

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0.5f, 0));
        if (transform.position.z < player.transform.position.z - 10)
            Destroy(gameObject);
    }

    public override void GetDoneSon()
    {
        GameObject.Find("Score Manager").GetComponent<ScoreManager>().AddBonus();
        SFXTempPlayer.Create(destroyClip, 0.9f);
        base.GetDoneSon();
    }
}
