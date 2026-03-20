using UnityEngine;

public class StorageRoomManager : MonoBehaviour
{
    [Header("Références")]
    public CubeSpawner spawner; // Le script qui fait spawner les cubes
    public Transform caseObject; // L'objet 'Case' dans le mur

    [Header("Animation du Case")]
    public Vector3 caseOpenPositionOffset = new Vector3(0, 0, 0.5f); // De combien il doit sortir (modifier le Z en fonction de ton mur)
    public float openSpeed = 2f; // Vitesse d'ouverture

    private int cubesDestroyed = 0;
    private bool shouldOpenCase = false;
    private Vector3 initialCasePosition;
    private Vector3 targetCasePosition;

    void Start()
    {
        if (caseObject != null)
        {
            // On mémorise la position de base et on calcule la position d'ouverture (sur son axe local)
            initialCasePosition = caseObject.localPosition;
            targetCasePosition = initialCasePosition + caseOpenPositionOffset;
        }
    }

    void Update()
    {
        // Animation douce d'ouverture du 'Case'
        if (shouldOpenCase && caseObject != null)
        {
            caseObject.localPosition = Vector3.Lerp(caseObject.localPosition, targetCasePosition, Time.deltaTime * openSpeed);
        }
    }

    // Appelé quand le joueur prend le blaster (via l'event Select Entered de XR)
    public void OnBlasterGrabbed()
    {
        // Debug pour vérifier
        Debug.Log("Blaster saisi ! Début de l'éjection des cubes.");
        if (spawner != null)
        {
            spawner.StartSpawning();
        }
    }

    // Appelé par chaque cube quand il est touché par un tir
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
                spawner.StopSpawning(); // On arrête d'éjecter des cubes
            }
        }
    }
}
