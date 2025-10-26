using UnityEngine;

[System.Flags]
public enum ObjectTag
{
    None = 0,
    Player = 1 << 0,
    Breakable = 1 << 1,
    Unbreakable = 1 << 2,
    Enemy = 1 << 3,
    Boss = 1 << 4,
    Soldier = 1 << 5,
    SoftHitbox = 1 << 6,
    HardHitbox = 1 << 7,
}
