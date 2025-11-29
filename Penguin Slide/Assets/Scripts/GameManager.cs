using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    static GameManager instance = null;
    static public GameManager Instance {get {return instance;}}
    
    [SerializeField] private GameObject pushablesParent;
    private List<PushableObject> pushableObjects = new List<PushableObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        PushableObject[] pushables = pushablesParent.GetComponentsInChildren<PushableObject>();
        foreach (PushableObject pushable in pushables)
        {
            pushableObjects.Add(pushable);
        }
    }

    public bool CheckMovingObjects()
    {
        bool isMoving = false;
        foreach (PushableObject pushable in pushableObjects)
        {
            if (pushable.IsMoving)
            {
                isMoving = true;
                break;
            }
        }
        return isMoving;
    }
}
