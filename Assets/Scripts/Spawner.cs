using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; } // Declaración correcta del Singleton

    public GameObject prefab;
    public float minTime = 2f;
    public float maxTime = 4f;

    private List<GameObject> barriles = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Asignar la instancia
        }
        else
        {
            Destroy(gameObject); // Evitar múltiples instancias si ya existe una
        }
    }

    public void Start()
    {
        StartSpawner();
    }

    private void StartSpawner()
    {
        CancelInvoke(nameof(Spawn));
        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
    }

    private void Spawn()
    {
        GameObject barril = Instantiate(prefab, transform.position, Quaternion.identity);
        barriles.Add(barril);
        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
    }

    public void ResetSpawner()
    {
        CancelInvoke(nameof(Spawn));
        EliminarBarrilesActivos();
        barriles.Clear();
        Invoke(nameof(StartSpawner), 0.5f);
        Debug.Log("Spawner reiniciado correctamente.");
    }

    private void EliminarBarrilesActivos()
    {
        foreach (GameObject barril in barriles)
        {
            if (barril != null)
            {
                Destroy(barril);
            }
        }
        Debug.Log("Barriles eliminados.");
    }
}
