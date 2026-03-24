using UnityEngine;
using TMPro;

public class ComputerTerminal : MonoBehaviour
{
    [Header("Configuration de l'Écran")]
    public TextMeshProUGUI ecranTexte; 
    
    [Header("Paramètres de l'Enigme")]
    public string prefixeMessage = "e = ";
    public string reponseAttendue = "mc2";
    
    [Header("État de la Sortie")]
    public bool doorOpen = false; // La variable demandée

    [Header("Porte")]
    public OpenDoor porteALiberer; // La porte qui doit s'ouvrir

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip beepSound;

    private string inputActuel = "";
    private bool estResolu = false;

    void Start()
    {
        doorOpen = false;
        MettreAJourEcran();
    }

    /// <summary>
    /// Appelé uniquement par les touches 'm', 'c' et '2'
    /// </summary>
    public void ToucheSpeciale(string lettre)
    {
        if (estResolu) return;

        // On joue le petit bip sonore
        if (audioSource != null && beepSound != null)
        {
            audioSource.PlayOneShot(beepSound);
        }

        inputActuel += lettre;
        MettreAJourEcran();
    }

    /// <summary>
    /// Appelé par le bouton 'STOP' pour valider
    /// </summary>
    public void ValiderReponse()
    {
        if (estResolu) return;

        if (inputActuel.ToLower() == reponseAttendue.ToLower())
        {
            Reussite();
        }
        else
        {
            Echec();
        }
    }

    private void Reussite()
    {
        estResolu = true;
        doorOpen = true; // On ouvre la porte !
        
        // On déverrouille et on ouvre la porte physiquement
        if (porteALiberer != null)
        {
            porteALiberer.isLocked = false;
            porteALiberer.ToggleDoor();
        }

        ecranTexte.text = "<color=green>ACCÈS AUTORISÉ</color>";
        Debug.Log("Code correct ! doorOpen est maintenant TRUE et la porte s'ouvre.");
    }

    private void Echec()
    {
        ecranTexte.text = "<color=red>CODE ERRONÉ</color>";
        // On laisse le message d'erreur 1.5s puis on efface tout
        Invoke("EffacerInput", 1.5f);
    }

    public void EffacerInput()
    {
        if (estResolu) return;
        inputActuel = "";
        MettreAJourEcran();
    }

    private void MettreAJourEcran()
    {
        if (estResolu) return;

        if (string.IsNullOrEmpty(inputActuel))
        {
            ecranTexte.text = prefixeMessage + "?";
        }
        else
        {
            ecranTexte.text = prefixeMessage + inputActuel;
        }
    }
}
