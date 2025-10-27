using UnityEngine;

[System.Flags]
public enum EntityTag
{
    None = 0,
    Player = 1 << 0,
    Breakable = 1 << 1,
    Unbreakable = 1 << 2,
    Enemy = 1 << 3,
    Soldier = 1 << 4,
    Boss = 1 << 5,
    Hitbox = 1 << 6,
    SoftHitbox = 1 << 7,
    HardHitbox = 1 << 8,
    PierceHitbox = 1 << 9,
}
