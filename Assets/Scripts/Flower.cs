using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public Color flowerColor;

    private void Update()
    {
        // Obtener la dirección hacia la cámara sin tener en cuenta la rotación en los ejes Y y Z
        Vector3 lookDirection = Camera.main.transform.position - transform.position;
        lookDirection.y = 0f;
        lookDirection.z = 0f;

        // Calcular la rotación deseada hacia la cámara en el eje X
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        // Aplicar la rotación solo en el eje X
        transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, 0f, 0f);
    }
}
