using System.Collections.Generic;
using UnityEngine;

public struct SaveState
{
    public PlayerState player;
    public List<EnemyState> enemies;
    public List<PushableState> pushables;
    public List<Key> keysHeld;
    public List<bool> doors;
    public List<KeyState> keys;
}

public struct PlayerState
{
    public Vector3 position;

    //animator parameters
    public float MoveX;
    public float MoveY;
}

public struct PushableState
{
    public Vector3 position;
    public string tag;
    public Sprite sprite;
}

public struct EnemyState
{
    public bool isAlive;
}

public struct KeyState
{
    public bool collected;
    public bool used;
}
