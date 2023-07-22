using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchController : MonoBehaviour
{
    public GameObject flowerObj;
    public GameObject[] objects;
    public GameObject spherePrefab;
    public GameObject frontLeafs;
    public GameObject backLeafs;

    private void Start()
    {
        RandomizeScale();
        RotateLeafsRandomly();
       
    }
    public void RandomizeScale()
    {
        foreach (GameObject obj in objects)
        {
            float randomScale = Random.Range(1f, 1.3f);
            Vector3 originalScale = obj.transform.localScale;
            Vector3 newScale = originalScale * randomScale;
            obj.transform.localScale = newScale;
        }
    }

    void RotateLeafsRandomly()
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
        flowerObj.gameObject.SetActive(true);

        Flower flower = flowerObj.GetComponent<Flower>();
        flower.SetColor(newColor);
    }

    public bool flowerIsPaired()
    {
        return flowerObj.GetComponent<Flower>().IsPaired;
    }

    public void disableFlower()
    {
        flowerObj.SetActive(false);
    }

    void GenerateSpheres()
    {
        // Obtener las dimensiones del cilindro
        float cylinderRadius = this.transform.localScale.x / 2;
        float cylinderHeight = this.transform.localScale.y-1;

        // Calcular el número de esferas que se pueden generar
        int sphereCount = 30;

        // Para cada esfera
        for (int i = 0; i < sphereCount; ++i)
        {
            // Calcular las coordenadas esféricas
            float phi = Random.Range(0, 2 * Mathf.PI);
            float theta = Random.Range(0, Mathf.PI);

            // Calcular las coordenadas cartesianas en espacio local
            float x = cylinderRadius * Mathf.Sin(theta) * Mathf.Cos(phi);
            float y = Random.Range(-cylinderHeight / 2, cylinderHeight / 2);
            float z = cylinderRadius * Mathf.Sin(theta) * Mathf.Sin(phi);

            // Convertir a espacio global
            Vector3 globalPosition = this.transform.TransformPoint(new Vector3(x, y, z));

            // Crear la esfera
            GameObject sphere = Instantiate(spherePrefab, globalPosition, Quaternion.identity);

            // Ajustar el tamaño de la esfera
            float scale = Random.Range(0.5f, 2.0f);
            sphere.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
