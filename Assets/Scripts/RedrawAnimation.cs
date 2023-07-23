using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RedrawAnimation : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private float redrawInterval = 1f; // Intervalo de tiempo entre cada cambio de sprite

    private SpriteRenderer spriteRenderer;
    private Image image;
    private float timer;
    private int currentSpriteIndex;

    public List<Sprite> Sprites { get => sprites; set => sprites = value; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();

        if(!(spriteRenderer || image))
        {
            Debug.LogError(this.gameObject.name + " needs either a SpriteRenderer or a Image component");
        }

        if (sprites.Count > 0)
        {
            SetSprite(sprites[0]);
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
                SetSprite(sprites[currentSpriteIndex]);
            }
        }
    }

    private void SetSprite(Sprite sprite)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }
        else if (image != null)
        {
            image.sprite = sprite;
        }
    }
}