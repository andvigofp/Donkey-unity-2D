using UnityEngine;

public class DonkeyKong : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>(); // Obtener referencia al Animator
    }

    private void Start()
    {
        anim.SetBool("movement", true); // Activar la animación al inicio

        // Opcional: desactivar "movement" después de unos segundos para que no se repita
        Invoke("DetenerAnimacion", 2f); // Se detendrá en 2 segundos
    }

    private void DetenerAnimacion()
    {
        anim.SetBool("movement", false); // Detener la animación
    }
}
