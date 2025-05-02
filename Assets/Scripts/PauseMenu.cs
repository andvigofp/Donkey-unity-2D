using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Asignar el panel en el Inspector
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true); // Mostrar el menú de pausa
        Time.timeScale = 0f; // Pausar el juego
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false); // Ocultar el menú de pausa
        Time.timeScale = 1f; // Reanudar el juego
        isPaused = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Asegurar que el tiempo vuelve a la normalidad
        SceneManager.LoadScene("MainMenu"); // Cargar el menú principal
    }
}

