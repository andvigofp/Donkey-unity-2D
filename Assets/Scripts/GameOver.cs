using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance;
    public GameObject gameOverPanel;
    public TMP_Text textPuntos;

    private void Awake()
    {
        Instance = this;
        gameOverPanel.SetActive(false); // Ocultar el menú al inicio
    }

    public void MostrarGameOver(int puntos)
    {
        gameOverPanel.SetActive(true); // Mostrar el menú de Game Over
        GuardarPuntuacion(puntos);
    }

    private void GuardarPuntuacion(int puntos)
    {
        textPuntos.text = "Puntos: " + puntos.ToString(); // Mostrar puntos en pantalla
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
