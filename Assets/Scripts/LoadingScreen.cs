using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textoProgreso;
    
    [Header("Configuración")]
    public float velocidadCarga = 1f;
    public string siguienteEscena = "Level1";
    
    private void Start()
    {
        StartCoroutine(CargarEscena());
    }

    private IEnumerator CargarEscena()
    {
        // Iniciamos la carga asíncrona de la siguiente escena
        AsyncOperation operacionCarga = SceneManager.LoadSceneAsync(siguienteEscena);
        operacionCarga.allowSceneActivation = false;
        
        float progreso = 0f;
        
        while (!operacionCarga.isDone)
        {
            // Calculamos el progreso
            progreso = Mathf.MoveTowards(progreso, operacionCarga.progress * 100f, velocidadCarga * Time.deltaTime * 100f);
            
            // Actualizamos el texto con el porcentaje
            if (textoProgreso != null)
            {
                textoProgreso.text = $"LOADING... {(int)progreso}%";
            }
            
            // Cuando llegamos al 90% (0.9 es el máximo que devuelve LoadSceneAsync)
            if (progreso >= 90f)
            {
                // Esperamos un momento para asegurarnos de que el jugador ve el 100%
                textoProgreso.text = "LOADING... 100%";
                yield return new WaitForSeconds(0.5f);
                operacionCarga.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }
} 