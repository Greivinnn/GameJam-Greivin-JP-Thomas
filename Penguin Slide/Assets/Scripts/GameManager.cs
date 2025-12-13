using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using NUnit.Framework;
using Unity.Mathematics;
using Unity.VisualScripting;
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

    public List<Key> keysHeld = new List<Key>();

    [SerializeField] private GameObject doorsParent;
    private List<Door> doors = new List<Door>();

    [SerializeField] private GameObject keysParent;
    private List<Key> keys = new List<Key>();

    private List<SaveState> previousStates = new List<SaveState>();
    private int maxStepsSaved = 100;


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

        Door[] newDoors = doorsParent.GetComponentsInChildren<Door>();
        foreach(Door door in newDoors)
        {
            doors.Add(door);
        }

        Key[] newKeys = keysParent.GetComponentsInChildren<Key>();
        foreach(Key key in newKeys)
        {
            keys.Add(key);
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
            Destroy(pushable.GetComponentInChildren<Transform>().gameObject);
            Destroy(pushable.gameObject);
        }
        pushableObjects.Clear();
        foreach(Vector3 position in rockSpawnPositions)
        {
            PushableObject pushable = Instantiate(rockPrefab, position, quaternion.identity).GetComponent<PushableObject>();
            pushableObjects.Add(pushable);
        }
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.GetComponentInChildren<Transform>().gameObject);
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
        foreach (Vector3 position in enemySpawnPositions)
        {
            Enemy enemy = Instantiate(enemyPrefab, position, quaternion.identity).GetComponent<Enemy>();
            enemy.player = player;
            enemies.Add(enemy);
        }
    }

    void OnRestart(InputAction.CallbackContext context)
    {
        Respawn();
    }

    public void CollectKey(Key key)
    {
        keysHeld.Add(key);
        key.Collect();
    }

    public void UnlockDoor(Door door)
    {
        bool hasCorrespondingKey = false;
        Key keyForDoor = null;
        foreach (Key key in keysHeld)
        {
            if (key.DoorUnlockID == door.DoorID)
            {
                hasCorrespondingKey = true;
                keyForDoor = key;
                break;
            }
        }
        if (hasCorrespondingKey)
        {
            door.Unlock();
            keysHeld.Remove(keyForDoor);
            Destroy(keyForDoor.gameObject);
        }
    }

    public void SaveGame()
    {
        SaveState saveState;
        saveState.player = player.GetPlayerData();
        saveState.enemies = new List<EnemyState>();
        foreach (Enemy enemy in enemies)
        {
            saveState.enemies.Add(enemy.GetEnemyData());
        }
        saveState.pushables = new List<PushableState>();
        foreach (PushableObject pushable in pushableObjects)
        {
            saveState.pushables.Add(pushable.GetPushableData());
        }

        saveState.keysHeld = new List<Key>();
        foreach (Key key in keysHeld)
        {
            saveState.keysHeld.Add(key);
        }

        saveState.doors = new List<bool>();
        foreach(Door door in doors)
        {
            saveState.doors.Add(door.Unlocked);
        }
        saveState.keys = new List<KeyState>();
        foreach(Key key in keys)
        {
            saveState.keys.Add(key.GetState());
        }

        bool isNewState = false;
        if (previousStates.Count == 0)
        {
            isNewState = true;
        }
        else if (previousStates[previousStates.Count-1].player.position != player.transform.position)
        {
            isNewState = true;
        }
        if (isNewState)
        {
            if (previousStates.Count == maxStepsSaved)
            {
                previousStates.RemoveAt(0);
            }
            previousStates.Add(saveState);
        }
    }
    public void LoadPreviousState()
    {
        if(previousStates.Count >= 1)
        {
            SaveState previousState = previousStates[previousStates.Count-1];
            player.LoadState(previousState.player);
            for (int i = 0; i < previousState.pushables.Count; i++)
            {
                pushableObjects[i].LoadState(previousState.pushables[i]);
            }

            for (int i = 0; i < previousState.enemies.Count; i++)
            {
                enemies[i].LoadState(previousState.enemies[i]);
            }

            for (int i = 0; i < previousState.doors.Count; i++)
            {
                doors[i].LoadState(previousState.doors[i]);   
            }

            for (int i = 0; i < previousState.keys.Count; i++)
            {
                keys[i].LoadState(previousState.keys[i]);
            }
            keysHeld = previousState.keysHeld;
            previousStates.RemoveAt(previousStates.Count-1);
        }
    }
}
