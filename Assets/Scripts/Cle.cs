using UnityEngine;

public class Cle : MonoBehaviour
{
    [Header("Configuration de la clé")]

    [SerializeField] private int numeroCle;
    [SerializeField] private GameObject porteAssociee;

    private void OnTriggerEnter2D(Collider2D autre)
    {
        // Seulement Trevor peut récupérer la clé
        if (!autre.CompareTag("Player"))
            return;

        if (GestionJeu.Instance == null)
        {
            Debug.LogError(
                "GestionJeu n'existe pas dans la scène !"
            );

            return;
        }

        bool cleRecuperee =
            GestionJeu.Instance.RecupererCle(
                numeroCle,
                porteAssociee
            );

        // La clé disparaît seulement si c'est
        // la bonne clé à récupérer
        if (cleRecuperee)
        {
            Destroy(gameObject);
        }
    }
}