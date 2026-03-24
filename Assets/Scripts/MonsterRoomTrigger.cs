using UnityEngine;
using UnityEngine.SceneManagement; 

public class MonsterRoomTrigger : MonoBehaviour
{
    [Header("Références")]
    public Animator monsterAnimator; 
    public GameObject gameOverScreen; 
    public string animationTriggerName = "Attack"; 
    
    [Header("Réglages")]
    public float delaisAvantReload = 2f; // Réglé sur 2 secondes pour plus de vitesse !

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip attackSound;

    private bool triggered = false;

    void Start()
    {
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        if (monsterAnimator != null) monsterAnimator.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // On détecte la tête ou le corps du joueur
        if (!triggered && (other.CompareTag("Player") || other.GetComponentInChildren<Camera>() != null))
        {
            triggered = true;
            Debug.Log("<color=orange>🏁 TRIGGER DÉTECTÉ !</color>");
            DeclencherGameOver();
        }
    }

    private void DeclencherGameOver()
    {
        Debug.Log("<color=yellow>💀 DEBUT DU GAME OVER...</color>");

        // 1. Son
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // 2. Animation
        if (monsterAnimator != null)
        {
            monsterAnimator.enabled = true;
            monsterAnimator.SetTrigger(animationTriggerName);
        }

        // 3. UI
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // 4. Lancement du délais
        Debug.Log("<color=orange>⏳ Délai de 2s avant le reload lancé.</color>");
        Invoke("RechargerLaPartie", delaisAvantReload);
    }

        private void RechargerLaPartie()
    {
        // Dans tes Build Settings, LabRoom est le numéro 0.
        // C'est l'ordre le plus puissant que Unity ne peut pas ignorer.
        Debug.Log("<color=red>🔄 RELOAD INDEX 0 (LabRoom) lancé !</color>");
        
        // On force le chargement du premier niveau de la liste
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
