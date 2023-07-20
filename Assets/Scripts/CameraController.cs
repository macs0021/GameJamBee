using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // Objeto a seguir
    public float distance = 5.0f; // Distancia desde el objeto
    public float height = 3.0f; // Altura de la c�mara
    public float rotationSpeed = 10.0f; // Velocidad de rotaci�n

    private void LateUpdate()
    {
        // Calcula la posici�n deseada de la c�mara en funci�n del objeto
        Vector3 desiredPosition = target.position - (Quaternion.Euler(0, transform.eulerAngles.y, 0) * Vector3.forward) * distance;
        desiredPosition.y = target.position.y + height;

        // Interpola suavemente hacia la posici�n deseada
        transform.position = Vector3.Lerp(transform.position, desiredPosition, rotationSpeed * Time.deltaTime);

        // Mira hacia el objeto
        transform.LookAt(target);
    }
}
