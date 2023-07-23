using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public float radius = 5f;
    public int numObjects = 10;
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numObjects; i++)
        {
            float radiusRandom = Random.Range(0, radius); // generamos un radio aleatorio
            float angle = Random.Range(0, 2 * Mathf.PI); // generamos un ángulo aleatorio

            Vector3 newPos = new Vector3(radiusRandom * Mathf.Cos(angle), -3.32f, radiusRandom * Mathf.Sin(angle));
            Instantiate(prefab, newPos, Quaternion.Euler(0, -angle * Mathf.Rad2Deg, 0));
        }
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
