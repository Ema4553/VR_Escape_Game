using UnityEngine;

public class StorageRoomManager : MonoBehaviour
{
    [Header("Références")]
    public CubeSpawner spawner; // Le script qui fait spawner les cubes
    public Transform caseObject; // L'objet 'Case' dans le mur

    [Header("Animation du Case")]
    public Vector3 caseOpenPositionOffset = new Vector3(0, 0, 0.5f); // De combien il doit sortir
    public float openSpeed = 2f; // Vitesse d'ouverture

    [Header("Objet à Libérer")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable poigneeVR; // La poignée à attraper

    private int cubesDestroyed = 0;
    private bool shouldOpenCase = false;
    private Vector3 initialCasePosition;
    private Vector3 targetCasePosition;

    void Start()
    {
        if (caseObject != null)
        {
            initialCasePosition = caseObject.localPosition;
            targetCasePosition = initialCasePosition + caseOpenPositionOffset;
        }

        // On bloque la saisie de la poignée au démarrage du jeu
        if (poigneeVR != null)
        {
            poigneeVR.enabled = false;
        }
    }

    void Update()
    {
        if (shouldOpenCase && caseObject != null)
        {
            caseObject.localPosition = Vector3.Lerp(caseObject.localPosition, targetCasePosition, Time.deltaTime * openSpeed);
        }
    }

    public void OnBlasterGrabbed()
    {
        Debug.Log("Blaster saisi ! Début de l'éjection des cubes.");
        if (spawner != null)
        {
            spawner.StartSpawning();
        }
    }

    public void OnCubeDestroyed()
    {
        cubesDestroyed++;
        Debug.Log("Cube détruit : " + cubesDestroyed + " / 5");

        if (cubesDestroyed >= 5 && !shouldOpenCase)
        {
            Debug.Log("Objectif atteint : les 5 cubes sont détruits ! Ouverture du Case.");
            shouldOpenCase = true;
            
            if (spawner != null)
            {
                spawner.StopSpawning();
            }

            // On déverrouille magiquement la poignée pour que la main VR puisse l'attraper !
            if (poigneeVR != null)
            {
                poigneeVR.enabled = true;
            }
        }
    }
}
