using UnityEngine;

public class VRBlaster : MonoBehaviour
{
    [Header("Tir")]
    public Transform firePoint;     // Objet vide au bout du canon du Blaster
    public float weaponRange = 30f; // Distance de tir maximale du Raycast
    public ParticleSystem muzzleFlash;

    // Cette fonction devra être liée à l'évènement :
    // XR Grab Interactable -> Activated (Quand on appuie sur la gâchette VR)
    public void Fire()
    {
        // 1. Son ou flash basique pour le tir
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // 2. Traçage du tir (laser)
        if (Physics.Raycast(firePoint.position, firePoint.forward, out RaycastHit hit, weaponRange))
        {
            // Vérification visuelle
            Debug.Log("Le tir a touché : " + hit.transform.name);

            // 3. Est-ce que la cible qui vient d'être percutée gère les tirs (Cube) ?
            ShootableCube target = hit.collider.GetComponent<ShootableCube>();
            if (target != null)
            {
                // Message "J'ai été touché !" envoyé au cube
                target.Hit();
            }
        }
        else
        {
            Debug.Log("Le tir s'est perdu dans le vide.");
        }
    }
}
