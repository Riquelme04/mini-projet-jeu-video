using UnityEngine;
using TMPro;

public class GestionJeu : MonoBehaviour
{
    public static GestionJeu Instance { get; private set; }

   
    // OBJECTIF D'ARGENT
   

    [Header("Objectif")]
    [SerializeField] private int objectifArgent = 26000;

    private int argentCollecte = 0;
    private bool objectifAtteint = false;
    private bool partieTerminee = false;



    // INTERFACE PENDANT LE JEU
   

    [Header("Interface de jeu")]
    [SerializeField] private TMP_Text texteArgentHUD;


  
    // MINUTERIE
 

    [Header("Minuterie")]
    [SerializeField] private MinuterieJeu minuterie;



    // SORTIE


    [Header("Sortie")]
    [SerializeField] private GameObject portePrincipale;


    // ÉCRAN DE FIN

    [Header("Écran de fin")]
    [SerializeField] private GameObject panelFin;
    [SerializeField] private TMP_Text texteResultat;
    [SerializeField] private TMP_Text texteButin;
    [SerializeField] private TMP_Text texteTemps;



    // PROPRIÉTÉS PUBLIQUES
  

    public bool ObjectifAtteint => objectifAtteint;
    public bool PartieTerminee => partieTerminee;



    // AWAKE
   

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


 
    // START
 

    private void Start()
    {
        // Cache l'écran de fin au début.
        if (panelFin != null)
        {
            panelFin.SetActive(false);
        }

        // La sortie est fermée au début.
        if (portePrincipale != null)
        {
            portePrincipale.SetActive(true);
        }

        // Affiche 0 $ / 26 000 $ au début.
        MettreAJourArgentHUD();
    }


    // ARGENT


    public bool AjouterArgent(int valeur)
    {
        // Impossible de prendre de l'argent
        // si la partie est déjà terminée.
        if (partieTerminee)
        {
            return false;
        }

        // Ajoute la valeur du billet.
        argentCollecte += valeur;

        // Met immédiatement à jour l'interface.
        MettreAJourArgentHUD();

        Debug.Log(
            $"Trevor a récupéré {valeur}$ - Total : " +
            $"{argentCollecte}$/{objectifArgent}$"
        );

        // Vérifie si tout l'argent nécessaire
        // a été récupéré.
        if (argentCollecte >= objectifArgent &&
            !objectifAtteint)
        {
            objectifAtteint = true;

            OuvrirSortie();
        }

        return true;
    }


    // MISE À JOUR DU HUD


    private void MettreAJourArgentHUD()
    {
        if (texteArgentHUD == null)
        {
            return;
        }

        texteArgentHUD.text =
            $"ARGENT : {argentCollecte:N0} $ / {objectifArgent:N0} $";
    }


    // CLÉS
    

    public bool RecupererCle(int numeroCle, GameObject porteAssociee)
{
    if (partieTerminee)
    {
        return false;
    }

    Debug.Log($"Clé {numeroCle} récupérée !");

    // La porte associée à cette clé disparaît.
    if (porteAssociee != null)
    {
        porteAssociee.SetActive(false);

        Debug.Log($"La porte associée à la clé {numeroCle} est ouverte !");
    }
    else
    {
        Debug.LogWarning(
            $"Aucune porte associée à la clé {numeroCle}."
        );
    }

    return true;
}


  
    // OUVERTURE DE LA SORTIE PRINCIPALE
    

    private void OuvrirSortie()
    {
        Debug.Log(
            "Tout l'argent a été récupéré ! " +
            "Trevor doit maintenant atteindre la sortie !"
        );

        // La porte principale disparaît.
        if (portePrincipale != null)
        {
            portePrincipale.SetActive(false);
        }
    }



    // TENTER DE SORTIR


    public void TenterSortie(GameObject joueur)
    {
        if (partieTerminee)
        {
            return;
        }

        // Trevor ne peut pas sortir s'il
        // n'a pas récupéré assez d'argent.
        if (!objectifAtteint)
        {
            Debug.Log(
                $"Impossible de sortir ! Argent : " +
                $"{argentCollecte}$/{objectifArgent}$"
            );

            return;
        }

        Victoire(joueur);
    }



    // VICTOIRE

    private void Victoire(GameObject joueur)
    {
        partieTerminee = true;

        // Arrête la minuterie.
        if (minuterie != null)
        {
            minuterie.ArreterMinuterie();
        }

        string temps =
            minuterie != null
                ? minuterie.ObtenirTempsFormate()
                : "--:--";

        Debug.Log(
            $"VICTOIRE ! Trevor s'est échappé avec " +
            $"{argentCollecte}$ !"
        );

        // Affiche le panneau de fin.
        if (panelFin != null)
        {
            panelFin.SetActive(true);
        }

        // Affiche MISSION RÉUSSIE.
        if (texteResultat != null)
        {
            texteResultat.text = "MISSION RÉUSSIE !";
        }

        // Affiche le butin.
        if (texteButin != null)
        {
            texteButin.text =
                $"Butin : {argentCollecte:N0} $";
        }

        // Affiche le temps restant.
        if (texteTemps != null)
        {
            texteTemps.text =
                $"Temps restant : {temps}";
        }

        // Cache Trevor après la victoire.
        if (joueur != null)
        {
            joueur.SetActive(false);
        }
    }


    // DÉFAITE
    public void TempsEcoule()
    {
        if (partieTerminee)
        {
            return;
        }

        partieTerminee = true;

        Debug.Log(
            "TEMPS ÉCOULÉ ! MISSION ÉCHOUÉE."
        );

        // Affiche le panneau de fin.
        if (panelFin != null)
        {
            panelFin.SetActive(true);
        }

        // Message d'échec.
        if (texteResultat != null)
        {
            texteResultat.text =
                "MISSION ÉCHOUÉE !";
        }

        // Affiche l'argent récupéré malgré l'échec.
        if (texteButin != null)
        {
            texteButin.text =
                $"Butin : {argentCollecte:N0} $";
        }

        // Le temps est terminé.
        if (texteTemps != null)
        {
            texteTemps.text =
                "Temps restant : 00:00";
        }
    }
}