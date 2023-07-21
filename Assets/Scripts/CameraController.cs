using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform targetCharacter; // El personaje que la cámara debe seguir en el eje Y

    private CinemachineVirtualCamera virtualCamera; // Referencia a la cámara virtual de Cinemachine

    private void Start()
    {
        // Obtener la referencia a la cámara virtual de Cinemachine
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera == null)
        {
            Debug.LogError("No se encontró el componente CinemachineVirtualCamera en la cámara.");
            return;
        }
    }

    private void Update()
    {
        if (targetCharacter != null && virtualCamera != null)
        {
            // Mantener la posición de la cámara solo en el eje Y
            Vector3 desiredPosition = new Vector3(virtualCamera.transform.position.x, targetCharacter.position.y, virtualCamera.transform.position.z);

            // Asignar la nueva posición de la cámara para seguir al personaje en el eje Y
            virtualCamera.transform.position = desiredPosition;
        }
    }
}
