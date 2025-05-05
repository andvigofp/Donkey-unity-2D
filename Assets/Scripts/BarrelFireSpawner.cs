using UnityEngine;

public class BarrelFireSpawner : MonoBehaviour
{
    public GameObject fireBallPrefab;
    public GameObject barrilPrefab; // Prefab del barril
    public float minTime = 2f;
    public float maxTime = 4f;
    public float fireBallSpeed = 5f;

    private void Start()
    {
        Invoke(nameof(SpawnBarril), Random.Range(minTime, maxTime));
    }

    private void SpawnBarril()
    {
        Debug.Log("Generando barril con bola de fuego...");

        GameObject barril = Instantiate(barrilPrefab, transform.position, Quaternion.identity);

        Invoke(nameof(SpawnFireBallFromBarril), Random.Range(2f, 5f)); // Esperar un tiempo antes de lanzar bola de fuego
    }

    private void SpawnFireBallFromBarril()
    {
        Debug.Log("Liberando bola de fuego desde barril!");

        GameObject fireBall = Instantiate(fireBallPrefab, transform.position, Quaternion.identity);
        fireBall.SetActive(true);

        Rigidbody2D rb = fireBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.right * fireBallSpeed;
        }

        Invoke(nameof(SpawnBarril), Random.Range(minTime, maxTime)); // Volver a generar otro barril después de un tiempo
    }
}
