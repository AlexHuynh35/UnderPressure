using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class HitboxManager : MonoBehaviour
{
    private GameObject owner;
    private List<Effect> effects;
    private HitboxShape shape;
    private HitboxMovement movement;
    private float lifetime;
    private bool targetSelf;
    private bool destroyOnHit;

    public void Initialize(GameObject owner, List<Effect> effects, HitboxShape shape, HitboxMovement movement, float lifetime, bool targetSelf, bool destroyOnHit)
    {
        this.owner = owner;
        this.effects = effects;
        this.shape = shape;
        this.movement = movement;
        this.lifetime = lifetime;
        this.targetSelf = targetSelf;
        this.destroyOnHit = destroyOnHit;
    }

    void Start()
    {
        shape.SetShape(GetComponent<Collider2D>(), GetComponentInChildren<SpriteRenderer>());
    }

    void Update()
    {
            lifetime -= Time.deltaTime;
            if (lifetime <= 0)
            {
                foreach (var effect in effects)
                {
                    effect.OnExpire(gameObject);
                }
                DestroyHitbox();
                return;
            }

            movement?.Move(this, Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EntityManager target = other.GetComponent<EntityManager>();
        if (target == null || (!targetSelf && target.gameObject == owner) || !shape.IsWithin(transform.position, other.transform.position)) return;

        foreach (var effect in effects)
        {
            if (effect.CanApplyTo(target))
            {
                effect.OnEnter(target);
            }
        }

        if (destroyOnHit && effects.Select(effect => effect.CanApplyTo(target)).Any(x => x))
        {
            DestroyHitbox();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        EntityManager target = other.GetComponent<EntityManager>();
        if (target == null || (!targetSelf && target.gameObject == owner) || !shape.IsWithin(transform.position, other.transform.position)) return;

        foreach (var effect in effects)
        {
            if (effect.CanApplyTo(target))
            {
                effect.OnLeave(target);
            }
        }
    }

    public void DestroyHitbox()
    {
        Destroy(gameObject);
    }
}
