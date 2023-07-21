using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Flower: MonoBehaviour
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

    [Header("Particles")]
    [SerializeField] private ParticleSystem flowerParticles;
    [SerializeField] private ParticleSystem leafParticles;

    private bool isPaired = false;

    private void Awake()
    {
        FlowerVisual flowerVisual = flowerList.GetRandomFlowerVisual();

        flowerSprite.GetComponent<RedrawAnimation>().Sprites = flowerVisual.flowerSprites;
        stemSprite.GetComponent<RedrawAnimation>().Sprites = flowerVisual.stemSprites;

        flowerSprite.transform.localPosition = flowerVisual.flowerPosition;
    }

    public Color GetColor()
    {
        return flowerSprite.color;
    }

    public void SetColor(Color color)
    {
        flowerSprite.color = color;
    }

    public bool IsPaired { get => isPaired; set => isPaired = value; }

    public void InitPairedAnimation()
    {
        var mainModule = flowerParticles.main;
        mainModule.startColor = new ParticleSystem.MinMaxGradient(this.GetColor());

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
        transform.DOScale(new Vector3(0, 0, 0), scaleTweenTime).SetEase(Ease.InOutSine);
    }
}
