using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VidaJugador : MonoBehaviour
{
    public List<GameObject> corazones; // Lista de GameObjects de corazones

    void Start()
    {
        // Suscribirse al evento de cambios en la vida del Player
        Player.OnVidasCambiadas += ActualizarCorazones;
    }

    private void OnDestroy()
    {
        Player.OnVidasCambiadas -= ActualizarCorazones; // Desuscribirse del evento
    }

    private void ActualizarCorazones(int vidaActual, int vidaMaxima)
    {
        for (int i = 0; i < corazones.Count; i++)
        {
            if (i >= vidaActual) // Si el índice es mayor o igual a la vida actual, destruir el corazón
            {
                Destroy(corazones[i]);
            }
        }

        Debug.Log($"Corazones restantes: {vidaActual}/{vidaMaxima}");
    }
}