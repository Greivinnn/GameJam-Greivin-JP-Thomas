using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    static GameManager instance = null;
    static public GameManager Instance {get {return instance;}}
    
    //empty gameobject which all pushable objects in the scene should be a child of
    [SerializeField] private GameObject pushablesParent; 
    //list of all pushable objects in the scene
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

    //returns true if any pushable objects in the scene are moving
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
