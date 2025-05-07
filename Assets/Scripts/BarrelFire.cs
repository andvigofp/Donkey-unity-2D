using UnityEngine;
using System.Collections;

public class BarrelFire : MonoBehaviour
{
    public GameObject fireBallPrefab;
    public float tiempoAntesDeDisparar = 2f;
    public float fireBallSpeed = 5f;

    private void Start()
    {
        StartCoroutine(DispararBolaDeFuego());
    }

    private IEnumerator DispararBolaDeFuego()
    {
        yield return new WaitForSeconds(tiempoAntesDeDisparar);

        // Crear bola de fuego
        GameObject fireBall = Instantiate(fireBallPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = fireBall.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.right * fireBallSpeed;
        }

        Destroy(fireBall, 5f); // Opcional
        Destroy(gameObject, 1f); // Destruir barril después de lanzar
    }
}