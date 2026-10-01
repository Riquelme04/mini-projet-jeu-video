using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnnemiPoursuite : MonoBehaviour
{
    [Header("Poursuite")]
    [SerializeField] private float distanceDetection = 5f;
    [SerializeField] private float vitesse = 3f;

    private Transform joueur;
    private Rigidbody2D rb;
    private bool joueurDetecte = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameObject objetJoueur =
            GameObject.FindGameObjectWithTag("Player");

        if (objetJoueur != null)
        {
            joueur = objetJoueur.transform;
        }
        else
        {
            Debug.LogError(
                "Aucun joueur avec le tag Player n'a été trouvé."
            );
        }
    }

    private void Update()
    {
        if (joueur == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            joueur.position
        );

        // Dès que Trevor est assez proche,
        // l'ennemi le détecte définitivement.
        if (!joueurDetecte && distance <= distanceDetection)
        {
            joueurDetecte = true;

            Debug.Log("L'ennemi a détecté Trevor !");
        }
    }

    private void FixedUpdate()
    {
        if (joueur == null || !joueurDetecte)
            return;

        // Direction vers Trevor
        Vector2 direction =
            ((Vector2)joueur.position - rb.position).normalized;

        // Déplacement avec le Rigidbody2D
        // pour respecter les collisions.
        rb.linearVelocity = direction * vitesse;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            distanceDetection
        );
    }
}