using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int level;
    private int lives;
    private int score;

    private void Start()
    {
        DontDestroyOnLoad(gameObject); // Persiste a través de escenas
        NewGame(); // Inicia un nuevo juego
    }

    private void NewGame()
    {
        lives = 3; // Reinicia las vidas
        score = 0; // Reinicia el puntaje

        LoadLevel(1); // Comienza desde el nivel 1
    }

    private void LoadLevel(int index)
    {
        level = index;

        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.cullingMask = -1; // Asegura que todas las capas sean renderizadas
        }
        else
        {
            Debug.LogWarning("No se encontró ninguna cámara principal en la escena."); // Manejo del caso
        }

        Invoke(nameof(LoadScene), 1f); // Carga la escena con retraso
    }

    private void LoadScene()
    {
        if (level >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("El nivel solicitado está fuera de los límites. Reiniciando al nivel 1.");
            level = 1; // Reinicia al nivel 1 si el índice está fuera de los límites
        }

        SceneManager.LoadScene(level); // Carga la escena actual
    }

    public void LevelComplete()
    {
        score += 1000; // Incrementa el puntaje al completar un nivel
        Debug.Log($"Nivel completado. Puntaje actual: {score}");

        int nextLevel = level + 1;

        if (nextLevel < SceneManager.sceneCountInBuildSettings)
        {
            LoadLevel(nextLevel); // Carga el siguiente nivel
        }
        else
        {
            Debug.Log("¡Has completado todos los niveles! Reiniciando...");
            LoadLevel(1); // Reinicia al nivel 1 si no hay más niveles
        }
    }

    public void LevelFailed()
    {
        lives--; // Reduce las vidas
        Debug.Log($"Nivel fallido. Vidas restantes: {lives}");

        if (lives <= 0)
        {
            Debug.Log("Se han acabado las vidas. Iniciando un nuevo juego...");
            NewGame(); // Reinicia el juego si se acaban las vidas
        }
        else
        {
            LoadLevel(level); // Reintenta el nivel actual
        }
    }

    // Métodos adicionales para depuración o UI
    public int GetLives() => lives;
    public int GetScore() => score;
    public int GetLevel() => level;
}
