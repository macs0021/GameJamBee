using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Flower : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FlowerListSO flowerList;
    [SerializeField] private List<Sprite> leafSprites;

    [Header("Sprite Renderers")]
    [SerializeField] private SpriteRenderer flowerSprite;
    [SerializeField] private SpriteRenderer stemSprite;

    [Header("Animation")]
    [SerializeField] private float scaleTweenTime;
    [SerializeField] private float waitBeforeScale;
    private Sequence pulseSequence;

    [Header("Particles")]
    [SerializeField] private ParticleSystem flowerParticles;
    [SerializeField] private ParticleSystem leafParticles;

    [SerializeField] private ParticleSystem pickedParticles;
    [SerializeField] private ParticleSystem pickedLeafParticles;

    private bool isPaired = false;
    public bool IsPaired { get => isPaired; set => isPaired = value; }

    private void Awake()
    {
        FlowerVisual flowerVisual = flowerList.GetRandomFlowerVisual();

        flowerSprite.GetComponent<RedrawAnimation>().Sprites = flowerVisual.flowerSprites;
        stemSprite.GetComponent<RedrawAnimation>().Sprites = flowerVisual.stemSprites;

        flowerSprite.transform.localPosition = flowerVisual.flowerPosition;

        pickedParticles.Stop();
        pickedLeafParticles.Stop();
    }

    public Color GetColor()
    {
        return flowerSprite.color;
    }

    public void SetColor(Color color)
    {
        flowerSprite.color = color;

        var pickedMain = pickedParticles.main;
        pickedMain.startColor = new ParticleSystem.MinMaxGradient(this.GetColor());

        var flowerModule = flowerParticles.main;
        flowerModule.startColor = new ParticleSystem.MinMaxGradient(this.GetColor());
    }


    // this animation makes the flower you already picked up easier to distinguish
    public void StartPickedAnimation()
    {
        // Setting random leaf sprite
        var textureSheet = pickedLeafParticles.textureSheetAnimation;
        textureSheet.SetSprite(0, leafSprites[Random.Range(0, leafSprites.Count)]);

        pickedParticles.Play();
        pickedLeafParticles.Play();

        transform.DOPunchScale(Vector3.one / 3, scaleTweenTime, 2, 0.3f).SetEase(Ease.InOutSine)
                .OnComplete(() => StartPulsingAnimation());
    }

    private void StartPulsingAnimation()
    {
        // Crea una secuencia infinita con DOTween
        pulseSequence = DOTween.Sequence();
        pulseSequence.Append(transform.DOScale(1.1f, 0.3f));
        pulseSequence.SetLoops(-1, LoopType.Yoyo);
        pulseSequence.OnKill(() => transform.localScale = Vector3.one);
    }

    public void StopPickedAnimation()
    {
        if(pulseSequence != null)
        {
            pulseSequence.Kill();
        }

        pickedParticles.Stop();
        pickedLeafParticles.Stop();
    }

    public void StartPairedAnimation()
    {
        // Setting random leaf sprite
        var textureSheet = leafParticles.textureSheetAnimation;
        textureSheet.SetSprite(0, leafSprites[Random.Range(0, leafSprites.Count)]);

        flowerParticles.Play();
        leafParticles.Play();

        StartCoroutine(WaitAndScale());
    }

    IEnumerator WaitAndScale()
    {
        // Espera a que termine el sistema de partículas
        yield return new WaitForSeconds(waitBeforeScale);
        // Luego ejecuta la animación de escala

        StopPickedAnimation();

        transform.DOScale(new Vector3(1.3f, 1.3f, 1.3f), 0.15f).SetEase(Ease.InOutSine)
            .OnComplete(() => transform.DOScale(new Vector3(0, 0, 0), scaleTweenTime).SetEase(Ease.InOutSine));
    }
}
