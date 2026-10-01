using UnityEngine;

public class CameraSuivi : MonoBehaviour
{
    [SerializeField] private Transform cible;
    [SerializeField] private float vitesseSuivi = 5f;

    private void LateUpdate()
    {
        if (cible == null) return;

        Vector3 destination = new Vector3(
            cible.position.x,
            cible.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            destination,
            vitesseSuivi * Time.deltaTime
        );
    }
}