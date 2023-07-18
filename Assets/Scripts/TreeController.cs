using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public float velocidadRotacion = 10f;
    public GameObject branchPrefab;
    public int numberOfBranchesPairs = 4;
    public int numberOfFlowerPairs = 2;
    GameObject[] branches;



    private void Start()
    {
        GenerateBranches(8);
    }
    void Update()
    {
        // Rotar el objeto en el eje Y continuamente
        transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime);
    }
    public void GenerateBranches(int numberOfLevels)
    {
        float height = 40;
        float radius = 5;
        float levelHeight = (height / numberOfLevels)-0.5f;
        branches = new GameObject[5 * numberOfLevels];
        int branchIndex = 0;

        for (int level = 0; level < numberOfLevels; level++)
        {
            for (int i = 0; i < 5; i++)
            {
                // Determinar la altura y el ángulo de la rama
                float branchHeight = levelHeight * level;
                float branchAngle = Mathf.Deg2Rad * ((360 / 5) * i); // Convertir a radianes

                // Calcula la posición de la rama
                Vector3 branchPosition = new Vector3(
                    radius * Mathf.Sin(branchAngle),
                    branchHeight,
                    radius * Mathf.Cos(branchAngle)
                );

                // Crear una nueva instancia de la rama
                GameObject newBranch = Instantiate(branchPrefab, transform.position + branchPosition, Quaternion.identity);
                branches[branchIndex] = newBranch;
                branchIndex++;
                newBranch.transform.parent = this.gameObject.transform;

                // Ajustar la rotación de la rama para que apunte al centro
                newBranch.transform.LookAt(transform.position);

                // Ajustar el ángulo en X a 90 grados
                Vector3 currentRotation = newBranch.transform.rotation.eulerAngles;
                newBranch.transform.rotation = Quaternion.Euler(90, currentRotation.y, currentRotation.z);


            }
        }
        DisableRandomObjects(branches, numberOfBranchesPairs);
        ColorPairs(branches, numberOfFlowerPairs);
    }
    void DisableRandomObjects(GameObject[] gameObjects, int numActiveObjects)
    {
        // Verifica si hay suficientes objetos para desactivar
        if (gameObjects.Length <= numActiveObjects)
        {
            return;
        }

        // Crea una lista de índices de objetos
        List<int> indices = new List<int>();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            indices.Add(i);
        }

        // Mézcla la lista de índices
        System.Random rng = new System.Random();
        int n = indices.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int temp = indices[k];
            indices[k] = indices[n];
            indices[n] = temp;
        }

        // Desactiva los objetos hasta que solo queden numActiveObjects
        for (int i = 0; i < indices.Count - numActiveObjects; i++)
        {
            gameObjects[indices[i]].SetActive(false);
        }
    }
    void ColorPairs(GameObject[] objects, int numPairs)
    {
        // Crear una lista para almacenar los índices de los objetos activos
        List<int> activeIndices = new List<int>();
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].activeInHierarchy)
            {
                activeIndices.Add(i);
            }
        }

        // Verificar si hay suficientes objetos activos para formar los pares
        if (activeIndices.Count < numPairs * 2)
        {
            Debug.LogError("No hay suficientes objetos activos para formar " + numPairs + " pares.");
            return;
        }

        // Mézclar los índices activos
        System.Random rng = new System.Random();
        int n = activeIndices.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            int temp = activeIndices[k];
            activeIndices[k] = activeIndices[n];
            activeIndices[n] = temp;
        }

        // Formar pares con los primeros numPairs * 2 índices activos mezclados
        for (int i = 0; i < numPairs * 2; i += 2)
        {
            // Generar un nuevo color aleatorio
            Color newColor = new Color(Random.value, Random.value, Random.value);
            newColor.a = 1;

            // Aplicar este color a los dos objetos del par actual
            objects[activeIndices[i]].GetComponent<BranchController>().ChangeFlowerType(newColor);
            objects[activeIndices[i + 1]].GetComponent<BranchController>().ChangeFlowerType(newColor);
        }
    }

}
