using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float sightDistance = 20.0f;
    [SerializeField] PlayerController player = null;
<<<<<<< Updated upstream
    Vector3 sightPositionR;
    Vector3 sightPositionL;
    Vector3 sightPositionU;
    Vector3 sightPositionD;
    LineRenderer lineRenderer = null;
    bool isAlive = true;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
=======
<<<<<<< HEAD
    Animator animator = null;
    void Awake()
    {
        animator = GetComponent<Animator>();
=======
    Vector3 sightPositionR;
    Vector3 sightPositionL;
    Vector3 sightPositionU;
    Vector3 sightPositionD;
    LineRenderer lineRenderer = null;
    bool isAlive = true;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
>>>>>>> 1940d617bb0eced16a1e38c4f8c1a4d58a7fefaf
>>>>>>> Stashed changes

    }

    void Update()
    {
<<<<<<< Updated upstream
=======
<<<<<<< HEAD
        if (CheckSightLines())
        {
            player.OnEnemyHit();
            animator.SetBool("IsAttacking", true);
=======
>>>>>>> Stashed changes
        if (isAlive)
        {   
            if (CheckSightLines())
            {
                player.OnEnemyHit();
            }
            UpdateLineRenderer();
<<<<<<< Updated upstream
=======
>>>>>>> 1940d617bb0eced16a1e38c4f8c1a4d58a7fefaf
>>>>>>> Stashed changes
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
            
            RaycastHit2D raycastHit = Physics2D.Raycast(rayOrigin,direction, sightDistance, LayerMask.GetMask("Walls", "Player"));
            
            if (direction.x > 0)
            {
                sightPositionR = raycastHit.point;
            }
            else if (direction.x < 0)
            {
                sightPositionL = raycastHit.point;
            }
            else if (direction.y > 0)
            {
                sightPositionU = raycastHit.point;
            }
            else if (direction.y < 0)
            {
                sightPositionD = raycastHit.point;
            }

            if (raycastHit.collider.tag == "Player")
            {
                playerFound = true;
                break;
            }
        }
        return playerFound;
    }
    void UpdateLineRenderer()
    {
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1,sightPositionR);
        lineRenderer.SetPosition(2,sightPositionL);
        lineRenderer.SetPosition(3,transform.position);
        lineRenderer.SetPosition(4,sightPositionU);
        lineRenderer.SetPosition(5,sightPositionD);
    }

    public void OnDeath()
    {
        isAlive = false;
        Debug.Log("Enemy died");
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
