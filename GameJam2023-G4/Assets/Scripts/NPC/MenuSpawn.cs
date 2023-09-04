using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPCs;

public class MenuSpawn : MonoBehaviour
{
    public SpawnNPC spawn;
    void Start()
    {
        spawn.Spawn(Type.Grandma, false, new Vector2Int(-4, -5), true);
        spawn.Spawn(Type.LifeGuard, false, new Vector2(10, -1), true);
        spawn.Spawn(Type.Kid, false, new Vector2(10, -3), true);
    }
}
