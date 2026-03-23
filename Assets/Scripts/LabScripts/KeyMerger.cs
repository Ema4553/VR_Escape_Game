using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KeyMerger : MonoBehaviour
{
    public GameObject completeKeyPrefab;
    private bool _isMerging = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("KeyPart"))
        {
            KeyMerger otherScript = collision.gameObject.GetComponent<KeyMerger>();
            if (otherScript != null & otherScript._isMerging) return;
            _isMerging = true;
            Merge(collision.gameObject);
        }
    }

    void Merge(GameObject otherPart)
    {
        Vector3 spawnPosition = (transform.position + otherPart.transform.position) / 2;

        GameObject newKey = Instantiate(completeKeyPrefab, spawnPosition, Quaternion.identity);

        XRGrabInteractable grabInteractable = newKey.GetComponent<XRGrabInteractable>();

        GameObject cameraMessage = GameObject.FindWithTag("GazeMessage");

        grabInteractable.hoverEntered.AddListener((args) => {
            cameraMessage.SetActive(true);
            });

        grabInteractable.hoverExited.AddListener((args) => {
            cameraMessage.SetActive(false);
            
            });

        Destroy(otherPart);
        Destroy(gameObject);
    }
}
