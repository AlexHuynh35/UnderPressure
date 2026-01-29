using UnityEngine;
using System.Collections.Generic;

public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private float contactDamage;
    private EntityManager enemy;
    private List<Effect> effects;

    void Start ()
    {
        enemy = GetComponentInParent<EntityManager>();
        effects = new List<Effect>()
        {
            new DamageEffect(damage: contactDamage, piercing: false, source: enemy, allowedTags: EntityTag.Player),
        };
    }

    void Update ()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        EntityManager target = other.GetComponent<EntityManager>();
        if (target == null || target.tags != EntityTag.Player) return;

        foreach (var effect in effects)
        {
            if (effect.CanApplyTo(target))
            {
                effect.OnEnter(target);
            }
        }
    }
}
