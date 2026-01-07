using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StatusDatabase", menuName = "StatusDatabase")]
public class StatusDatabase : ScriptableObject
{
    [System.Serializable]
    public struct StatusEntry
    {
        public StatusTag effect;
        public Color color;
    }

    [SerializeField] private List<StatusEntry> entries;

    private Dictionary<StatusTag, Color> lookup;

    private void OnEnable()
    {
        lookup = new Dictionary<StatusTag, Color>();
        foreach (var e in entries)
        {
            lookup[e.effect] = e.color;
        }
    }

    public Color GetColor(StatusTag effect)
    {
        return lookup.TryGetValue(effect, out var color) ? color : Color.white;
    }
}
