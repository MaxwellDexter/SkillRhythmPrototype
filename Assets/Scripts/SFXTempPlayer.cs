using System.Collections;
using UnityEngine;

public class SFXTempPlayer : MonoBehaviour
{
    private AudioSource source;

    public static void Create(AudioClip clip, float volume = 1.0f)
    {
        GameObject obj = new GameObject("SFX Player");
        AudioSource source = obj.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        obj.AddComponent<SFXTempPlayer>();
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.Play();
        StartCoroutine(KillMe());
    }

    private IEnumerator KillMe()
    {
        yield return new WaitForSeconds(source.clip.length);
        Destroy(gameObject);
    }
}
