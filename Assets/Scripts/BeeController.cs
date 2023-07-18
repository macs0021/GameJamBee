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
    public TreeController tree;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moverHaciaArriba = true;
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            moverHaciaArriba = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            tree.velocidadRotacion = -Mathf.Abs(tree.velocidadRotacion);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            tree.velocidadRotacion = Mathf.Abs(tree.velocidadRotacion);
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
        if (other.CompareTag("Flower") && !other.GetComponent<Flower>().paired)
        {
            if (seedsColor == Color.white)
            {
                seedsColor = other.GetComponent<Flower>().flowerColor;
                GetComponent<SpriteRenderer>().color = seedsColor;
                collectedFlower = other.gameObject;
            }
            if (seedsColor == other.GetComponent<SpriteRenderer>().color && other.gameObject!=collectedFlower)
            {
                collectedFlower.GetComponent<Flower>().paired = true;
                other.GetComponent<Flower>().paired = true;
                collectedFlower = null;
                seedsColor = Color.white;
                GetComponent<SpriteRenderer>().color = seedsColor;
            }
        }
    }
}
