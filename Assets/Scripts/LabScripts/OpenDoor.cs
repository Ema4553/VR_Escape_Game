using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;
using System.Collections;

public class OpenDoor : MonoBehaviour
{
    [Header("Réglages du mouvement")]
    public GameObject door;
    public Vector3 openOffset = new Vector3(2.38f, 0, 0);
    public float speed = 2f;

    [Header("Réglages du vérouillage des portes")]
    public bool isLocked = false;
    public string keyTag = "Handle";

    [Header("Réglages UI")]
    public TextMeshProUGUI statusText;
    public Renderer ledRenderer;
    public Color[] colors;

    [Header("Réglages Audio")]
    public AudioSource audioSource;
    public AudioClip doorSound;

    private Vector3 closedPos;
    private Vector3 targetPos;
    private bool isOpen = false;

    void Start()
    {
        closedPos = door.transform.localPosition;
        targetPos = closedPos;
        if (statusText != null) statusText.text = "";
        updateLED();
    }

    void Update()
    {
        door.transform.localPosition = Vector3.Lerp(door.transform.localPosition, targetPos, Time.deltaTime * speed);
    }

    public void OnDoorClicked(SelectEnterEventArgs args)
    {
        if (isLocked)
        {
            var interactor = args.interactorObject;
            if (CheckIfPlayerHasKey())
            {
                UnlockDoor();
            }
            else
            {
                ShowMessage("La porte est vérouillée ! Trouvez un objet permettant de la dévérouiller.");
            }
        }
        else
        {
            ToggleDoor();
        }
    }

    private bool CheckIfPlayerHasKey()
    {
        XRBaseInteractor[] interactors = Object.FindObjectsByType<XRBaseInteractor>(FindObjectsSortMode.None);
        
        foreach (XRBaseInteractor interactor in interactors)
        {
            if (interactor.interactablesSelected.Count > 0)
            {
                if (interactor.interactablesSelected[0].transform.CompareTag(keyTag))
                {
                    Destroy(interactor.interactablesSelected[0].transform.gameObject);
                    return true;
                }
            }
        }
        return false;
    }

    private void UnlockDoor()
    {
        isLocked = false;
        updateLED();
        ShowMessage("La porte est maintenant dévérouillée !");
        Debug.Log("Porte déverrouillée !");
    }

    private void ShowMessage(string message)
    {
        if (statusText != null)
        {
            StopAllCoroutines(); // Annule la disparition du message précédent si on clique vite
            statusText.text = message;
            StartCoroutine(HideMessageAfterDelay(3f));
        }
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        statusText.text = "";
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        
        // On joue le son de porte si tout est configuré
        if (audioSource != null && doorSound != null)
        {
            audioSource.PlayOneShot(doorSound);
        }

        targetPos = isOpen ? closedPos + openOffset : closedPos;
    }

    private void updateLED()
    {
        if (ledRenderer != null && colors.Length >= 2)
        {
            Color currentColor = isLocked ? colors[0] : colors[1];
            ledRenderer.material.color = currentColor;
        }
    }
}
