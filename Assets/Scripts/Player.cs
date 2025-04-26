using System.Collections; // Necesario para IEnumerator
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private Sprite climbSprite;
    private int spriteIndex;

    private Rigidbody2D playerRigidbody; // Renombrado para evitar conflictos.
    private Collider2D playerCollider;  // Renombrado para evitar conflictos.
    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
    public float jumpStrength = 1f;

    private bool grounded;
    private bool climbing;

    private bool shouldAnimate; // Controla la ejecución del Coroutine.

    private GameManager gameManager; // Referencia al GameManager.

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

        if (spriteRenderer == null || playerRigidbody == null || playerCollider == null)
        {
            Debug.LogError("Faltan componentes esenciales en el Player");
            enabled = false;
            return;
        }

        results = new Collider2D[10]; // Tamaño ajustado para evitar pérdida de colisiones.
    }

    private void Start()
    {
        // Asigna dinámicamente el GameManager al iniciar el juego.
        gameManager = Object.FindAnyObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogWarning("GameManager no encontrado en la escena actual.");
        }
    }

    private void OnEnable()
    {
        shouldAnimate = true; // Activa la animación.
        StopAllCoroutines(); // Detiene cualquier Coroutine activo.
        StartCoroutine(AnimateCoroutine()); // Inicia la animación.
    }

    private void OnDisable()
    {
        shouldAnimate = false; // Desactiva la animación.
        StopAllCoroutines(); // Detiene todas las Coroutines.
    }

    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        Vector2 size = playerCollider.bounds.size;
        size.y += 0.1f; // Margen vertical para detección precisa.
        size.x /= 2f; // Reduce tamaño horizontal.

        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter(); // Configuración sin filtros específicos.

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
            direction.y = Mathf.Max(direction.y, -1f); // Limita velocidad descendente.
        }

        transform.eulerAngles = direction.x > 0f ? Vector3.zero : new Vector3(0f, 180f, 0f);
    }

    private void FixedUpdate()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + direction * Time.fixedDeltaTime);
    }

    private IEnumerator AnimateCoroutine()
    {
        while (shouldAnimate) // Controla el bucle de animación.
        {
            AnimateSprite();
            yield return new WaitForSeconds(1f / 12f); // Ajusta velocidad de animación.
        }
    }

    private void AnimateSprite()
    {
        if (climbing)
        {
            spriteRenderer.sprite = climbSprite;
        }
        else if (direction.x != 0f)
        {
            spriteIndex = (spriteIndex + 1) % runSprites.Length; // Ciclo de sprites.
            spriteRenderer.sprite = runSprites[spriteIndex];
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameManager == null)
        {
            Debug.LogWarning("GameManager no asignado. No se puede ejecutar la lógica de colisión.");
            return; // Evita continuar si no hay GameManager asignado.
        }

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

    private void OnDrawGizmos()
    {
        if (playerCollider != null)
        {
            Vector2 size = playerCollider.bounds.size;
            size.y += 0.1f; // Margen para depuración.
            size.x /= 2f;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, size); // Visualiza el área de colisión.
        }
    }
}

