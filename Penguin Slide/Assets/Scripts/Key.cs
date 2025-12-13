using Unity.VisualScripting;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] int doorUnlockID;
    public int DoorUnlockID {get{return doorUnlockID;}}
    private bool collected = false;
    public bool Collected {get{return collected;}}
    private bool used = false;
    public bool Used {get{return used;} set{used = value;}}
    private Collider2D keyCollider = null;
    private SpriteRenderer spriteRenderer = null;

    void Awake()
    {
        keyCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        keyCollider.enabled = !collected;
        spriteRenderer.enabled = !collected;
    }

    public void Collect()
    {
        collected = true;
    }
    public void LoadState(KeyState state)
    {
        collected = state.collected;
        used = state.used;
    }
    public KeyState GetState()
    {
        KeyState state = new KeyState();
        state.collected = collected;
        state.used = used;
        return state;
    }
    
}
