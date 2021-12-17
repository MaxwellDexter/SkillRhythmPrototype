using UnityEngine;

public class InteractableThing : MonoBehaviour
{
    public AudioClip destroyClip;
    public bool addScoreOnDestroy;

    protected GameObject player;

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    private void Update()
    {
        if (transform.position.z < player.transform.position.z - 10)
            DestroyMe();
    }

    public virtual void DestroyMe()
    {
        Destroy(gameObject);
    }

    public virtual void BlowUp()
    {
        if (addScoreOnDestroy)
            GameObject.Find("Score Manager").GetComponent<ScoreManager>().Add();
        SFXTempPlayer.Create(destroyClip, 0.9f);
        Destroy(gameObject);
    }
}
