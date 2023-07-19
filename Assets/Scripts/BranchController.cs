using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{
    public GameObject flowerObj;

    public bool ChangeFlowerType(Color newColor)
    {
        flowerObj.gameObject.SetActive(true);

        Flower flower = flowerObj.GetComponent<Flower>();
        SpriteRenderer spriteRenderer = flower.FlowerSprite;
        flower.FlowerColor = newColor;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = newColor;
            return true;
        }
        else
        {
            Debug.LogWarning("GameObject " + flowerObj.name + " no tiene un componente SpriteRenderer.");
            return false;
        }
    }
}
