using UnityEngine;

public class Argent : MonoBehaviour
{
    [Header("Valeur")]
    [SerializeField] private int valeur = 1000;

    private bool recupere = false;

    private void OnTriggerEnter2D(Collider2D autre)
    {
        // Vérifie que c'est Trevor
        if (!autre.CompareTag("Player"))
        {
            return;
        }

   
        if (recupere)
        {
            return;
        }

        // Vérifie que GestionJeu existe
        if (GestionJeu.Instance == null)
        {
            Debug.LogError(
                "GestionJeu.Instance est introuvable."
            );

            return;
        }

        // Essaie d'ajouter l'argent
        bool argentAjoute =
            GestionJeu.Instance.AjouterArgent(valeur);

        if (argentAjoute)
        {
            recupere = true;

            // Le billet disparaît
            Destroy(gameObject);
        }
    }
}