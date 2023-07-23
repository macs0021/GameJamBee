using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{
    [Header("Flower")]
    [SerializeField] private Flower flower;

    [Header("Leafs")]
    [SerializeField] private GameObject frontLeafs;
    [SerializeField] private GameObject backLeafs;

    private void Start()
    {
        RandomizeScale();
        RotateLeafsRandomly();
       
    }
    private void RandomizeScale()
    {
        // front leafs
        foreach (Transform leaf in frontLeafs.transform)
        {
            float randomScale = Random.Range(1f, 1.3f);
            leaf.localScale *= randomScale;
        }

        // back leafs
        foreach (Transform leaf in backLeafs.transform)
        {
            float randomScale = Random.Range(1f, 1.3f);
            leaf.localScale *= randomScale;
        }
    }

    private void RotateLeafsRandomly()
    {
        // Generar dos ángulos aleatorios para la rotación en el eje Y
        float frontLeafsRotation = Random.Range(0f, 360f);
        float backLeafsRotation = Random.Range(0f, 360f);

        // Aplicar la rotación a los objetos
        frontLeafs.transform.localRotation = Quaternion.Euler(0, frontLeafsRotation, 0);
        backLeafs.transform.localRotation = Quaternion.Euler(0, backLeafsRotation, 0);
    }

    public void ChangeFlowerType(Color newColor)
    {
        flower.gameObject.SetActive(true);
        flower.SetColor(newColor);
    }

    public bool IsFlowerPaired()
    {
        return flower.GetComponent<Flower>().IsPaired;
    }
}
