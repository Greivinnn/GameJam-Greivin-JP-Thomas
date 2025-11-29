using System.Xml.Serialization;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float iceSlideSpeed = 6f;
    [SerializeField] private Transform destination;

    private PlayerInput input = null;
    private InputAction moveAction = null;


    void Awake()
    {
        destination.parent = null;
        input = new PlayerInput();
        moveAction = input.Player.Move;
    }

    void OnEnable()
    {
        input.Enable();
        moveAction.Enable();
    }

    void OnDisable()
    {
        input.Disable();
        moveAction.Disable();
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        if (destination.position == transform.position && moveInput != Vector2.zero)
        {
            ChangeDestination(moveInput);
        }
        transform.position = Vector3.MoveTowards(transform.position,destination.position,moveSpeed*Time.deltaTime);
    }

    void ChangeDestination(Vector2 moveInput)
    {
        if (moveInput.x == 1 || moveInput.x == -1)
        {
            Vector3 direction = Vector3.zero;
            direction.x += moveInput.x;
            destination.position = GetTravelDistance(direction);
        }
        else if (moveInput.y == 1 || moveInput.y == -1)
        {
            Vector3 direction = Vector3.zero;
            direction.y += moveInput.y;
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
            if (wallPosition.x % 1 == 0 && direction.x == -1)
            {
                wallPosition.x--;
            }
            if (wallPosition.y % 1 == 0 && direction.y == -1)
            {
                wallPosition.y--;
            }
            wallPosition.x = math.floor(wallPosition.x) + 0.5f;
            wallPosition.y = math.floor(wallPosition.y) + 0.5f;
            targetTile = wallPosition - direction;
        }
        if (floorHit.collider != null)
        {
            Vector3 floorPosition = floorHit.point;
            if (floorPosition.x % 1 == 0 && direction.x == -1)
            {
                floorPosition.x--;
            }
            if (floorPosition.y % 1 == 0 && direction.y == -1)
            {
                floorPosition.y--;
            }
            floorPosition.x = math.floor(floorPosition.x) + 0.5f;
            floorPosition.y = math.floor(floorPosition.y) + 0.5f;
            if ((floorPosition-transform.position).magnitude < (targetTile-transform.position).magnitude)
            {
                targetTile = floorPosition;
            }
        }

        return targetTile;
    }
}
