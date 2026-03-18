using UnityEngine;
using TMPro;

public class ComputerTerminal : MonoBehaviour
{
    [Header("Configuration de l'Écran")]
    [Tooltip("Glisser l'élément TextMeshPro de l'écran ici.")]
    public TextMeshProUGUI ecranTexte; 
    
    [Header("Paramètres de l'Enigme")]
    public string prefixeMessage = "e = ";
    public string reponseAttendue = "mc2";
    
    private string inputActuel = "";
    private bool estResolu = false;

    void Start()
    {
        MettreAJourEcran();
    }

    // Cette fonction sera appelée par les boutons (lettres/chiffres)
    public void AjouterLettre(string lettre)
    {
        if (estResolu) return;

        inputActuel += lettre;
        MettreAJourEcran();
        VerifierReponse();
    }

    // Permet d'effacer en cas d'erreur ou d'annulation
    public void EffacerInput()
    {
        if (estResolu) return;
        
        inputActuel = "";
        MettreAJourEcran();
    }

    // Affiche le texte sur l'écran et gère l'interface
    private void MettreAJourEcran()
    {
        if (estResolu)
        {
            ecranTexte.text = prefixeMessage + inputActuel + "\n<color=green>ACCES AUTORISE</color>";
        }
        else
        {
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

    private void VerifierReponse()
    {
        // On vérifie si la réponse tapée est exactement celle attendue (insensible à la casse)
        if (inputActuel.ToLower() == reponseAttendue.ToLower())
        {
            estResolu = true;
            MettreAJourEcran();
            AppelerActionReussite();
        }
        else if (inputActuel.Length >= reponseAttendue.Length)
        {
            // Si le texte a atteint la longueur limite mais est faux, on affiche Erreur
            // et on efface l'entrée automatiquement après 1.5 seconde
            ecranTexte.text = prefixeMessage + inputActuel + "\n<color=red>ERREUR</color>";
            Invoke("EffacerInput", 1.5f);
        }
    }

    private void AppelerActionReussite()
    {
        // Ajoute ici ce qui doit se passer quand le joueur gagne l'énigme
        Debug.Log("Enigme réussie ! L'ordinateur est débloqué.");
        
        // Exemple pour ouvrir une porte si tu as une référence :
        // if(porte != null) porte.Ouvrir();
    }
}
