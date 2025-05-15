using UnityEngine;
using System.Collections;

public class BarrelFireSpawner : MonoBehaviour
{
    public GameObject barrilPrefab;
    public GameObject fireBallPrefab; // 🔹 Prefab de la bola de fuego
    public Transform fireSpawnPoint; // 🔹 Punto de generación del fuego
    public float minTime = 2f;
    public float maxTime = 4f;
    public float tiempoEntreBolas = 2f; // 🔹 Tiempo entre bolas de fuego

    private Coroutine fireBallCoroutine;
    private Coroutine barrilCoroutine;

    private void Start()
    {
        barrilCoroutine = StartCoroutine(SpawnBarriles());
        fireBallCoroutine = StartCoroutine(SpawnFireBalls());
    }

    private IEnumerator SpawnBarriles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            Instantiate(barrilPrefab, transform.position, Quaternion.identity);
        }
    }

    private IEnumerator SpawnFireBalls()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreBolas);
            if (fireSpawnPoint != null)
            {
                Instantiate(fireBallPrefab, fireSpawnPoint.position, Quaternion.identity);
            }
        }
    }

    public void ResetSpawner()
    {
        // Eliminar todas las bolas de fuego activas
        foreach (var fireBall in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (fireBall.GetType().Name == "FireBall")
            {
                Destroy(fireBall.gameObject);
            }
        }

        // Detener las corutinas actuales
        if (fireBallCoroutine != null)
        {
            StopCoroutine(fireBallCoroutine);
            fireBallCoroutine = null;
        }
        if (barrilCoroutine != null)
        {
            StopCoroutine(barrilCoroutine);
            barrilCoroutine = null;
        }

        // Iniciar nuevas corutinas
        barrilCoroutine = StartCoroutine(SpawnBarriles());
        fireBallCoroutine = StartCoroutine(RestaurarFireBalls());
    }

    private IEnumerator RestaurarFireBalls()
    {
        // Esperar el tiempo completo antes de reiniciar
        yield return new WaitForSeconds(tiempoEntreBolas);
        fireBallCoroutine = StartCoroutine(SpawnFireBalls());
    }
}

