<<<<<<< HEAD
using System.Collections; // Necesario para IEnumerator
=======
using System.Collections;
>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] runSprites;
    [SerializeField] private Sprite climbSprite;
<<<<<<< HEAD
    private int spriteIndex;

    private Rigidbody2D playerRigidbody; // Renombrado para evitar conflictos.
    private Collider2D playerCollider;  // Renombrado para evitar conflictos.
=======
    [SerializeField] private Sprite[] hammerSprites; // Animaciones del martillo
    private Animator anim;
    private int spriteIndex;
    private int hammerSpriteIndex;

    private Rigidbody2D playerRigidbody;
    private Collider2D playerCollider;
>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)
    private Collider2D[] results;
    private Vector2 direction;

    public float moveSpeed = 1f;
<<<<<<< HEAD
    public float jumpStrength = 1f;

    private bool grounded;
    private bool climbing;

    private bool shouldAnimate; // Controla la ejecuci�n del Coroutine.

    private GameManager gameManager; // Referencia al GameManager.
=======
    public float jumpStrength = 5f;

    private bool grounded;
    private bool climbing;
    private bool tieneMartillo;

    private bool shouldAnimate;

    private GameManager gameManager;
>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
<<<<<<< HEAD

        if (spriteRenderer == null || playerRigidbody == null || playerCollider == null)
=======
        anim = GetComponent<Animator>();

        if (spriteRenderer == null || playerRigidbody == null || playerCollider == null || anim == null)
>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)
        {
            Debug.LogError("Faltan componentes esenciales en el Player");
            enabled = false;
            return;
        }

<<<<<<< HEAD
        results = new Collider2D[10]; // Tama�o ajustado para evitar p�rdida de colisiones.
=======
        results = new Collider2D[10];
>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)
    }

    private void Start()
    {
<<<<<<< HEAD
        // Asigna din�micamente el GameManager al iniciar el juego.
=======
>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)
        gameManager = Object.FindAnyObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogWarning("GameManager no encontrado en la escena actual.");
        }
    }

<<<<<<< HEAD
    private void OnEnable()
    {
        shouldAnimate = true; // Activa la animaci�n.
        StopAllCoroutines(); // Detiene cualquier Coroutine activo.
        StartCoroutine(AnimateCoroutine()); // Inicia la animaci�n.
    }

    private void OnDisable()
    {
        shouldAnimate = false; // Desactiva la animaci�n.
        StopAllCoroutines(); // Detiene todas las Coroutines.
    }

=======
>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)
    private void CheckCollision()
    {
        grounded = false;
        climbing = false;

        Vector2 size = playerCollider.bounds.size;
<<<<<<< HEAD
        size.y += 0.1f; // Margen vertical para detecci�n precisa.
        size.x /= 2f; // Reduce tama�o horizontal.

        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter(); // Configuraci�n sin filtros espec�ficos.
=======
        size.y += 0.1f;
        size.x /= 2f;

        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)

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
<<<<<<< HEAD
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

=======
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


>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)
    private void FixedUpdate()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + direction * Time.fixedDeltaTime);
    }

<<<<<<< HEAD
    private IEnumerator AnimateCoroutine()
    {
        while (shouldAnimate) // Controla el bucle de animaci�n.
        {
            AnimateSprite();
            yield return new WaitForSeconds(1f / 12f); // Ajusta velocidad de animaci�n.
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
=======
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
>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
<<<<<<< HEAD
        if (gameManager == null)
        {
            Debug.LogWarning("GameManager no asignado. No se puede ejecutar la l�gica de colisi�n.");
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

=======
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


>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)
    private void OnDrawGizmos()
    {
        if (playerCollider != null)
        {
            Vector2 size = playerCollider.bounds.size;
<<<<<<< HEAD
            size.y += 0.1f; // Margen para depuraci�n.
            size.x /= 2f;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, size); // Visualiza el �rea de colisi�n.
=======
            size.y += 0.1f;
            size.x /= 2f;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, size);
>>>>>>> a8d4f0d (Añadido animaciones de correr, saltar y martillo, falta las animaciones, cambio el scrip player para las aniamciones, romper barriles)
        }
    }
}

