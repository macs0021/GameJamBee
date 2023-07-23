using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassController : MonoBehaviour
{
    public List<Sprite> spriteList;
    public List<GameObject> objectList;

    void Start()
    {
        foreach (GameObject obj in objectList)
        {
            AssignRandomSprite(obj);
        }
    }

    void AssignRandomSprite(GameObject obj)
    {
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();

        if (renderer != null && spriteList.Count > 0)
        {
            int randomIndex = Random.Range(0, spriteList.Count);
            renderer.sprite = spriteList[randomIndex];
        }
    }
}
