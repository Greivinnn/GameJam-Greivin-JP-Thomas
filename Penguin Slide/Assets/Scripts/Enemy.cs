using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float sightDistance = 20.0f;
    [SerializeField] PlayerController player = null;
    void Awake()
    {
        

    }

    void Update()
    {
        if (CheckSightLines())
        {
            player.OnEnemyHit();
        }
    }
    bool CheckSightLines()
    {
        Vector3 rayOrigin = transform.position;

        bool playerFound = false;

        for (int i = 0; i < 4; i++)
        {
            Vector3 direction = Vector3.zero;
            if (i < 2)
            {
                direction.x = i % 2 * 2 - 1;
            }
            else
            {
                direction.y = i % 2 * 2 - 1;
            }
            
            RaycastHit2D raycastHit = Physics2D.Raycast(rayOrigin + direction,direction, sightDistance, LayerMask.GetMask("Walls", "Player"));
            
            if (raycastHit.collider.tag == "Player")
            {
                playerFound = true;
                break;
            }
        }
        return playerFound;
    }
}
