using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public float speed = 2.5f;
    public float amplitude = 1.2f;
    public float frequency = 1.5f;
    public float detectionRange = 5f;

    private Vector2 startPosition;
    private float time;
    private Transform mario;

    private void Start()
    {
        startPosition = transform.position;
        time = 0f;
        GameObject marioObj = GameObject.FindGameObjectWithTag("Player");
        if (marioObj != null)
            mario = marioObj.transform;
        // Asegúrate de que el Rigidbody2D tiene gravityScale = 0
        GetComponent<Rigidbody2D>().gravityScale = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;

        Vector2 objetivo = startPosition;

        if (mario != null && Mathf.Abs(mario.position.y - startPosition.y) < 1f
            && Vector2.Distance(transform.position, mario.position) < detectionRange)
        {
            objetivo = new Vector2(mario.position.x, startPosition.y);
        }

        // Movimiento horizontal hacia el objetivo (Mario o posición inicial)
        float step = speed * Time.deltaTime;
        Vector2 nuevaPos = Vector2.MoveTowards(new Vector2(transform.position.x, startPosition.y), objetivo, step);

        // Movimiento en onda vertical
        float vertical = Mathf.Sin(time * frequency) * amplitude;
        transform.position = new Vector3(nuevaPos.x, startPosition.y + vertical, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca con una pared o plataforma, invierte la dirección
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.collider.CompareTag("InvisibleWall"))
        {
            speed = -speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                // Verifica si el parámetro "hammer" está activo en el Animator de Mario
                Animator anim = player.GetComponent<Animator>();
                if (anim != null && anim.GetBool("hammer"))
                {
                    Destroy(gameObject); // La abeja se destruye si Mario tiene el martillo
                }
                else
                {
                    player.RecibirDanio(1); // Si no, Mario recibe daño
                }
            }
        }
    }
} 