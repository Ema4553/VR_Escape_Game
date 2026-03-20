using UnityEngine;

public class ShootableCube : MonoBehaviour
{
    private StorageRoomManager manager;

    void Start()
    {
        // Le cube va chercher automatiquement le Manager tout seul (pratique car il est spawn dynamiquement)
        manager = FindFirstObjectByType<StorageRoomManager>();
    }

    // Appelé par le tir du Blaster
    public void Hit()
    {
        if (manager != null)
        {
            manager.OnCubeDestroyed(); // On prévient de la destruction de l'objectif
        }
        else
        {
            Debug.LogWarning("Manager introuvable pour enregistrer le point du ShootableCube!");
        }

        // Effet visuel simple (si pas de particules)
        Renderer r = GetComponent<Renderer>();
        if(r != null) r.material.color = Color.black; 
        
        // Destruction de l'objet de base pour ne pas surcharger la scène
        Destroy(gameObject, 0.1f);
    }
}
