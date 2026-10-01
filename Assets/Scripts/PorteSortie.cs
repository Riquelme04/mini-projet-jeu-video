using UnityEngine;

public class PorteSortie : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D autre)
    {
        if (!autre.CompareTag("Player"))
            return;

        if (GestionJeu.Instance == null)
        {
            Debug.LogError(
                "GestionJeu n'existe pas dans la scène !"
            );

            return;
        }

        GestionJeu.Instance.TenterSortie(autre.gameObject);
    }
}