using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{
    public GameObject flowerObj;

    public void ChangeFlowerType(Color newColor)
    {
        flowerObj.gameObject.SetActive(true);

        Flower flower = flowerObj.GetComponent<Flower>();
        flower.SetColor(newColor);
    }
}
