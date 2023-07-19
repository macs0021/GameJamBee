using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct FlowerVisual
{
    public Sprite flowerSprite;
    public Sprite stemSprite;

    public Vector3 flowerPosition;
}

[CreateAssetMenu(fileName = "Flower List", menuName = "Flower List")]
public class FlowerListSO : ScriptableObject
{
    [SerializeField] private List<FlowerVisual> flowerVisuals;

    public FlowerVisual GetRandomFlowerVisual()
    {
        return flowerVisuals[UnityEngine.Random.Range(0, flowerVisuals.Count)];
    }
}
