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

    public void RemoveColors(StatusTag effect)
    {
        Color color = statusDB.GetColor(effect);
        if (spriteRenderer.color == color)
        {
            Color noColor = statusDB.GetColor(StatusTag.None);
            spriteRenderer.color = noColor;
        }
    }

    public void Flash(bool final)
    {
        Color color = spriteRenderer.color;
        if (color.a != 1 || final)
        {
            color.a = 1f;
        }
        else
        {
            color.a = 0.5f;
        }
        spriteRenderer.color = color;
    }
}
