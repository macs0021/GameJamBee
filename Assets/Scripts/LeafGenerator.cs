using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafGenerator : MonoBehaviour
{
    public GameObject Leaf;

    void Start()
    {
        StartCoroutine(SpawnLeaf());
    }

    IEnumerator SpawnLeaf()
    {
        while (true)
        {
            // Realiza el spawn del objeto Leaf
            GameObject leaf = Instantiate(Leaf, transform.position, Quaternion.identity);
            leaf.transform.parent = this.gameObject.transform;

            // Espera 5 segundos antes de la siguiente iteración
            yield return new WaitForSeconds(20f);
        }
    }

    void Update()
    {
        // Aquí puede ir cualquier otro código que quieras ejecutar cada frame
    }
}
