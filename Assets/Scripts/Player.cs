using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Vida")]
    public int vidaMaxima = 3;
    private int vidaActual;
    public static event Action<int, int> OnVidasCambiadas;

    [Header("Componentes")]
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;
    private EfectosSonido efectosSonido;

    [Header("Animaciones")]
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private Sprite climbSprite;
    [SerializeField] private Sprite[] hammerSprites;

    [Header("Movimiento")]
    public float moveSpeed = 1f;
    public float jumpStrength = 5f;
    private Vector2 direction;

    [Header("Martillo")]
    private bool tieneMartillo;

    [Header("Estado")]
    private bool grounded;
    private bool climbing;
    private bool estabaEnEscalera;

    [Header("Respawn")]
    private Vector3 puntoDeInicio;

    [Header("Spawner")]
    [SerializeField] private Spawner spawner; // Asigna en el Inspector

    private Collider2D[] results;

    [Header("Puntos")]
    private int puntos = 0; // Variable para almacenar los puntos
    private int puntosMaximos = 0; // Variable para almacenar los puntos máximos
    public static event System.Action<int> OnPuntosCambiados;

    [Header("Spawner de fuego")]
    [SerializeField] private BarrelFireSpawner spawnerFire; // Asegúrate de asignarlo en el Inspector

    [SerializeField]public TextMeshProUGUI numeroPuntosTexto ; // Referencia al texto en el Canvas

    private bool invulnerable = false;
    public float tiempoInvulnerabilidad = 1.0f; // 1 segundo de invulnerabilidad

    private void Awake()
    {
        results = new Collider2D[10];
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        efectosSonido = GetComponent<EfectosSonido>();

        if (spawner == null && Spawner.Instance != null)
        {
            spawner = Spawner.Instance; // Referencia segura como fallback
        }

        vidaActual = vidaMaxima;
        puntoDeInicio = transform.position;

        // Recuperar los puntos guardados
        puntos = PlayerPrefs.GetInt("PuntosGuardados", 0);
    }

    private void Start()
    {
        OnVidasCambiadas?.Invoke(vidaActual, vidaMaxima);
        OnPuntosCambiados?.Invoke(puntos);
        numeroPuntosTexto.text = "00";
        ActualizarTextoPuntos(); // Mostrar puntos al inicio
        puntos = 0; // Reiniciar puntos a 0 al inicio
        puntosMaximos = PlayerPrefs.GetInt("PuntosMaximos", 0); // Cargar puntos máximos guardados
    }

    private void Update()
    {
        CheckCollision();

        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
            if (!estabaEnEscalera && Mathf.Abs(direction.y) > 0.1f)
            {
                efectosSonido.ReproducirEscalera();
            }
        }
        else if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
            efectosSonido.ReproducirSalto();
        }
        else
        {
            direction += Physics2D.gravity * Time.deltaTime;
        }

        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

        if (grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f);
        }

        if (direction.x > 0f)
        {
            transform.localScale = tieneMartillo ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        }
        else if (direction.x < 0f)
        {
            transform.localScale = tieneMartillo ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        }

        //anim.SetBool("climbing", climbing);
        //anim.SetBool("jump", !grounded && !climbing);
        //anim.SetBool("run", grounded && Mathf.Abs(direction.x) > 0.1f);

        // 🔹 Si tiene el martillo, activar la animación sin importar el estado
        if (tieneMartillo)
        {
            anim.SetBool("hammer", true);
            anim.SetBool("run", false);
            anim.SetBool("jump", false);
            anim.SetBool("climbing", false);
        }
        else
        {
            anim.SetBool("hammer", false);
            anim.SetBool("climbing", climbing);
            anim.SetBool("jump", !grounded && !climbing);
            anim.SetBool("run", grounded && Mathf.Abs(direction.x) > 0.1f);
        }

        estabaEnEscalera = climbing;
    }

    public void SumarPuntos(int cantidad)
    {
        puntos += cantidad;
        OnPuntosCambiados?.Invoke(puntos);
        Debug.Log($"Puntos actuales: {puntos}"); // Verificar si los puntos se están sumando
        ActualizarTextoPuntos(); // Actualizar el nuevo texto en el nivel
        // Ya no actualizamos el récord ni el GameOver aquí
    }

    private void ActualizarTextoPuntos()
    {
        if (numeroPuntosTexto != null)
        {
            numeroPuntosTexto.text = puntos.ToString(); // Solo actualizar el número
        }
        else
        {
            Debug.LogError("NumeroPuntosTexto no está asignado en el Inspector.");
        }
    }

    private void FixedUpdate()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + direction * Time.fixedDeltaTime);
    }

    public void PasarDeNivel()
    {
        PlayerPrefs.SetInt("PuntosGuardados", puntos);
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        Vector2 size = playerCollider.bounds.size;
        size.y += 0.1f;
        size.x /= 2f;

        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();

        int amount = Physics2D.OverlapBox(transform.position, size, 0f, filter, results);

        for (int i = 0; i < amount; i++)
        {
            GameObject hit = results[i].gameObject;

            switch (hit.layer)
            {
                case int layer when layer == LayerMask.NameToLayer("Ground"):
                    grounded = hit.transform.position.y < (transform.position.y - 0.5f);
                    Physics2D.IgnoreCollision(playerCollider, results[i], !grounded);
                    break;
                case int layer when layer == LayerMask.NameToLayer("Ladder"):
                    climbing = true;
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Martillo"))
        {
            anim.SetBool("hammer", true);
            Destroy(collision.gameObject);
            tieneMartillo = true;
            efectosSonido.ReproducirMartillo();
            StartCoroutine(TemporizadorMartillo(5f));
        }
    }

    private IEnumerator TemporizadorMartillo(float duracion)
    {
        tieneMartillo = true;
        anim.SetBool("hammer", true);

        float tiempoRestante = duracion;
        bool parpadeando = false;

        while (tiempoRestante > 0)
        {
            if (tiempoRestante <= 2f) // 🔹 Cuando queden 2 segundos, empieza a parpadear
            {
                parpadeando = !parpadeando;
                spriteRenderer.color = parpadeando ? new Color(1f, 1f, 1f, 0.5f) : new Color(1f, 1f, 1f, 1f);
            }

            yield return new WaitForSeconds(0.2f); // Cambia cada 0.2 segundos
            tiempoRestante -= 0.2f;
        }

        // Restaurar estado normal
        tieneMartillo = false;
        anim.SetBool("hammer", false);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f); // Restaurar color normal
    }

    public void RecibirDanio(int cantidadDanio)
    {
        if (invulnerable || vidaActual <= 0)
            return;

        vidaActual -= cantidadDanio;
        OnVidasCambiadas?.Invoke(vidaActual, vidaMaxima);

        // Feedback visual y sonoro
        StartCoroutine(InvulnerabilidadTemporal());
        if (efectosSonido != null) efectosSonido.ReproducirDaño(); // Si tienes un sonido de daño

        if (vidaActual <= 0)
        {
            if (!anim.GetBool("dead")) // Evitar múltiples ejecuciones
            {
                anim.SetBool("dead", true);
                anim.SetBool("run", false);
                anim.SetBool("jump", false);
                anim.SetBool("climbing", false);
                anim.SetBool("hammer", false);
                moveSpeed = 0f;
                jumpStrength = 0f;
                direction = Vector2.zero;
                playerRigidbody.linearVelocity = Vector2.zero;
                StartCoroutine(EsperarGameOver());
            }
        }
        else
        {
            Respawn(false);
        }
    }

    private IEnumerator InvulnerabilidadTemporal()
    {
        invulnerable = true;
        float tiempo = 0f;
        while (tiempo < tiempoInvulnerabilidad)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            tiempo += 0.1f;
        }
        spriteRenderer.enabled = true;
        invulnerable = false;
    }

    private IEnumerator EsperarGameOver()
    {
        Debug.Log("Esperando que la animación de muerte inicie...");

        // 🔹 Esperar hasta que la animación "Dead" se active en el Animator
        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).IsName("dead"));

        Debug.Log("Animación de muerte iniciada, esperando su duración...");

        yield return new WaitForSeconds(2f); // 🔹 Esperar manualmente 2 segundos después de la animación

        Debug.Log("Animación de muerte terminada, activando Game Over...");

        // Comprobar si se supera el récord
        if (puntos > puntosMaximos)
        {
            puntosMaximos = puntos;
            PlayerPrefs.SetInt("PuntosMaximos", puntosMaximos);
        }
        Time.timeScale = 0f; // Pausar el juego
        GameOver.Instance.MostrarGameOver(puntosMaximos); // Mostrar el menú de Game Over con el récord
    }

    public void Respawn(bool restaurarVidas)
    {
        if (restaurarVidas)
        {
            vidaActual = vidaMaxima; // Restaurar vidas
            Debug.Log("Vidas restauradas al máximo.");

            OnVidasCambiadas?.Invoke(vidaActual, vidaMaxima); // **Actualizar la barra de vida aquí**
        }

        transform.position = puntoDeInicio;
        PlayerPrefs.SetInt("PuntosGuardados", puntos);
        Debug.Log("Jugador respawneado al punto de inicio.");

        // Reiniciar el temporizador de todos los barriles de fuego activos
        foreach (var barrelFire in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (barrelFire.GetType().Name == "BarrelFire")
            {
                var method = barrelFire.GetType().GetMethod("ReiniciarDisparo");
                if (method != null)
                {
                    method.Invoke(barrelFire, null);
                }
            }
        }
        // Destruir todas las bolas de fuego activas
        foreach (var fireBall in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (fireBall.GetType().Name == "FireBall")
            {
                Destroy(fireBall.gameObject);
            }
        }

        if (spawner != null)
        {
            spawner.ResetSpawner();
        }

        // 🔹 Reiniciar el Spawner solo cuando Mario reaparece
        if (spawnerFire != null)
        {
            spawnerFire.ResetSpawner();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tieneMartillo && collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log($"Enemigo {collision.gameObject.name} destruido! Puntos sumados: 100");
            SumarPuntos(100); // Sumar puntos por enemigo eliminado
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            RecibirDanio(1);
        }
    }

    private void OnDrawGizmos()
    {
        if (playerCollider != null)
        {
            Vector2 size = playerCollider.bounds.size;
            size.y += 0.1f;
            size.x /= 2f;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}


