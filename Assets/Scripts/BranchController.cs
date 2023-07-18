using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{
    public GameObject flower;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ChangeFlowerType(Color newColor)
    {
       
        flower.gameObject.SetActive(true);
        SpriteRenderer spriteRenderer = flower.GetComponent<SpriteRenderer>();
        flower.GetComponent<Flower>().flowerColor = newColor;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = newColor;
            return true;
        }
        else
        {
            Debug.LogWarning("GameObject " + flower.name + " no tiene un componente SpriteRenderer.");
            return false;
        }
    }
}
