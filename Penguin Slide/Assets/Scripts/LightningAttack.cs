using UnityEngine;

public class LightningAttack : MonoBehaviour
{
    [SerializeField] float lifetime = 0.5f;
    bool isSetup = false;

    void Update()
    {
        // Destroy after lifetime if setup was completed
        if (isSetup)
        {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                Destroy(gameObject);
                isSetup = false;
            }
        }
    }

    // Call this to set up the lightning before it spawns
    public void Setup(Vector3 startPos, Vector3 endPos, Vector3 direction)
    {
        // Position at the exact midpoint between enemy and player
        transform.position = (startPos + endPos) * 0.5f;

        // Calculate distance between enemy and player
        float distance = Vector3.Distance(startPos, endPos);

        // Rotate lightning to face the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        transform.localScale = new Vector3(0.2f * distance, 1f, 1f);

        //// Scale the sprite to exactly match the distance
        //// Since pivot is at center, scaling from center will reach both ends
        //transform.localScale = new Vector3(distance / spriteWidth, transform.localScale.y, transform.localScale.z);

        //isSetup = true;
    }
}