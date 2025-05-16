using UnityEngine;

public class Barrel : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignorar colisiones con barriles de fuego al instante
        if (collision.gameObject.layer == LayerMask.NameToLayer("BarrelFireLayer"))
        {
            Collider2D myCollider = GetComponent<Collider2D>();
            Collider2D otherCollider = collision.collider;

            if (myCollider != null && otherCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, otherCollider);
            }

            return; // Salir para evitar cualquier otra lógica
        }

        // Reacción con el suelo
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.AddForce(collision.transform.right * speed, ForceMode2D.Impulse);
        }
    }
}