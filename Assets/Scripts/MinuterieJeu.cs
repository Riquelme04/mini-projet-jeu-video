using UnityEngine;
using TMPro;

public class MinuterieJeu : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float dureeDepart = 120f;

    [Header("Interface")]
    [SerializeField] private TMP_Text texteTimer;

    private float tempsRestant;
    private bool minuterieActive = true;

    public float TempsRestant => tempsRestant;

    private void Start()
    {
        tempsRestant = dureeDepart;
        ActualiserAffichage();
    }

    private void Update()
    {
        if (!minuterieActive)
            return;

        tempsRestant -= Time.deltaTime;

        if (tempsRestant <= 0f)
        {
            tempsRestant = 0f;
            minuterieActive = false;

            ActualiserAffichage();

            if (GestionJeu.Instance != null)
            {
                GestionJeu.Instance.TempsEcoule();
            }

            return;
        }

        ActualiserAffichage();
    }

    private void ActualiserAffichage()
    {
        if (texteTimer == null)
            return;

        int minutes = Mathf.FloorToInt(tempsRestant / 60f);
        int secondes = Mathf.FloorToInt(tempsRestant % 60f);

        texteTimer.text = $"{minutes:00}:{secondes:00}";
    }

    public void ArreterMinuterie()
    {
        minuterieActive = false;
    }

    public string ObtenirTempsFormate()
    {
        int minutes = Mathf.FloorToInt(tempsRestant / 60f);
        int secondes = Mathf.FloorToInt(tempsRestant % 60f);

        return $"{minutes:00}:{secondes:00}";
    }
}