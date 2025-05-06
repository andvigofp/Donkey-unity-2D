using UnityEngine;
using System.Collections;

public class BarrelFireSpawner : MonoBehaviour
{
    public GameObject fireBallPrefab;
    public GameObject barrilPrefab; // Prefab del barril
    public float minTime = 2f;
    public float maxTime = 4f;
    public float fireBallSpeed = 5f;
    public float tiempoEntreBolas = 3f; // Tiempo mínimo entre cada bola de fuego

    private GameObject currentBarril; // Almacenar el barril activo
    private bool puedeGenerar = true; // Controlar si se puede generar una nueva bola

    private void Start()
    {
        StartCoroutine(SpawnBarriles());
    }

    private IEnumerator SpawnBarriles()
    {
        while (true) // Bucle infinito para generar barriles
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime)); // Esperar antes de generar barril

            if (currentBarril == null) // Solo generar un barril si no hay otro activo
            {
                SpawnBarril();
            }
        }
    }

    private void SpawnBarril()
    {
        Debug.Log("🔥 Generando barril con bola de fuego...");
        currentBarril = Instantiate(barrilPrefab, transform.position, Quaternion.identity);

        if (puedeGenerar)
        {
            StartCoroutine(SpawnFireBallFromBarril());
        }
    }

    private IEnumerator SpawnFireBallFromBarril()
    {
        puedeGenerar = false; // Bloquear generación de más bolas de fuego

        yield return new WaitForSeconds(tiempoEntreBolas); // Esperar antes de lanzar bola de fuego

        Debug.Log("🔥 Liberando bola de fuego desde barril!");
        GameObject fireBall = Instantiate(fireBallPrefab, transform.position, Quaternion.identity);
        fireBall.SetActive(true);

        Rigidbody2D rb = fireBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.right * fireBallSpeed;
        }

        puedeGenerar = true; // Permitir generar otra bola después del tiempo establecido

        // Cuando el barril ya lanzó la bola, eliminarlo
        Destroy(currentBarril, 1f); // Destruir barril después de 1 segundo
        currentBarril = null; // Reiniciar la referencia del barril
    }
}

