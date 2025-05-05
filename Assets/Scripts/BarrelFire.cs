using UnityEngine;

public class BarrelFire : MonoBehaviour
{
    private GameObject fireBall; // Referencia a la bola de fuego dentro del barril

    public void AsignarBolaDeFuego(GameObject bola)
    {
        fireBall = bola; // Guardamos la referencia de la bola de fuego
    }

    public void Explode()
    {
        Debug.Log("¡Barril explotando!");

        // Liberar la bola de fuego antes de destruir el barril
        if (fireBall != null)
        {
            fireBall.transform.parent = null; // Sacar la bola del barril
            fireBall.SetActive(true);
            fireBall.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 5f, ForceMode2D.Impulse);
        }

        Destroy(gameObject); // Destruir el barril
    }
}

