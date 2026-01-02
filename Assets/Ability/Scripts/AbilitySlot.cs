using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class AbilitySlot
{
    public KeyCode inputKey;
    public List<Ability> abilities;
    public float cooldownTime;
}
