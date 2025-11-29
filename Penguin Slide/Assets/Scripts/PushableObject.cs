using UnityEngine;
using System;
public class PushableObject : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform destination;
    private bool isMoving;
    public bool IsMoving {get {return isMoving;}}
    
    private PushableObject pushableAtDestination = null;
    Vector3 pushDirection = Vector3.zero;

    void Awake()
    {
        destination.parent = null;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,destination.position,moveSpeed*Time.deltaTime);
        isMoving = !(transform.position == destination.position);
        if (!isMoving)
        {
            if(pushableAtDestination != null && pushDirection.magnitude == 1)
            {
                pushableAtDestination.ChangeDestination(pushDirection);
                pushableAtDestination = null;
                pushDirection = Vector3.zero;
            }
        }
       
    }

    public void ChangeDestination(Vector3 pushDirection)
    {
        if (pushDirection.x == 1 || pushDirection.x == -1)
        {
            Vector3 direction = Vector3.zero;
            direction.x += pushDirection.x;
            destination.position = GetTravelDistance(direction);
        }
        else if (pushDirection.y == 1 || pushDirection.y == -1)
        {
            Vector3 direction = Vector3.zero;
            direction.y += pushDirection.y;
            destination.position = GetTravelDistance(direction);
        }
    }

    Vector3 GetTravelDistance(Vector3 direction)
    {
        const float maxRaycastDistance = 50.0f;
        Vector3 targetTile = transform.position + direction * maxRaycastDistance;

        Vector3 rayOrigin = transform.position + direction;
        RaycastHit2D wallHit = Physics2D.Raycast(rayOrigin,direction, maxRaycastDistance, LayerMask.GetMask("Walls"));
        RaycastHit2D floorHit = Physics2D.Raycast(rayOrigin,direction, maxRaycastDistance, LayerMask.GetMask("Floor"));
        if(wallHit.collider != null)
        {
            Vector3 wallPosition = wallHit.point;

            wallPosition.x = MathF.Round(wallPosition.x * 2.0f) / 2.0f;
            wallPosition.y = MathF.Round(wallPosition.y * 2.0f) / 2.0f;   

            if (wallPosition.x % 1 == 0 && direction.x == -1)
            {
                wallPosition.x--;
            }
            if (wallPosition.y % 1 == 0 && direction.y == -1)
            {
                wallPosition.y--;
            }
            wallPosition.x = MathF.Floor(wallPosition.x) + 0.5f;
            wallPosition.y = MathF.Floor(wallPosition.y) + 0.5f;
            targetTile = wallPosition - direction;
            if (wallHit.collider.tag == "Pushable")
            {
                pushableAtDestination = wallHit.collider.GetComponent<PushableObject>();
                pushDirection = direction;
            }
        }
        if (floorHit.collider != null)
        {
            Vector3 floorPosition = floorHit.point;

            floorPosition.x = MathF.Round(floorPosition.x * 2.0f) / 2.0f;
            floorPosition.y = MathF.Round(floorPosition.y * 2.0f) / 2.0f;   

            if (floorPosition.x % 1 == 0 && direction.x == -1)
            {
                floorPosition.x--;
            }
            if (floorPosition.y % 1 == 0 && direction.y == -1)
            {
                floorPosition.y--;
            }
            floorPosition.x = MathF.Floor(floorPosition.x) + 0.5f;
            floorPosition.y = MathF.Floor(floorPosition.y) + 0.5f;
            if ((floorPosition-transform.position).magnitude <= (targetTile-transform.position).magnitude)
            {
                targetTile = floorPosition;
                pushableAtDestination = null;
                pushDirection = Vector3.zero;
            }
        }

        return targetTile;
    }

}
