using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Configuración del Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evitar duplicados
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
    }

    private void Start()
    {
        // Si la escena actual es "Preload", inicia la transición automática a Level1
        if (SceneManager.GetActiveScene().name == "Preload")
        {
            StartCoroutine(EsperarYCargarNivel(1)); // Cargar Level1 después de unos segundos
        }
    }

    private IEnumerator EsperarYCargarNivel(int index)
    {
        yield return new WaitForSeconds(2f); // Esperar 2 segundos antes de cambiar
        LoadLevel(index); // Cargar Level1
    }

    public void LoadLevel(int index)
    {
        if (index >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError("El nivel solicitado está fuera de los límites. Reiniciando al nivel 1.");
            index = 0; // Reinicia al primer nivel si el índice está fuera de los límites
        }

        SceneManager.LoadScene(index);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(currentIndex + 1); // Carga el siguiente nivel
    }
}
