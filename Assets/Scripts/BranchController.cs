using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{
    public GameObject flowerObj;
    public GameObject[] objects;

    private void Start()
    {
        RandomizeScale();
    }
    public void RandomizeScale()
    {
        foreach (GameObject obj in objects)
        {
            float randomScale = Random.Range(0.8f, 1.2f);
            Vector3 originalScale = obj.transform.localScale;
            Vector3 newScale = originalScale * randomScale;
            obj.transform.localScale = newScale;
        }
    }

    public void ChangeFlowerType(Color newColor)
    {
        flowerObj.gameObject.SetActive(true);

        Flower flower = flowerObj.GetComponent<Flower>();
        flower.SetColor(newColor);
    }

    public bool flowerIsPaired()
    {
        return flowerObj.GetComponent<Flower>().IsPaired;
    }

    public void disableFlower()
    {
        flowerObj.SetActive(false);
    }
}
