using UnityEngine;

public class EfectosSonido : MonoBehaviour
{
    private AudioSource audioSource;
    
    [Header("Clips de Audio")]
    [SerializeField] private AudioClip sonidoSalto;
    [SerializeField] private AudioClip sonidoMartillo;
    [SerializeField] private AudioClip sonidoEscalera;
    [SerializeField] private AudioClip sonidoDaño;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No se encontró el componente AudioSource");
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        Debug.Log("EfectosSonido iniciado con AudioSource: " + (audioSource != null));
    }

    public void ReproducirSalto()
    {
        if (sonidoSalto != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoSalto);
            Debug.Log("Reproduciendo sonido de salto");
        }
        else
        {
            Debug.LogWarning("Falta configurar el sonido de salto o el AudioSource");
        }
    }

    public void ReproducirMartillo()
    {
        if (sonidoMartillo != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoMartillo);
            Debug.Log("Reproduciendo sonido de martillo");
        }
        else
        {
            Debug.LogWarning("Falta configurar el sonido de martillo o el AudioSource");
        }
    }

    public void ReproducirEscalera()
    {
        if (sonidoEscalera != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoEscalera);
            Debug.Log("Reproduciendo sonido de escalera");
        }
        else
        {
            Debug.LogWarning("Falta configurar el sonido de escalera o el AudioSource");
        }
    }

    public void ReproducirDaño()
    {
        if (sonidoDaño != null && audioSource != null)
        {
            audioSource.PlayOneShot(sonidoDaño);
            Debug.Log("Reproduciendo sonido de daño");
        }
        else
        {
            Debug.LogWarning("Falta configurar el sonido de daño o el AudioSource");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
