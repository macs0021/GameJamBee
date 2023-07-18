using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    public float speed = 25f;  // puedes ajustar la velocidad aquí
    private float lifeTime = 40f;  // el tiempo después del cual se destruirá la hoja

    // Start is called before the first frame update
    void Start()
    {
        // Destruir la hoja después de un cierto tiempo
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        // Mover la hoja hacia abajo a la velocidad especificada
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
