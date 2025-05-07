using UnityEngine;
using UnityEngine.SceneManagement;

public class Continuara : MonoBehaviour
{
    void Start()
    {
        // Cargar el MainMenu después de 2 segundos (ajusta el tiempo si es necesario)
        Invoke(nameof(LoadMainMenu), 5f);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Cambiar a la escena del menú principal
    }
}