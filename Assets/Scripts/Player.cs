using System;
using UnityEngine;
using System.Collections;

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

    [Header("Respawn")]
    private Vector3 puntoDeInicio;

    [Header("Spawner")]
    [SerializeField] private Spawner spawner; // Asigna en el Inspector

    private Collider2D[] results;

    private void Awake()
    {
        results = new Collider2D[10];
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        if (spawner == null && Spawner.Instance != null)
        {
            spawner = Spawner.Instance; // Referencia segura como fallback
        }

        vidaActual = vidaMaxima;
        puntoDeInicio = transform.position;
    }

    private void Start()
    {
        OnVidasCambiadas?.Invoke(vidaActual, vidaMaxima);
    }

    private void Update()
    {
        CheckCollision();

        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed;
        }
        else if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength;
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

        anim.SetBool("climbing", climbing);
        anim.SetBool("jump", !grounded && !climbing);
        anim.SetBool("run", grounded && Mathf.Abs(direction.x) > 0.1f);
        anim.SetBool("hammer", tieneMartillo);
    }

    private void FixedUpdate()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + direction * Time.fixedDeltaTime);
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
            StartCoroutine(TemporizadorMartillo(5f));
        }
    }

    private IEnumerator TemporizadorMartillo(float duracion)
    {
        yield return new WaitForSeconds(duracion);
        tieneMartillo = false;
        anim.SetBool("hammer", false);
    }

    public void RecibirDanio(int cantidadDanio)
    {
        if (vidaActual > 0)
        {
            vidaActual -= cantidadDanio;
            OnVidasCambiadas?.Invoke(vidaActual, vidaMaxima);
            Debug.Log($"Jugador recibió daño. Vidas actuales: {vidaActual}");

            if (vidaActual <= 0)
            {
                Respawn(true); // Respawn con vidas restauradas
            }
            else
            {
                Respawn(false); // Respawn sin restaurar vidas
            }
        }
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
        Debug.Log("Jugador respawneado al punto de inicio.");

        if (spawner != null)
        {
            spawner.ResetSpawner();
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tieneMartillo && collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Barril destruido!");
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


