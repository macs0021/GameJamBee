using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RaycastController : MonoBehaviour
{
    protected const float skinWidth = 0.02f;

    [Header("Collisions Stats")]
    [SerializeField] protected LayerMask collisionMask;
    protected BoxCollider col;

    [Header("Raycasts Stats")]
    [SerializeField] protected int horizontalRayCount;
    [SerializeField] protected int verticalRayCount;

    protected float horizontalRaySpacing;
    protected float verticalRaySpacing;

    private Quaternion initialRotation;

    protected RaycastOrigins raycastOrigins;

    protected virtual void Start()
    {
        // Avoid wiggling
        Physics.autoSyncTransforms = true;
        col = GetComponent<BoxCollider>();
        CalculateRaySpacing();
        initialRotation = transform.rotation;
    }

    protected void UpdateRaycastOrigins()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(-2 * skinWidth);

        // Calcula los puntos de los centros de las caras
        Vector3 center = bounds.center;
        Vector3 extents = bounds.extents;

        Vector3 localRight = transform.TransformDirection(Vector3.right);
        Vector3 localUp = transform.TransformDirection(Vector3.up);

        // Puntos para la cara TOP
        Vector3 topCenter = center + localUp * extents.y;
        raycastOrigins.topCenterLeft = (topCenter - localRight * extents.x);
        raycastOrigins.topCenterRight = (topCenter + localRight * extents.x);

        // Puntos para la cara RIGHT
        Vector3 rightCenter = center + localRight * extents.x;
        raycastOrigins.rightCenterUp = (rightCenter + localUp * extents.y);
        raycastOrigins.rightCenterDown = (rightCenter - localUp * extents.y);

        // Puntos para la cara BOTTOM
        Vector3 bottomCenter = center - localUp * extents.y;
        raycastOrigins.bottomCenterLeft = (bottomCenter - localRight * extents.x);
        raycastOrigins.bottomCenterRight = (bottomCenter + localRight * extents.x);

        // Puntos para la cara LEFT
        Vector3 leftCenter = center - localRight * extents.x;
        raycastOrigins.leftCenterUp = (leftCenter + localUp * extents.y);
        raycastOrigins.leftCenterDown = (leftCenter - localUp * extents.y);
    }

    private void CalculateRaySpacing()
    {
        Bounds bounds = col.bounds;
        bounds.Expand(-2 * skinWidth);

        // Make sure there are at least 2 rays (1 on each corner of the bound)
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    protected struct RaycastOrigins
    {
        public Vector3 topCenterLeft, topCenterRight;
        public Vector3 rightCenterUp, rightCenterDown;
        public Vector3 bottomCenterLeft, bottomCenterRight;
        public Vector3 leftCenterUp, leftCenterDown;
    }
}
