using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower: MonoBehaviour
{
    [SerializeField] private SpriteRenderer flowerSprite;
    private Color flowerColor;
    private bool isPaired = false;

    public Color FlowerColor { get => flowerColor; set => flowerColor = value; }
    public bool IsPaired { get => isPaired; set => isPaired = value; }
    public SpriteRenderer FlowerSprite { get => flowerSprite; set => flowerSprite = value; }
}
