using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioNivel : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != null && collision.gameObject.CompareTag("Objective")) // Asegurar que el objeto existe
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadNextLevel(); // Cargar el siguiente nivel
                Debug.Log("Nivel cambiado: " + SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Debug.LogError("GameManager.Instance es NULL. Asegúrate de que GameManager está en la escena.");
            }
        }
    }
}
