using UnityEngine;
using UnityEngine.SceneManagement;

public class GestionNiveaux : MonoBehaviour
{
    public void ChargerNiveau2()
    {
        Time.timeScale = 1f;

        SceneManager.LoadSceneAsync("Niveau2");
    }

    public void ChargerNiveau1()
    {
        Time.timeScale = 1f;

        SceneManager.LoadSceneAsync("Niveau1");
    }

    public void RecommencerNiveau()
    {
        Time.timeScale = 1f;

        SceneManager.LoadSceneAsync(
            SceneManager.GetActiveScene().name
        );
    }
}