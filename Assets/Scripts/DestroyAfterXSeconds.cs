using System.Collections;
using UnityEngine;

public class DestroyAfterXSeconds : MonoBehaviour
{
    public float secondsBeforeDeath;

    private void Start()
    {
        StartCoroutine(DelayDeath());
    }

    private IEnumerator DelayDeath()
    {
        yield return new WaitForSeconds(secondsBeforeDeath);
        Destroy(gameObject);
    }
}
