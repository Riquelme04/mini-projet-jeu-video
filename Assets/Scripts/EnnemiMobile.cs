using UnityEngine;


[RequireComponent(
    typeof(Rigidbody2D),
    typeof(Collider2D),
    typeof(SpriteRenderer)
)]
public class EnnemiMobile : MonoBehaviour
{
    

    public enum TypeDeplacement
    {
        Patrouille,
        Sinusoidal
    }

    
    [Header("Patrouille")]

    [SerializeField]
    private TypeDeplacement typeDeplacement =
        TypeDeplacement.Patrouille;

    [SerializeField]
    private Transform pointA;

    [SerializeField]
    private Transform pointB;

    [SerializeField, Min(0.1f)]
    private float vitessePatrouille = 1.8f;

    // Seulement utilisé avec le déplacement Sinusoidal.
    [SerializeField, Min(0.1f)]
    private float hauteurVague = 1.1f;

    [SerializeField, Min(0.1f)]
    private float frequenceVague = 1.4f;


    [Header("Poursuite")]

    // Trevor / Robot
    [SerializeField]
    private Transform joueur;

    // Distance à laquelle le garde voit Trevor.
    [SerializeField, Min(0.5f)]
    private float rayonDetection = 3.5f;

    // Vitesse du garde lorsqu'il poursuit Trevor.
    [SerializeField, Min(0.1f)]
    private float vitessePoursuite = 2.8f;

    
    [Header("Retour du joueur")]

    // Endroit où Trevor retourne lorsqu'il est attrapé.
    [SerializeField]
    private Transform pointDepartJoueur;

    
    private Rigidbody2D corps;
    private SpriteRenderer rendu;

    private Transform ciblePatrouille;

    private float progressionVague;

    private EffetAttaqueEnnemi effetAttaque;

   
    private void Awake()
    {
        corps = GetComponent<Rigidbody2D>();
        rendu = GetComponent<SpriteRenderer>();

        // Le garde ne tombe pas.
        corps.gravityScale = 0f;

        // Le garde ne tourne pas lorsqu'il touche un mur.
        corps.freezeRotation = true;

        // Effet visuel facultatif.
        effetAttaque =
            GetComponent<EffetAttaqueEnnemi>();
    }

    private void Start()
    {
        // Le garde commence par aller vers PointB.
        if (pointB != null)
        {
            ciblePatrouille = pointB;
        }
        else
        {
            ciblePatrouille = pointA;
        }
    }


    private void FixedUpdate()
    {
        // Si la partie est terminée, le garde s'arrête.
        if (GestionJeu.Instance != null &&
            GestionJeu.Instance.PartieTerminee)
        {
            corps.linearVelocity = Vector2.zero;
            return;
        }

        // Vérifie si Trevor est suffisamment proche.
        bool joueurDetecte =
            joueur != null &&
            Vector2.Distance(
                corps.position,
                joueur.position
            ) <= rayonDetection;

        // Si Trevor est détecté, la poursuite
        // devient prioritaire.
        if (joueurDetecte)
        {
            PoursuivreJoueur();
        }
        else if (
            typeDeplacement ==
            TypeDeplacement.Sinusoidal
        )
        {
            DeplacementSinusoidal();
        }
        else
        {
            DeplacementPatrouille();
        }
    }

 
    private void PoursuivreJoueur()
    {
        if (joueur == null)
            return;

        Vector2 direction =
            ((Vector2)joueur.position -
             corps.position).normalized;

        AppliquerVitesse(
            direction,
            vitessePoursuite
        );
    }


    private void DeplacementPatrouille()
    {
        if (ciblePatrouille == null)
        {
            corps.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            ((Vector2)ciblePatrouille.position -
             corps.position).normalized;

        AppliquerVitesse(
            direction,
            vitessePatrouille
        );

        // Si le garde arrive près de son point,
        // il change de destination.
        if (Vector2.Distance(
                corps.position,
                ciblePatrouille.position
            ) < 0.2f)
        {
            if (ciblePatrouille == pointA)
            {
                ciblePatrouille = pointB;
            }
            else
            {
                ciblePatrouille = pointA;
            }
        }
    }

  
    private void DeplacementSinusoidal()
    {
        if (pointA == null || pointB == null)
        {
            corps.linearVelocity = Vector2.zero;
            return;
        }

        progressionVague +=
            Time.fixedDeltaTime * frequenceVague;

        float allerRetour =
            Mathf.PingPong(
                progressionVague,
                1f
            );

        Vector2 baseTrajet =
            Vector2.Lerp(
                pointA.position,
                pointB.position,
                allerRetour
            );

        Vector2 cibleVague =
            baseTrajet +
            Vector2.up *
            (
                Mathf.Sin(
                    progressionVague *
                    Mathf.PI *
                    2f
                ) *
                hauteurVague
            );

        Vector2 direction =
            (cibleVague -
             corps.position).normalized;

        AppliquerVitesse(
            direction,
            vitessePatrouille * 1.15f
        );
    }


    private void AppliquerVitesse(
        Vector2 direction,
        float vitesse
    )
    {
        corps.linearVelocity =
            direction * vitesse;

        // Retourne visuellement le garde
        // selon la direction horizontale.
        if (Mathf.Abs(direction.x) > 0.05f)
        {
            rendu.flipX =
                direction.x < 0f;
        }
    }

    
    private void OnTriggerEnter2D(
        Collider2D autre
    )
    {
        // Ignore tout ce qui n'est pas Trevor.
        if (!autre.CompareTag("Player"))
            return;

        if (pointDepartJoueur == null)
        {
            Debug.LogError(
                "Le PointDepart de Trevor " +
                "n'est pas configuré sur l'ennemi."
            );

            return;
        }

        // Effet visuel du garde.
        if (effetAttaque != null)
        {
            effetAttaque.Declencher();
        }

        // Trevor retourne au point de départ.
        autre.transform.position =
            pointDepartJoueur.position;

        // Enlève son mouvement pour éviter
        // qu'il continue à glisser après
        // la téléportation.
        Rigidbody2D corpsJoueur =
            autre.GetComponent<Rigidbody2D>();

        if (corpsJoueur != null)
        {
            corpsJoueur.linearVelocity =
                Vector2.zero;
        }

        Debug.Log(
            "Trevor a été attrapé ! " +
            "Retour au point de départ."
        );
    }

 
    private void OnDrawGizmosSelected()
    {
        // Cercle représentant la zone
        // de détection du garde.
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            rayonDetection
        );

        // Ligne représentant son trajet.
        if (pointA != null &&
            pointB != null)
        {
            Gizmos.DrawLine(
                pointA.position,
                pointB.position
            );
        }
    }
}