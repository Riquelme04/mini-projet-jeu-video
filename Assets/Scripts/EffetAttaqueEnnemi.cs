using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EffetAttaqueEnnemi : MonoBehaviour
{
    [SerializeField]
    private Color couleurAttaque =
        new Color(1f, 0.45f, 0.12f, 1f);

    [SerializeField, Min(1f)]
    private float agrandissement = 1.22f;

    [SerializeField, Min(0.05f)]
    private float duree = 0.18f;

    private SpriteRenderer rendu;
    private Color couleurInitiale;
    private Vector3 echelleInitiale;
    private Coroutine effetEnCours;

    private void Awake()
    {
        rendu = GetComponent<SpriteRenderer>();
        couleurInitiale = rendu.color;
        echelleInitiale = transform.localScale;
    }

    public void Declencher()
    {
        if (effetEnCours != null)
        {
            StopCoroutine(effetEnCours);
        }

        effetEnCours = StartCoroutine(JouerEffet());
    }

    private IEnumerator JouerEffet()
    {
        rendu.color = couleurAttaque;
        transform.localScale =
            echelleInitiale * agrandissement;

        yield return new WaitForSeconds(
            duree * 0.45f
        );

        rendu.color = Color.white;
        transform.localScale =
            echelleInitiale * 0.92f;

        yield return new WaitForSeconds(
            duree * 0.25f
        );

        rendu.color = couleurInitiale;
        transform.localScale = echelleInitiale;

        effetEnCours = null;
    }

    private void OnDisable()
    {
        if (rendu != null)
        {
            rendu.color = couleurInitiale;
        }

        transform.localScale = echelleInitiale;
    }
}