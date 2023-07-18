using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    // Start is called before the first frame update
    public float velocidad = 5f;
    private bool moverHaciaArriba = false;
    public Color seedsColor = Color.white;
    public GameObject collectedFlower;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            moverHaciaArriba = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            moverHaciaArriba = false;
        }

        if (moverHaciaArriba)
        {
            transform.Translate(Vector3.up * velocidad * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.down * velocidad * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Hay que optimizar esto
        if (other.CompareTag("Flower"))
        {
            if (seedsColor == Color.white)
            {
                seedsColor = other.GetComponent<Flower>().flowerColor;
                GetComponent<SpriteRenderer>().color = seedsColor;
            }
        }
    }
}
