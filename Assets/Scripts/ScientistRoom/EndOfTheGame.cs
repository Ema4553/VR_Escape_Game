using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfTheGame : MonoBehaviour
{
    public GameObject portalDetector;
    public Renderer portalRenderer;
    public GameObject key;
    public GameObject keyVerifZone;
    public Color[] colors;
    private static bool isPortalActive = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successSound;

    void Start()
    {
        if (portalRenderer != null && colors.Length >= 2)
        {
            portalRenderer.material.color = isPortalActive ? colors[1] : colors[0];
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key") && !isPortalActive)
        {
            OpenPortal();
        }

        if (isPortalActive && other.CompareTag("MainCamera"))
        {
            SceneManager.LoadScene("EndScene");
            Debug.Log("CHARGEMENT SCENE");
        }
    }

    void OpenPortal()
    {
        isPortalActive = true;
        
        if (audioSource != null && successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }

        portalRenderer.material.color = colors[1];
        Debug.Log("Le portail est ACTIF");
    }
}
