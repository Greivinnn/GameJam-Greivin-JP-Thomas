using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    static GameManager instance = null;
    static public GameManager Instance {get {return instance;}}
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
