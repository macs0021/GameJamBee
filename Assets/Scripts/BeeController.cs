using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeeController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float smoothMovement;
    [SerializeField] private float maxRotateAngle = 45f;
    private Vector2 velocity;
    private float velXSmoothing, velYSmoothing;
    private bool canFlipMovement;

    [Header("VFX")]
    [SerializeField] private float minWaitTimeToBlink = 1.0f;
    [SerializeField] private float maxWaitTimeToBlink = 5.0f;
    [SerializeField] private Transform eyesTransform;
    [SerializeField] private ParticleSystem droppingPolen;
    [SerializeField] private GameObject trailObject;

    private Tweener beeTween;
    private Tweener boingTween;
    private Tweener rotateBeeTween;
    [SerializeField] private Transform visualWrapperTransform;
    [SerializeField] private Transform visualTransform;

    private bool isTweening;
    [SerializeField] private Transform leftWingTransform;
    [SerializeField] private Transform rightWingTransform;

    [Header("Misc")]
    [SerializeField] private SpriteRenderer bellySprite;
    private Controller3D controller;
    private Flower collectedFlower;

    private void Awake()
    {
        canFlipMovement = true;
        controller = GetComponent<Controller3D>();
        StartWingAnimation();
        StartWinkAnimationLoop();
        droppingPolen.Stop();
    }

    private void Update()
    {
        ProcessInput();
        Vector3 newVelocity = controller.Move(velocity);

        HandleBoingAnimation();
    }

    private void ProcessInput()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        RotateBee(input.y);

        if (canFlipMovement)
        {
            if (input.x < 0.0f)
            {
                FlipBee(-1);
            }
            else if (input.x > 0.0f)
            {
                FlipBee(1);
            }
        }

        velocity.x = Mathf.SmoothDamp(velocity.x, input.x, ref velXSmoothing, smoothMovement);
        velocity.y = Mathf.SmoothDamp(velocity.y, input.y, ref velYSmoothing, smoothMovement);
    }

    private void FlipBee(float scaleX)
    {
        if (beeTween == null)
        {
            canFlipMovement = false;
            beeTween = visualWrapperTransform.DOScaleX(scaleX, 0.5f).SetEase(Ease.InOutSine)
                .OnKill(() => { canFlipMovement = true; beeTween = null; });
        }
    }

    private void RotateBee(float input)
    {
        if (rotateBeeTween != null && rotateBeeTween.IsActive())
        {
            rotateBeeTween.Kill();
        }

        rotateBeeTween = visualTransform
            .DOLocalRotate(new Vector3(0f, 0f, input * maxRotateAngle), 0.5f);
    }

    private void HandleBoingAnimation()
    {
        bool hasCollisions = (controller.collisions.left || controller.collisions.right || controller.collisions.up || controller.collisions.down);

        if (hasCollisions && boingTween == null)
        {
            // boing boing en x
            boingTween = visualTransform.DOScale(0.9f, 0.2f)
            .SetEase(Ease.OutBack)
            .SetLoops(2, LoopType.Yoyo);
        }

        if (!hasCollisions && boingTween != null && !boingTween.active)
        {
            boingTween = null;
        }
    }

    private void StartWingAnimation()
    {
        if (!isTweening)
        {
            isTweening = true;

            leftWingTransform.DOScaleY(-1, 0.3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo)
            .OnKill(() => leftWingTransform.DOScaleY(1, 0.1f).SetEase(Ease.Linear)
            .OnComplete(() => isTweening = false));

            rightWingTransform.DOScaleY(-1, 0.3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo)
                .OnKill(() => rightWingTransform.DOScaleY(1, 0.1f).SetEase(Ease.Linear)
                .OnComplete(() => isTweening = false));
        }
    }

    void StartWinkAnimationLoop()
    {
        float randomWaitTime = UnityEngine.Random.Range(minWaitTimeToBlink, maxWaitTimeToBlink);
        eyesTransform.DOScaleY(0.1f, 0.2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(randomWaitTime, StartWinkAnimationLoop);
            });
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
                collectedFlower = flower;
                bellySprite.enabled = true;
                bellySprite.color = flower.GetColor();
                droppingPolen.Play();
                var mainModule = droppingPolen.main;
                mainModule.startColor = new ParticleSystem.MinMaxGradient(flower.GetColor());
                //droppingPolen.Emit(1);
            }
            // Remove belly sprite
            if (bellySprite.color == flower.GetColor() && flower.gameObject != collectedFlower.gameObject)
            {
                collectedFlower.IsPaired = true;
                flower.IsPaired = true;
                collectedFlower.pairedAnimation();
                flower.pairedAnimation();
                collectedFlower = null;
                droppingPolen.Stop();

                bellySprite.enabled = false;
            }
        }
    }
}
