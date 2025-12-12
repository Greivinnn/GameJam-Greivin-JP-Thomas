using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private int doorID = 0;
    public int DoorID {get {return doorID;}}
    [SerializeField] bool unlocked = false;
    public bool Unlocked {get {return unlocked;}}
    Collider2D doorCollider = null;
    SpriteRenderer spriteRenderer = null;

    void Awake()
    {
        doorCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Unlock()
    {
        unlocked = true;
    }
    void Update()
    {
        spriteRenderer.enabled = !unlocked;
        doorCollider.enabled = !unlocked;
    }
    public void LoadState(bool isUnlocked)
    {
        unlocked = isUnlocked;
    }
}
