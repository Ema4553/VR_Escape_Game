using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables; // Pour Unity 6

[RequireComponent(typeof(LineRenderer))]
public class VRLaserSight : MonoBehaviour
{
    [Header("Paramètres du Viseur")]
    public float distanceMax = 500f; // Portée géante du laser
    public LayerMask ceQuiArreteLeLaser = ~0;

    private LineRenderer lineRenderer;
    private XRGrabInteractable armeParent;
    
    // Le point visuel au bout du laser
    private GameObject pointViseur;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.005f;
        lineRenderer.endWidth = 0.005f;
        
        if (lineRenderer.material == null) {
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }
        
        // FORCER la couleur VERTE
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        // Créer un petit point (sphère) rouge/verte automatique pour la visée
        pointViseur = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Destroy(pointViseur.GetComponent<SphereCollider>()); // Pas besoin de collisions sur le pointeur
        pointViseur.transform.localScale = new Vector3(0.04f, 0.04f, 0.04f); // Petite boule
        pointViseur.GetComponent<Renderer>().material = lineRenderer.material; // Même couleur verte brilllante

        // On éteint le viseur par défaut
        lineRenderer.enabled = false;
        pointViseur.SetActive(false);

        // On cherche le composant pour attraper l'arme (sur le parent)
        armeParent = GetComponentInParent<XRGrabInteractable>();
        if (armeParent != null)
        {
            armeParent.selectEntered.AddListener(AllumerViseur);
            armeParent.selectExited.AddListener(EteindreViseur);
        }
    }

    void OnDestroy()
    {
        if (armeParent != null)
        {
            armeParent.selectEntered.RemoveListener(AllumerViseur);
            armeParent.selectExited.RemoveListener(EteindreViseur);
        }
    }

    private void AllumerViseur(SelectEnterEventArgs args) 
    {
        lineRenderer.enabled = true;
        pointViseur.SetActive(true);
    }
    
    private void EteindreViseur(SelectExitEventArgs args) 
    {
        lineRenderer.enabled = false;
        pointViseur.SetActive(false);
    }

    void Update()
    {
        // Ne rien calculer si le viseur est éteint
        if (!lineRenderer.enabled) return;

        lineRenderer.SetPosition(0, transform.position);

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, distanceMax, ceQuiArreteLeLaser))
        {
            lineRenderer.SetPosition(1, hit.point);
            
            // Placer le point viseur exactement sur le mur touché
            pointViseur.SetActive(true);
            pointViseur.transform.position = hit.point;
        }
        else
        {
            // S'il vise dans le vide, tracer loin devant et enlever le point
            lineRenderer.SetPosition(1, transform.position + transform.forward * distanceMax);
            pointViseur.SetActive(false);
        }
    }
}
