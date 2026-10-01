
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class MouvementPlayer : MonoBehaviour
{
    [Header("Paramètres du déplacement")]
    [SerializeField] private float vitesse = 5f;

    private Rigidbody2D corps;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 direction;

    private bool commandesActives = true;

    private void Awake()
    {
        corps = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Configuration du Rigidbody 2D
        corps.gravityScale = 0f;
        corps.freezeRotation = true;

        corps.interpolation =
            RigidbodyInterpolation2D.Interpolate;

        corps.collisionDetectionMode =
            CollisionDetectionMode2D.Continuous;
    }

    private void Update()
    {
        if (!commandesActives)
        {
            direction = Vector2.zero;

            animator.SetBool("EnMouvement", false);

            return;
        }

        // Déplacement horizontal et vertical
        direction = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

        // Retourner le personnage sans modifier sa taille
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        // Animation du déplacement
        animator.SetBool(
            "EnMouvement",
            direction.sqrMagnitude > 0.01f
        );
    }

    private void FixedUpdate()
    {
        if (!commandesActives)
        {
            corps.linearVelocity = Vector2.zero;
            return;
        }

        // Déplacement du personnage
        corps.linearVelocity = direction * vitesse;
    }

    public void DesactiverCommandes()
    {
        commandesActives = false;

        direction = Vector2.zero;

        corps.linearVelocity = Vector2.zero;

        animator.SetBool("EnMouvement", false);
    }
}
