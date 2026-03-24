using UnityEngine;

public class VRProjectile : MonoBehaviour
{
    [Header("Paramètres du Projectile")]
    public float vitesse = 20f;      // Vitesse de déplacement du rayon
    public float dureeDeVie = 3f;    // Temps avant suppression (pour éviter lags)

    [Header("Effets Visuels")]
    [Tooltip("Objet (ex: Particules ou Lumière flash) qui apparait contre le mur")]
    public GameObject effetImpactPrefab; 

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // La capsule ayant été couchée de 90° sur X, c'est son axe Y (UP) qui est maitenant dirigé vers l'avant !
            rb.linearVelocity = transform.up * vitesse; 
        }
        else
        {
            Debug.LogWarning("VRProjectile a besoin d'un composant Rigidbody");
        }

        Destroy(gameObject, dureeDeVie);
    }

    void OnCollisionEnter(Collision col)
    {
        GererImpact(col.gameObject, col.contacts.Length > 0 ? col.contacts[0].point : transform.position, col.contacts.Length > 0 ? col.contacts[0].normal : -transform.up);
    }

    void OnTriggerEnter(Collider other)
    {
        GererImpact(other.gameObject, transform.position, -transform.up);
    }

    // Gère la logique d'impact pour les deux types de collision
    private void GererImpact(GameObject objetTouche, Vector3 pointDImpact, Vector3 normale)
    {
        // 3. Est-ce qu'on a touché un objectif (Cube) ?
        ShootableCube target = objetTouche.GetComponent<ShootableCube>();
        if (target != null)
        {
            target.Hit();
        }

        // 4. On crée une belle lumière ou explosion au point D'IMPACT EXACT sur le mur
        if (effetImpactPrefab != null)
        {
            Instantiate(effetImpactPrefab, pointDImpact, Quaternion.LookRotation(normale));
        }
        
        // 5. On détruit le rayon laser parce qu'il s'est écrasé
        Destroy(gameObject);
    }
}
