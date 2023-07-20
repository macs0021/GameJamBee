using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RedrawAnimation : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private float redrawInterval = 1f; // Intervalo de tiempo entre cada cambio de sprite

    private SpriteRenderer spriteRenderer;
    private float timer;
    private int currentSpriteIndex;

    public List<Sprite> Sprites { get => sprites; set => sprites = value; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (sprites.Count > 0)
        {
            spriteRenderer.sprite = sprites[0];
        }
    }

    private void Update()
    {
        if (sprites.Count > 1)
        {
            timer += Time.deltaTime;
            if (timer >= redrawInterval)
            {
                timer -= redrawInterval;
                currentSpriteIndex = (currentSpriteIndex + 1) % sprites.Count;
                spriteRenderer.sprite = sprites[currentSpriteIndex];
            }
        }
    }
}
