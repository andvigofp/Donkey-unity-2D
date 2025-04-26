using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private Sprite climbSprite;
    [SerializeField] private Sprite[] hammerSprites; // Animaciones del martillo
    private Animator anim;
    private int spriteIndex;
    private int hammerSpriteIndex;

    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;
    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrength = 5f;

    private bool grounded;
    private bool climbing;
    private bool tieneMartillo;

    private bool shouldAnimate;

    private GameManager gameManager;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        if (spriteRenderer == null || playerRigidbody == null || playerCollider == null || anim == null)
        {
            Debug.LogError("Faltan componentes esenciales en el Player");
            enabled = false;
            return;
        }

        results = new Collider2D[10];
    }

    private void Start()
    {
        gameManager = Object.FindAnyObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogWarning("GameManager no encontrado en la escena actual.");
        }
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

    private void Update()
    {
        // Verificar colisiones (suelo y escalera)
        CheckCollision();

        // Movimiento vertical (escalar y saltar)
        if (climbing)
        {
            direction.y = Input.GetAxis("Vertical") * moveSpeed; // Movimiento en escaleras
        }
        else if (grounded && Input.GetButtonDown("Jump"))
        {
            direction = Vector2.up * jumpStrength; // Saltar
        }
        else
        {
            direction += Physics2D.gravity * Time.deltaTime; // Caída libre
        }

        // Movimiento horizontal
        direction.x = Input.GetAxis("Horizontal") * moveSpeed;

        // Limitar la velocidad de caída si está en el suelo
        if (grounded)
        {
            direction.y = Mathf.Max(direction.y, -1f); // Reducir velocidad descendente
        }

        // Orientación del personaje según la dirección de movimiento
        if (direction.x > 0f && transform.localScale.x != 1f) // Moviéndose a la derecha
        {
            transform.localScale = new Vector3(1, 1, 1); // Mirar a la derecha
        }
        else if (direction.x < 0f && transform.localScale.x != -1f) // Moviéndose a la izquierda
        {
            transform.localScale = new Vector3(-1, 1, 1); // Mirar a la izquierda
        }

        // Actualización de animaciones
        anim.SetBool("climbing", climbing); // Activar animación de escalada
        anim.SetBool("jump", !grounded); // Activar animación de salto
        anim.SetBool("run", grounded && Mathf.Abs(direction.x) > 0.1f); // Activar animación de correr
        anim.SetBool("hammer", tieneMartillo); // Activar estado del martillo
    }


    private void FixedUpdate()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + direction * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Martillo"))
        {
            anim.SetBool("hammer", true); // Activar martillo

            Destroy(collision.gameObject); // Eliminar el objeto del martillo
            tieneMartillo = true;

            StartCoroutine(TemporizadorMartillo(5f)); // Controlar duración del martillo
        }
    }

    private IEnumerator TemporizadorMartillo(float duracion)
    {
        yield return new WaitForSeconds(duracion);
        tieneMartillo = false;
        anim.SetBool("hammer", false); // Desactivar martillo
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tieneMartillo && collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject); // Destruir el barril
            Debug.Log("Barril destruido!"); // Confirmación en consola
        }
        else if (gameManager != null)
        {
            switch (collision.gameObject.tag)
            {
                case "Objective":
                    enabled = false;
                    gameManager.LevelComplete();
                    break;

                case "Obstacle":
                    enabled = false;
                    gameManager.LevelFailed();
                    break;
            }
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

