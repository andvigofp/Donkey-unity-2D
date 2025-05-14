using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    private void Start()
    {
        rb.linearVelocity = Vector2.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Hacer daño al jugador
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                Animator anim = player.GetComponent<Animator>();
                if (anim != null && anim.GetBool("hammer"))
                {
                    player.SumarPuntos(100); // Sumar puntos si Mario tiene el martillo
                    Destroy(gameObject);
                }
                else
                {
                    player.RecibirDanio(1); // Puedes ajustar la cantidad de daño si quieres
                    Destroy(gameObject);
                }
            }
        }
        else if (other.CompareTag("InvisibleWall"))
        {
            Destroy(gameObject);
        }
    }
}
