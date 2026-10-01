using UnityEngine;

public class ZoneDangereuse : MonoBehaviour
{
    [SerializeField] private Transform pointDepart;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject autre = collision.gameObject;

        if (!autre.CompareTag("Player"))
            return;

        if (pointDepart == null)
        {
            Debug.LogError(
                "Le point de départ n'est pas assigné."
            );
            return;
        }

        autre.transform.position = pointDepart.position;

        Debug.Log(
            "Trevor a touché l'ennemi et retourne au point de départ."
        );
    }
}