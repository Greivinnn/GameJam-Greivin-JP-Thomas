using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] int doorUnlockID;
    public int DoorUnlockID {get{return doorUnlockID;}}

    public void Collect()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
