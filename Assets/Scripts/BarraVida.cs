using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image rellenoBarraVida;

    void Start()
    {
        // Suscribirse al evento de cambios en la vida del Player
        Player.OnVidasCambiadas += ActualizarBarra;
    }

    private void OnDestroy()
    {
        Player.OnVidasCambiadas -= ActualizarBarra; // Desuscribirse del evento
    }

    private void ActualizarBarra(int vidaActual, int vidaMaxima)
    {
        rellenoBarraVida.fillAmount = (float)vidaActual / vidaMaxima; // Actualizar la barra
        Debug.Log($"Barra de vida actualizada: {rellenoBarraVida.fillAmount}");
    }
}

