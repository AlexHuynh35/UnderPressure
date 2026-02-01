using UnityEngine;
using System.Collections.Generic;

public class ShieldManager : MonoBehaviour
{
    public int maxUses;
    public float parryTime;
    public float magnitude;
    public float offset;
    public float radius;
    public GameObject hitboxPrefab;

    private int remainingUses;
    private bool active;
    private bool parryMode;
    private float parryTimer;
    private EntityManager entityManager;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        remainingUses = maxUses;
        entityManager = transform.parent.GetComponentInParent<EntityManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Deactivate();
    }

    void Update()
    {
        transform.position = entityManager.transform.position + (Vector3)entityManager.orientation * offset;

        if (parryMode)
        {
            parryTimer -= Time.deltaTime;

            if (parryTimer < 0)
            {
                parryMode = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EntityManager target = other.GetComponent<EntityManager>();
        if (!active || target == null || target.tags != EntityTag.HardHitbox) return;

        if (!parryMode)
        {
            Destroy(target.gameObject);
            remainingUses--;
            if (remainingUses <= 0) Deactivate();
        }
        else
        {
            Effect parryEffect = new ParryStatusEffect(rate: 1f, duration: magnitude, source: entityManager, allowedTags: EntityTag.Player);
            parryEffect.OnEnter(entityManager);

            var hitbox = target.GetComponent<HitboxManager>();
            var hitboxMovement = hitbox.GetMovement();
            if (hitboxMovement is StraightMovement)
            {
                hitbox.SwapTarget(entityManager, magnitude);
            }
            else
            {
                List<Effect> effects = new List<Effect>()
                {
                    new StunStatusEffect(rate: 1f, duration: magnitude, source: entityManager, allowedTags: EntityTag.Enemy)
                };
                HitboxShape shape = new CircleShape(radius: radius);
                HitboxMovement movement = new FollowMovement(following: entityManager, offset: Vector3.zero);
                HitboxManager attack = Instantiate(hitboxPrefab, entityManager.transform.position, Quaternion.identity).GetComponent<HitboxManager>();
                attack.Initialize
                (
                    owner: entityManager.gameObject,
                    effects: effects,
                    shape: shape,
                    movement: movement,
                    lifetime: 0.25f,
                    targetSelf: false,
                    destroyOnHit: false
                );

                Destroy(target.gameObject);
            }
        }
    }

    public void Activate()
    {
        if (remainingUses > 0)
        {
            active = true;
            parryMode = true;
            parryTimer = parryTime;
            spriteRenderer.gameObject.SetActive(true);
        }
    }

    public void Deactivate()
    {
        active = false;
        parryMode = false;
        spriteRenderer.gameObject.SetActive(false);
    }
}
