using UnityEngine;
using System.Collections;

public class BarrelFireSpawner : MonoBehaviour
{
    public GameObject barrilPrefab;
    public GameObject fireBallPrefab; // Prefab de la bola de fuego
    public Transform fireSpawnPoint; // 🔹Punto de generación del fuego
    public float minTime = 2f;
    public float maxTime = 4f;
    public float tiempoEntreBolas = 2f; // Nuevo: tiempo entre bolas de fuego

    private void Start()
    {
        StartCoroutine(SpawnBarriles());
        StartCoroutine(SpawnFireBalls()); // Nuevo: corrutina independiente
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
}
