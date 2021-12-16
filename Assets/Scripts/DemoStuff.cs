using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoStuff : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
        else if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(1);
    }
}
