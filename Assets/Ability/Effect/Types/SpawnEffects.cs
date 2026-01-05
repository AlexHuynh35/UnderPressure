using UnityEngine;
using System.Collections.Generic;

public class SpawnData
{
    public List<Effect> effects;
    public GameObject hitboxPrefab;
    public HitboxShape shape;
    public HitboxMovement movement;
    public float lifetime;
    public bool targetSelf;
    public bool destroyOnHit;

    public SpawnData(List<Effect> effects, GameObject hitboxPrefab, HitboxShape shape, HitboxMovement movement, float lifetime, bool targetSelf, bool destroyOnHit)
    {
        this.effects = effects;
        this.hitboxPrefab = hitboxPrefab;
        this.shape = shape;
        this.movement = movement;
        this.lifetime = lifetime;
        this.targetSelf = targetSelf;
        this.destroyOnHit = destroyOnHit;
    }
}

public class SpawnOnHitEffect : Effect
{
    public SpawnData data;

    public SpawnOnHitEffect(SpawnData data, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.data = data;
    }

    public override void OnEnter(EntityManager target)
    {
        if (data.movement == null)
        {
            data.movement = new FollowMovement(target, Vector2.zero);
        }

        HitboxManager attack = Object.Instantiate(data.hitboxPrefab, target.transform.position, Quaternion.identity).GetComponent<HitboxManager>();
        attack.Initialize
        (
            owner: source.gameObject,
            effects: data.effects,
            shape: data.shape,
            movement: data.movement,
            lifetime: data.lifetime,
            targetSelf: data.targetSelf,
            destroyOnHit: data.destroyOnHit
        );
    }
}

public class SpawnOnExpireEffect : Effect
{
    public SpawnData data;

    public SpawnOnExpireEffect(SpawnData data, EntityManager source, EntityTag allowedTags) : base(source, allowedTags)
    {
        this.data = data;
    }

    public override void OnExpire(GameObject hitbox)
    {
        if (data.movement == null)
        {
            data.movement = new StationaryMovement();
        }

        HitboxManager attack = Object.Instantiate(data.hitboxPrefab, hitbox.transform.position, Quaternion.identity).GetComponent<HitboxManager>();
        attack.Initialize
        (
            owner: source.gameObject,
            effects: data.effects,
            shape: data.shape,
            movement: data.movement,
            lifetime: data.lifetime,
            targetSelf: data.targetSelf,
            destroyOnHit: data.destroyOnHit
        );
    }
}
