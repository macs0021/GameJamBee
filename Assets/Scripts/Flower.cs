using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower: MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FlowerListSO flowerList;

    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer flowerSprite;
    [SerializeField] private SpriteRenderer stemSprite;

    private bool isPaired = false;

    private void Awake()
    {
        FlowerVisual flowerVisual = flowerList.GetRandomFlowerVisual();

        flowerSprite.sprite = flowerVisual.flowerSprite;
        stemSprite.sprite = flowerVisual.stemSprite;

        flowerSprite.transform.localPosition = flowerVisual.flowerPosition;
    }

    public Color GetColor()
    {
        return flowerSprite.color;
    }

    public void SetColor(Color color)
    {
        flowerSprite.color = color;
    }

    public bool IsPaired { get => isPaired; set => isPaired = value; }
}
