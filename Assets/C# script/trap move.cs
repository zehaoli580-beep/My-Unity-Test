using UnityEngine;

public class trapmove : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 targetPosition;
    private bool movingToB = true;

    private void Start()
    {
        targetPosition = pointB.position;
    }

    private void Update()
    {
        if (pointA == null || pointB == null) return;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (transform.position == pointB.position)
        {
            targetPosition = pointA.position;
        }
        else if (transform.position == pointA.position)
        {
            targetPosition = pointB.position;
        }
    }
}
