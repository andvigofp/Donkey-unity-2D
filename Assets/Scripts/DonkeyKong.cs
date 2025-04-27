using UnityEngine;

public class DonkeyKong : MonoBehaviour
{
    private Vector3 posicionInicial;

    private void Awake()
    {
        posicionInicial = transform.position; // Guardar la posición inicial de Donkey Kong
    }

    public void ReiniciarDonkeyKong()
    {
        transform.position = posicionInicial; // Restaurar la posición original
        gameObject.SetActive(true); // Asegurar que está activado en la escena

        Debug.Log("Donkey Kong reiniciado correctamente.");
    }
}
