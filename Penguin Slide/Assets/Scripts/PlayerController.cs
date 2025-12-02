using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform destination;

    private PlayerInput input = null;
    private InputAction moveAction = null;

    private bool isMoving;
    public bool IsMoving {get {return isMoving;}}

    private bool movingToWater = false;  //makes the player die when they reach the destination

    private bool isAlive = true;

    //stores the pushable object that the player is currently moving to, if there is one
    private PushableObject pushableAtDestination = null; 
    Vector3 pushDirection = Vector3.zero;

    private Vector2 spriteDirection = Vector2.down;

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
        if (isAlive)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();

            if (destination.position == transform.position && moveInput != Vector2.zero && !GameManager.Instance.CheckMovingObjects())
            {
                
                ChangeDestination(moveInput);
            }

            //linearly moves the player to the destination
            transform.position = Vector3.MoveTowards(transform.position,destination.position,moveSpeed*Time.deltaTime);
            
            //determine if the player is moving or stopped
            isMoving = !(transform.position == destination.position);
            if (!IsMoving)
            {
                if(pushableAtDestination != null && pushDirection.magnitude == 1)
                {
                    //this code runs when the player pushes an object
                    pushableAtDestination.ChangeDestination(pushDirection);
                    pushableAtDestination = null;
                    pushDirection = Vector3.zero;
                }
                if (movingToWater)
                {
                    //this code runs when the player falls in water
                    movingToWater = false;
                    Debug.Log("You died");
                    isAlive = false;
                }
            }
        }
    }

    //updates the destination point based on the given input direction
    void ChangeDestination(Vector2 moveInput)
    {
        if (moveInput.x == 1 || moveInput.x == -1)
        {
            //horizontal movement
            Vector3 direction = Vector3.zero;
            direction.x += moveInput.x;
            spriteDirection = direction;
            destination.position = GetTravelDistance(direction);
        }
        else if (moveInput.y == 1 || moveInput.y == -1)
        {
            //vertical movement
            Vector3 direction = Vector3.zero;
            direction.y += moveInput.y;
            spriteDirection = direction;
            destination.position = GetTravelDistance(direction);
        }
    }
    
    //finds and returns the position that should be moved to in the given direction
    Vector3 GetTravelDistance(Vector3 direction)
    {
        const float maxRaycastDistance = 50.0f;
        Vector3 targetTile = transform.position + direction * maxRaycastDistance;

        //sets the origin point for raycasts to one tile ahead of the player
        Vector3 rayOrigin = transform.position + direction;
        //gets the closest collision with a wall in the given direction
        RaycastHit2D wallHit = Physics2D.Raycast(rayOrigin,direction, maxRaycastDistance, LayerMask.GetMask("Walls"));
        //gets the closest collision with a floor or water tile in the given direction
        RaycastHit2D floorHit = Physics2D.Raycast(rayOrigin,direction, maxRaycastDistance, LayerMask.GetMask("Floor", "Water"));
        if(wallHit.collider != null)
        {
            //gets the position of the collision
            Vector3 wallPosition = wallHit.point;

            //rounds the collision to nearest 0.5 to avoid weird errors
            wallPosition.x = MathF.Round(wallPosition.x * 2.0f) / 2.0f;
            wallPosition.y = MathF.Round(wallPosition.y * 2.0f) / 2.0f;   

            //offset to fix collision in negative directions with perfectly square tiles
            if (wallPosition.x % 1 == 0 && direction.x == -1)
            {
                wallPosition.x--;
            }
            if (wallPosition.y % 1 == 0 && direction.y == -1)
            {
                wallPosition.y--;
            }

            //floor then add 0.5 to get the middle point of the tile
            wallPosition.x = MathF.Floor(wallPosition.x) + 0.5f;
            wallPosition.y = MathF.Floor(wallPosition.y) + 0.5f;

            //move one tile backwards to get the tile before the wall
            targetTile = wallPosition - direction;

            //queue pushable to be pushed if relevant
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

            //use floor position only if it is closer than wall position
            if ((floorPosition-transform.position).magnitude <= (targetTile-transform.position).magnitude)
            {
                targetTile = floorPosition;

                //do not push if moving to a floor tile
                pushableAtDestination = null; 
                pushDirection = Vector3.zero;
                if (floorHit.collider.tag == "Water" && targetTile != transform.position)
                {
                    movingToWater = true; //queues water death
                }
            }
        }

        return targetTile;
    }
}
