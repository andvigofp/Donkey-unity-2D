using UnityEngine;
using System.Collections;

public class BarrelFireSpawner : MonoBehaviour
{
    public GameObject barrilPrefab;
    public float minTime = 2f;
    public float maxTime = 4f;

    private void Start()
    {
        StartCoroutine(SpawnBarriles());
    }

    private IEnumerator SpawnBarriles()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            // Instanciar un nuevo barril
            Instantiate(barrilPrefab, transform.position, Quaternion.identity);
        }
    }
}

