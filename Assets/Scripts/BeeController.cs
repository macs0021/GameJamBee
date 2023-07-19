using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeeController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float horizontalSpeed; // rotation
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float smoothMovement;
    private Rigidbody rb;
    private Vector2 velocity;
    private float velXSmoothing, velYSmoothing;

    private bool canFlipMovement;

    [Header("VFX")]
    [SerializeField] private float minWaitTimeToBlink = 1.0f;
    [SerializeField] private float maxWaitTimeToBlink = 5.0f;
    [SerializeField] private Transform eyesTransform;
    private Tweener blinkTween;

    private Tweener beeTween;
    [SerializeField] private Transform beeTransform;

    private bool isTweening;
    private Tweener leftWingTween;
    private Tweener rightWingTween;
    [SerializeField] private Transform leftWingTransform;
    [SerializeField] private Transform rightWingTransform;

    [Header("Misc")]
    [SerializeField] private SpriteRenderer bellySprite;
    [SerializeField] private TreeController tree;
    private Color seedsColor = Color.white;
    private Flower collectedFlower;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        canFlipMovement = true;
        StartWinkAnimationLoop();
    }

    private void Update()
    {
        ProcessInput();
        Move();
    }

    private void ProcessInput()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (canFlipMovement)
        {
            if (input.x < 0.0f)
            {
                //tree.IsBackwardsRotation = true;
                FlipBee(-1);
            }
            else if (input.x > 0.0f)
            {
                //tree.IsBackwardsRotation = false;
                FlipBee(1);
            }
        }

        if (input.x != 0.0f || input.y != 0.0f)
        {
            StartWingAnimation();
        }
        else
        {
            StopWingAnimation();
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, input.x, ref velXSmoothing, smoothMovement);
        velocity.y = Mathf.SmoothDamp(velocity.y, input.y, ref velYSmoothing, smoothMovement);
    }

    private void FlipBee(float scaleX)
    {
        if (beeTween == null)
        {
            canFlipMovement = false;
            beeTween = beeTransform.DOScaleX(scaleX, 0.5f).SetEase(Ease.InOutSine)
                .OnKill(() => { canFlipMovement = true; beeTween = null; });
        }
    }

    private void StartWingAnimation()
    {
        if (!isTweening)
        {
            isTweening = true;

            leftWingTween = leftWingTransform.DOScaleY(-1, 0.3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo)
            .OnKill(() => leftWingTransform.DOScaleY(1, 0.1f).SetEase(Ease.Linear)
            .OnComplete(() => isTweening = false));

            rightWingTween = rightWingTransform.DOScaleY(-1, 0.3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo)
                .OnKill(() => rightWingTransform.DOScaleY(1, 0.1f).SetEase(Ease.Linear)
                .OnComplete(() => isTweening = false));
        }
    }

    private void StopWingAnimation()
    {
        if (isTweening)
        {
            leftWingTween.Kill(true);
            rightWingTween.Kill(true);
        }
    }

    void StartWinkAnimationLoop()
    {
        float randomWaitTime = UnityEngine.Random.Range(minWaitTimeToBlink, maxWaitTimeToBlink);
        blinkTween = eyesTransform.DOScaleY(0.1f, 0.2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(randomWaitTime, StartWinkAnimationLoop);
            });
    }

    private void Move()
    {
        transform.Translate(new Vector3(0, velocity.y * verticalSpeed * Time.deltaTime, 0));

        // Rotar el arbol en el eje Y
        tree.transform.Rotate(Vector3.up, velocity.x * horizontalSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flower") &&
            other.TryGetComponent(out Flower flower) && 
            !flower.IsPaired)
        {
            // Picked up pollen
            if (!bellySprite.enabled)
            {
                seedsColor = flower.GetColor();
                collectedFlower = flower;

                bellySprite.enabled = true;
                bellySprite.color = seedsColor;
            }
            // Remove belly sprite
            if (seedsColor == flower.GetColor() && flower.gameObject != collectedFlower.gameObject)
            {
                collectedFlower.IsPaired = true;
                flower.IsPaired = true;
                collectedFlower = null;

                bellySprite.enabled = false;
            }
        }
    }
}
