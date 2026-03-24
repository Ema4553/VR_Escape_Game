using UnityEngine;
using TMPro;

public class MonsterRoomTrigger : MonoBehaviour
{
    [Header("Références")]
    public Animator monsterAnimator; // L'animator sur ton monstre
    public GameObject gameOverScreen; // Le Canvas ou l'objet Game Over à afficher
    public string animationTriggerName = "Attack"; // Le nom exact du trigger dans l'Animator

    private bool triggered = false;

    void Start()
    {
        // On s'assure que tout est éteint au début
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        
        // Optionnel : on peut mettre l'animator en pause au début pour économiser
        if (monsterAnimator != null) monsterAnimator.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si c'est le joueur qui entre (le XR Origin possède souvent un collider ou on check le tag)
        if (!triggered && (other.CompareTag("Player") || other.GetComponentInChildren<Camera>() != null))
        {
            triggered = true;
            DeclencherGameOver();
        }
    }

    private void DeclencherGameOver()
    {
        Debug.Log("GAME OVER : Le monstre s'est réveillé !");

        // 1. Réveil de l'animator et lancement de l'attaque
        if (monsterAnimator != null)
        {
            monsterAnimator.enabled = true;
            monsterAnimator.SetTrigger(animationTriggerName);
        }

        // 2. Affichage de l'écran de fin
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        
        // Ici tu pourrais aussi ajouter un son de cri de monstre !
    }
}
