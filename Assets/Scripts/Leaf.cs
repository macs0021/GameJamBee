using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    public float speed = 25f;  // puedes ajustar la velocidad aqu�
    private float lifeTime = 40f;  // el tiempo despu�s del cual se destruir� la hoja

    // Start is called before the first frame update
    void Start()
    {
        // Destruir la hoja despu�s de un cierto tiempo
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        // Mover la hoja hacia abajo a la velocidad especificada
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
