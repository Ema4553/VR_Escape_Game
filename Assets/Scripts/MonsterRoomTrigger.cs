using UnityEngine;
using UnityEngine.SceneManagement; // Requis pour recharger la scène
using TMPro;

public class MonsterRoomTrigger : MonoBehaviour
{
    [Header("Références")]
    public Animator monsterAnimator; 
    public GameObject gameOverScreen; 
    public string animationTriggerName = "Attack"; 
    public float delaisAvantReload = 3f; // Temps à attendre avant de recommencer

    private bool triggered = false;

    void Start()
    {
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        if (monsterAnimator != null) monsterAnimator.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered && (other.CompareTag("Player") || other.GetComponentInChildren<Camera>() != null))
        {
            triggered = true;
            DeclencherGameOver();
        }
    }

    private void DeclencherGameOver()
    {
        Debug.Log("GAME OVER : Le monstre s'est réveillé !");

        if (monsterAnimator != null)
        {
            monsterAnimator.enabled = true;
            monsterAnimator.SetTrigger(animationTriggerName);
        }

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // On lance le compte à rebours pour recommencer le jeu !
        Invoke("RechargerLaPartie", delaisAvantReload);
    }

    private void RechargerLaPartie()
    {
        // On récupère le nom de la scène actuelle et on la relance
        string nomScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nomScene);
    }
}
