using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControlsController : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
