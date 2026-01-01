using UnityEngine;
using System.Collections.Generic;

public static class PlayerData
{
    private static EntityManager player;
    public static EntityManager Player => player;

    public static void Set(EntityManager entity)
    {
        if (player == null) player = entity;
    }

    public static void Clear(EntityManager entity)
    {
        if (player == entity) player = null;
    }
}
