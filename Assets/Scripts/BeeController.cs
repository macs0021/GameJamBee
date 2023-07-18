using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeeController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float smoothMovement;
    private Rigidbody rb;
    private float verticalVelocity; // this is not Vector2 bcs only moves upwards
    private float velYSmoothing;

    private bool canFlipMovement;

    [Header("VFX")]
    private Tweener beeTween;

    [SerializeField] private Transform beeTransform;

    private bool isTweening;
    private Tweener leftWingTween;
    private Tweener rightWingTween;

    [SerializeField] private Transform leftWingTransform;
    [SerializeField] private Transform rightWingTransform;

    [Header("Misc")]
    public Color seedsColor = Color.white;
    public GameObject collectedFlower;
    public TreeController tree;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        canFlipMovement = true;
    }

    private void Update()
    {
        ProcessInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ProcessInput()
    {
        float verticalInput = Input.GetAxisRaw("Vertical") <= 0.0f ? -1 : 1;
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        if (canFlipMovement)
        {
            if (horizontalInput < 0.0f)
            {
                tree.IsBackwardsRotation = true;
                FlipBee(-1);
            }
            else if (horizontalInput > 0.0f)
            {
                tree.IsBackwardsRotation = false;
                FlipBee(1);
            }
        }

        if (verticalInput >= 0.0f)
        {
            StartWingAnimation();
        }
        else
        {
            StopWingAnimation();
        }

        verticalVelocity = Mathf.SmoothDamp(verticalVelocity, verticalInput, ref velYSmoothing, smoothMovement);
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
            .OnKill(() => leftWingTransform.DOScaleY(1, 0.1f).SetEase(Ease.Linear));

            rightWingTween = rightWingTransform.DOScaleY(-1, 0.3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo)
                .OnKill(() => rightWingTransform.DOScaleY(1, 0.1f).SetEase(Ease.Linear));
        }
    }

    private void StopWingAnimation()
    {
        if (isTweening)
        {
            leftWingTween.Kill(true);
            rightWingTween.Kill(true);

            isTweening = false;
        }
    }

    private void Move()
    {
        rb.velocity = new Vector3(0, verticalVelocity * moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Hay que optimizar esto
        if (other.CompareTag("Flower") && !other.GetComponent<Flower>().paired)
        {
            if (seedsColor == Color.white)
            {
                seedsColor = other.GetComponent<Flower>().flowerColor;
                //GetComponent<SpriteRenderer>().color = seedsColor;
                collectedFlower = other.gameObject;
            }
            if (seedsColor == other.GetComponent<SpriteRenderer>().color && other.gameObject != collectedFlower)
            {
                collectedFlower.GetComponent<Flower>().paired = true;
                other.GetComponent<Flower>().paired = true;
                collectedFlower = null;
                seedsColor = Color.white;
                //GetComponent<SpriteRenderer>().color = seedsColor;
            }
        }
    }
}
