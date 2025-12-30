using UnityEngine;

public class ShieldManager : MonoBehaviour
{
    public int maxUses;
    private int remainingUses;
    private bool active;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        remainingUses = maxUses;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Deactivate();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EntityManager target = other.GetComponent<EntityManager>();
        if (!active || target == null || target.tags != EntityTag.Hitbox) return;

        Destroy(target.gameObject);
        remainingUses--;
    }

    public void Activate()
    {
        if (remainingUses > 0) 
        {
            active = true;
            spriteRenderer.gameObject.SetActive(true);
        }
    }

    public void Deactivate()
    {
        active = false;
        spriteRenderer.gameObject.SetActive(false);
    }
}
