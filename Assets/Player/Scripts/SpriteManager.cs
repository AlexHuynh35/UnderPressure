using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] private StatusDatabase statusDB;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }

    public void ApplyColors(StatusTag effect)
    {
        Color color = statusDB.GetColor(effect);
        spriteRenderer.color = color;
    }
}
