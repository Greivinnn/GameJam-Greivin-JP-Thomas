using System.Collections.Generic;
using System.Data;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    
    static GameManager instance = null;
    static public GameManager Instance {get {return instance;}}
    
    //empty gameobject which all pushable objects in the scene should be a child of
    [SerializeField] private GameObject pushablesParent; 
    [SerializeField] private PlayerController player;
    //list of all pushable objects in the scene
    private List<PushableObject> pushableObjects = new List<PushableObject>(); 
    private List<Vector3> rockSpawnPositions = new List<Vector3>(); 
    [SerializeField] private GameObject rockPrefab;

    //empty gameobject which all enemy objects in the scene should be a child of
    [SerializeField] private GameObject enemiesParent; 
    //list of all enemies in the scene
    private List<Enemy> enemies = new List<Enemy>(); 
    private List<Vector3> enemySpawnPositions = new List<Vector3>(); 
    [SerializeField] private GameObject enemyPrefab;

    private Vector3 spawnPos;

    private PlayerInput input = null;
    private InputAction restartAction = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        input = new PlayerInput();
        restartAction = input.UI.Restart;
        PushableObject[] pushables = pushablesParent.GetComponentsInChildren<PushableObject>();
        foreach (PushableObject pushable in pushables)
        {
            pushableObjects.Add(pushable);
            rockSpawnPositions.Add(pushable.transform.position);
        }
        Enemy[] newEnemies = enemiesParent.GetComponentsInChildren<Enemy>();
        foreach (Enemy enemy in newEnemies)
        {
            enemies.Add(enemy);
            enemySpawnPositions.Add(enemy.transform.position);
        }
        spawnPos = player.transform.position;
        restartAction.performed += OnRestart;
    }

        void OnEnable()
    {
        input.Enable();
        restartAction.Enable();
    }

    void OnDisable()
    {
        input.Disable();
        restartAction.Disable();
    }

    void Update()
    {

        if (!player.IsAlive)
        {
            
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

    public void SetSpawnPos(Vector3 position)
    {
        spawnPos = position;
    }

    public void Respawn()
    {
        player.Respawn(spawnPos);
        foreach (PushableObject pushable in pushableObjects)
        {
            Destroy(pushable.gameObject);
        }
        pushableObjects.Clear();
        foreach(Vector3 position in rockSpawnPositions)
        {
            PushableObject pushable = Instantiate(rockPrefab, position, quaternion.identity).GetComponent<PushableObject>();
            pushableObjects.Add(pushable);
        }
        // foreach (Enemy enemy in enemies)
        // {
        //     Destroy(enemy.gameObject);
        // }
        // enemies.Clear();
        // foreach (Vector3 position in enemySpawnPositions)
        // {
        //     Enemy enemy = Instantiate(enemyPrefab, position, quaternion.identity).GetComponent<Enemy>();
        //     enemies.Add(enemy);
        // }
    }

    void OnRestart(InputAction.CallbackContext context)
    {
        Respawn();
    }
}
