using UnityEngine;
using UnityEngine.InputSystem;

public class VRBlaster : MonoBehaviour
{
    [Header("Tir (Physique)")]
    public GameObject projectilePrefab; // L'objet 3D (le rayon laser) qui va traverser l'espace
    public Transform firePoint;         // Objet vide au bout du canon du Blaster
    public ParticleSystem muzzleFlash;

    // Permet de tester sur l'ordinateur sans le casque VR (Touche Espace)
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Fire();
        }
    }

    // Cette fonction devra être liée à l'évènement :
    // XR Grab Interactable -> Activated (Quand on appuie sur la gâchette VR)
    public void Fire()
    {
        // 1. Flash de l'arme (le petit effet visuel du canon)
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // 2. Création du véritable Laser physique qui voyage dans l'espace
        if (projectilePrefab != null && firePoint != null)
        {
            // Correction : une Capsule par défaut dans Unity est verticale (Axe Y).
            // On la tourne de 90° sur l'axe X pour la coucher (Axe Z) dans le sens du canon.
            Quaternion correctionRotation = firePoint.rotation * Quaternion.Euler(90f, 0f, 0f);
            
            GameObject nouveauLaser = Instantiate(projectilePrefab, firePoint.position, correctionRotation);
            
            // 1. Corrige le problème du "tir qui rentre dans l'arme" visuellement
            nouveauLaser.transform.position += firePoint.forward * (nouveauLaser.transform.localScale.y / 2f);

            // 2. CORRECTION CRUCIALE : Empêcher le laser de percuter le bouclier invisible du pistolet !
            Collider[] gunColliders = GetComponentsInChildren<Collider>();
            Collider[] laserColliders = nouveauLaser.GetComponentsInChildren<Collider>();
            
            foreach(var gunCol in gunColliders)
            {
                foreach(var laserCol in laserColliders)
                {
                    Physics.IgnoreCollision(gunCol, laserCol);
                }
            }

            Debug.Log("TIR VRBLASTER: Le laser a été propulsé et ignore désormais le pistolet !");
        }
        else
        {
            Debug.LogWarning("VRBlaster ERREUR: Il te manque le 'Projectile Prefab' OU le 'Fire point' dans l'inspector !");
        }
    }
}
