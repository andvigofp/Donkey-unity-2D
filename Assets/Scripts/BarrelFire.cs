using UnityEngine;
using System.Collections;

public class BarrelFire : MonoBehaviour
{
    public GameObject fireBallPrefab;
    public float tiempoAntesDeDisparar = 2f;
    public float fireBallSpeed = 5f;

    private Coroutine fireCoroutine;

    private void Start()
    {
        fireCoroutine = StartCoroutine(DispararBolasDeFuegoInfinito());
    }

    private IEnumerator DispararBolasDeFuegoInfinito()
    {
        while (true)
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
        }
    }

    public void ReiniciarDisparo()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        fireCoroutine = StartCoroutine(DispararBolasDeFuegoInfinito());
    }
}