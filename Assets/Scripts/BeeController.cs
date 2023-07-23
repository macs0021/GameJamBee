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
    [SerializeField] private Transform visualEyesTransform;

    [SerializeField] private Transform dizzyEyesTransform;
    [SerializeField] private Transform leftDizzyEyeTransform;
    [SerializeField] private Transform rightDizzyEyeTransform;

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
    private Flower collectedFlower;
    private bool canPickUpFlower;
    [SerializeField] TutorialController tutorialController;

    bool movementTutorial = false;
    bool pickupTutorial = false;
    bool collisionTutorial = false;
    bool pairedTutorial = false;

    private Controller3D controller;

    private void Awake()
    {
        canFlipMovement = true;
        canPickUpFlower = true;

        controller = GetComponent<Controller3D>();
        StartWingAnimation();
        StartWinkAnimationLoop();
        droppingPolen.Stop();

        leftDizzyEyeTransform.gameObject.SetActive(false);
        rightDizzyEyeTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        ProcessInput();
        controller.Move(velocity);

        HandleBoingAnimation();
    }

    private void ProcessInput()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (input != Vector2.zero && !movementTutorial)
        {
            movementTutorial = tutorialController.NextTutorial();
        }

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
        bool hasCollisions = (controller.collisions.left ||
            controller.collisions.right ||
            controller.collisions.up /* || controller.collisions.down */);

        if (hasCollisions && boingTween == null)
        {
            if (!collisionTutorial && movementTutorial && pickupTutorial)
            {
                collisionTutorial = tutorialController.NextTutorial();
            }
            // Cant pick up flower
            canPickUpFlower = false;
            float duration = 0.4f;
            boingTween = visualTransform.DOShakeScale(duration, 0.6f);

            visualEyesTransform.gameObject.SetActive(false);
            leftDizzyEyeTransform.gameObject.SetActive(true);
            rightDizzyEyeTransform.gameObject.SetActive(true);

            leftDizzyEyeTransform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360);
            rightDizzyEyeTransform.DORotate(new Vector3(0, 0, 360), duration, RotateMode.FastBeyond360);

            Sequence dizzyEyes = DOTween.Sequence();
            dizzyEyes.Append(dizzyEyesTransform.DOPunchScale(Vector3.one / 3, duration, 3, 0.4f));
            dizzyEyes.Append(dizzyEyesTransform.DOScaleY(0.1f, 0.2f).SetEase(Ease.InOutSine));
            dizzyEyes.OnComplete(() =>
            {
                dizzyEyesTransform.localScale = Vector3.one;
                leftDizzyEyeTransform.gameObject.SetActive(false);
                rightDizzyEyeTransform.gameObject.SetActive(false);

                visualEyesTransform.gameObject.SetActive(true);
                visualEyesTransform.localScale = new Vector3(1.0f, 0.1f, 1.0f);
                visualEyesTransform.DOScaleY(1.0f, 0.2f);
            });

            RemovePollen(); // here it resets
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

    private void StartWinkAnimationLoop()
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

    private void RemovePollen()
    {
        droppingPolen.Stop();

        if (collectedFlower)
        {
            collectedFlower.StopPickedAnimation();
        }

        bellySprite.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                bellySprite.enabled = false;
                canPickUpFlower = true; // reset to pick up flowers
            });
    }
    private void StartPolenParticles(Color color)
    {
        var mainModule = droppingPolen.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(color);

        droppingPolen.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Flower") &&
            other.TryGetComponent(out Flower flower) &&
            !flower.IsPaired)
        {
            // Picked up pollen
            if (!bellySprite.enabled && canPickUpFlower /* flag to avoid animation bugs */)
            {
                canPickUpFlower = false; // while we pick up pollen, cant get hurt
                if (!pickupTutorial && movementTutorial)
                {
                    pickupTutorial = tutorialController.NextTutorial();
                }
                collectedFlower = flower;
                bellySprite.enabled = true;
                bellySprite.color = flower.GetColor();
                bellySprite.transform.localScale = Vector3.zero;

                bellySprite.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutSine)
                    .OnComplete(() => canPickUpFlower = true);

                flower.StartPickedAnimation();
                StartPolenParticles(bellySprite.color);
            }
            // Remove belly sprite
            if (bellySprite.color == flower.GetColor() && flower.gameObject != collectedFlower.gameObject)
            {
                collectedFlower.IsPaired = true;
                flower.IsPaired = true;
                if (!pairedTutorial && movementTutorial && pickupTutorial && collisionTutorial)
                {
                    pairedTutorial = tutorialController.NextTutorial();
                    if (pairedTutorial)
                    {
                        Invoke("endTutorial", 5f);
                    }
                }

                collectedFlower.StartPairedAnimation();
                flower.StartPairedAnimation();

                collectedFlower = null;
                droppingPolen.Stop();

                bellySprite.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutSine)
                    .OnComplete(() => bellySprite.enabled = false);
            }
        }
    }
    private void endTutorial()
    {
        tutorialController.EndTutorial();
    }
}
