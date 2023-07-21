using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform targetCharacter; // El personaje que la c�mara debe seguir en el eje Y

    private CinemachineVirtualCamera virtualCamera; // Referencia a la c�mara virtual de Cinemachine

    private void Start()
    {
        // Obtener la referencia a la c�mara virtual de Cinemachine
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera == null)
        {
            Debug.LogError("No se encontr� el componente CinemachineVirtualCamera en la c�mara.");
            return;
        }
    }

    private void Update()
    {
        if (targetCharacter != null && virtualCamera != null)
        {
            // Mantener la posici�n de la c�mara solo en el eje Y
            Vector3 desiredPosition = new Vector3(virtualCamera.transform.position.x, targetCharacter.position.y, virtualCamera.transform.position.z);

            // Asignar la nueva posici�n de la c�mara para seguir al personaje en el eje Y
            virtualCamera.transform.position = desiredPosition;
        }
    }
}
