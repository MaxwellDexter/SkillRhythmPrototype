using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextScore : MonoBehaviour
{
    public List<Color> colours;

    public float lifetimeSeconds = 2;
    public int cycleAmount = 1;
    public Vector3 moveSpeed;

    private TextMesh textMesh;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        transform.position = player.transform.position;
        textMesh = GetComponent<TextMesh>();
        StartCoroutine(StartColourChange());
    }
    
    private void Update()
    {
        if (player != null)
            transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);
        transform.position += moveSpeed * Time.deltaTime;
    }

    private IEnumerator StartColourChange()
    {
        float timePerChange = lifetimeSeconds / colours.Count;
        timePerChange /= cycleAmount;
        for (int i = 0; i < cycleAmount; i++)
        {
            foreach(Color colour in colours)
            {
                textMesh.color = colour;
                yield return new WaitForSeconds(timePerChange);
            }
        }

        // end, kill myself
        Destroy(gameObject);
    }
}
