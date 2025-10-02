using System;
using UnityEngine;

public class CollisionMasks : MonoBehaviour
{
    private Int32 player_bitmask;
    private Int32 world_bitmask;

    void Awake()
    {

        player_bitmask = 1 << LayerMask.NameToLayer("Player");
        world_bitmask = 1 << LayerMask.NameToLayer("Environment");
    }

    public Int32 GetPlayerMask => player_bitmask;
    public Int32 GetWorldMask => world_bitmask;
    public Int32 GetPlayerAndWorldMask => GetPlayerMask | GetWorldMask;
}

