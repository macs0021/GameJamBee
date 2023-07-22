using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller3D : RaycastController
{
    [SerializeField] private float horizontalSpeed; // rotation
    [SerializeField] private float verticalSpeed;
    [SerializeField] private GameObject rotator;

    public CollisionInfo collisions { get => collisionInfo; set => collisionInfo = value; }
    private CollisionInfo collisionInfo;

    protected override void Start()
    {
        base.Start();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycastOrigins();
        collisionInfo.Reset();

        if (velocity.x != 0)
        {
            CalculateHorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            CalculateVerticalCollisions(ref velocity);
        }

        transform.Translate(new Vector3(0, velocity.y * verticalSpeed * Time.deltaTime, 0));
        // Rotar el arbol en el eje Y
        rotator.transform.Rotate(Vector3.up, velocity.x * horizontalSpeed * Time.deltaTime);
    }

    private void CalculateHorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        Vector3 localRight = transform.TransformDirection(Vector3.right);
        Vector3 localUp = transform.TransformDirection(Vector3.up);

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector3 rayOrigin = (directionX == -1) ? raycastOrigins.leftCenterDown : raycastOrigins.rightCenterDown;

            rayOrigin += localUp * (horizontalRaySpacing * i);

            Debug.DrawRay(rayOrigin, localRight * directionX * rayLength, Color.red);

            if (Physics.Raycast(rayOrigin, localRight * directionX, out RaycastHit hit, rayLength, collisionMask))
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;

                collisionInfo.left = directionX == -1;
                collisionInfo.right = directionX == 1;
            }
        }
    }

    private void CalculateVerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        Vector3 localRight = transform.TransformDirection(Vector3.right);
        Vector3 localUp = transform.TransformDirection(Vector3.up);

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector3 rayOrigin = (directionY == -1) ? raycastOrigins.bottomCenterLeft : raycastOrigins.topCenterLeft;

            rayOrigin += localRight * (verticalRaySpacing * i /* + velocity.x*/);

            Debug.DrawRay(rayOrigin, localUp * directionY * rayLength, Color.red);

            if (Physics.Raycast(rayOrigin, localUp * directionY, out RaycastHit hit, rayLength, collisionMask))
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisionInfo.down = directionY == -1;
                collisionInfo.up = directionY == 1;
            }
        }
    }

    public struct CollisionInfo
    {
        public bool up, down;
        public bool left, right;

        public bool climbing, descending;
        public float slopeAngle, lastFrameSlopeAngle;

        public void Reset()
        {
            up = down = left = right = climbing = descending = false;
            lastFrameSlopeAngle = slopeAngle;
            slopeAngle = 0f;
        }
    }
}
