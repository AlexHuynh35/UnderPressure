using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityGroup", menuName = "AbilityGroup")]
public class AbilityGroup : ScriptableObject
{
    public new string name;
    public List<Ability> abilities;
    public float cooldownTime;
}
