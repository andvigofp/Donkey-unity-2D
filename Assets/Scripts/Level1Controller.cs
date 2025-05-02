using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1Controller : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

