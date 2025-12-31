using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    public int maxUses;
    public float parryTime;

    private int remainingUses;
    private bool active;
    private bool parryMode;
    private float parryTimer;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        remainingUses = maxUses;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Deactivate();
    }

    void Update()
    {
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
        }
        else
        {
            var hitbox = target.GetComponent<HitboxManager>();
            var hitboxMovement = hitbox.GetMovement();
            if (hitboxMovement is StraightMovement)
            {
                Destroy(target.gameObject);
                Debug.Log("Reflect");
            }
            else
            {
                Destroy(target.gameObject);
                Debug.Log("Stun");
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
