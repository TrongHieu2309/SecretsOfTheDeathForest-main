using UnityEngine;

public class PickupCoin : MonoBehaviour
{
    [SerializeField] private Transform destinationCoin;
    private Transform targetUI;
    private Vector3 startPos;
    private float speed = 6f;
    private bool isFlying = false;
    private CircleCollider2D circleCollider2D;

    void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    public void FlyToUI()
    {
        circleCollider2D.enabled = false;
        targetUI = destinationCoin;
        startPos = transform.position;
        isFlying = true;
    }

    void Update()
    {
        if (!isFlying) return;

        // coin bay gần target
        float t = speed * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetUI.position, t);
    }
}
