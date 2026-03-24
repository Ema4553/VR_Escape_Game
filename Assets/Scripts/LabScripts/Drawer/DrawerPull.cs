using UnityEngine;

public class DrawerPull : MonoBehaviour
{
    [Header("Réglages du mouvement")]
    public Vector3 openOffset = new Vector3(0, 0, 0.003f);
    public float speed = 2f;
    private Vector3 closedPos;
    private Vector3 targetPos;
    private bool isOpen = false;

    void Start()
    {
        closedPos = transform.localPosition;
        targetPos = closedPos;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime*speed);
    }

    public void ToggleDrawer()
    {
        isOpen = !isOpen;
        targetPos = isOpen ? closedPos + openOffset : closedPos;
    }

}
