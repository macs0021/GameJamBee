using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private FlowerColorsSO colors;

    [Header("Branches")]
    public GameObject branchPrefab;
    public int numberOfBranchesPairs = 1;
    public int numberOfFlowerPairs = 2;
    GameObject[] branches;

    [Header("Misc")]
    [SerializeField] private BeeController bee;
    [SerializeField] private GameObject endPanel;
    [SerializeField] private TextMeshProUGUI endPanelText;
    [SerializeField] private CounterController counter;

    private void Start()
    {
        colors.ClearUsedColors();
        GenerateBranches(11);
    }

    public void GenerateBranches(int numberOfLevels)
    {
        float height = 40;
        float radius = 5;
        float levelHeight = (height / numberOfLevels) - 0.5f;
        List<GameObject> branchList = new List<GameObject>();
        int branchIndex = 0;

        for (int level = 0; level < numberOfLevels; level++)
        {
            int branchesPerLevel = Random.Range(4, 6); // Elige un número aleatorio entre 4 y 5
            float levelRotation = Random.Range(0, 360); // Rotación aleatoria para cada nivel

            for (int i = 0; i < branchesPerLevel; i++)
            {
                // Determinar la altura y el ángulo de la rama
                float branchHeight = levelHeight * level;
                float branchAngle = Mathf.Deg2Rad * ((360 / branchesPerLevel) * i + levelRotation); // Convertir a radianes y añadir la rotación del nivel

                // Calcula la posición de la rama
                Vector3 branchPosition = new Vector3(
                    radius * Mathf.Sin(branchAngle),
                    branchHeight,
                    radius * Mathf.Cos(branchAngle)
                );

                // Crear una nueva instancia de la rama
                GameObject newBranch = Instantiate(branchPrefab, transform.position + branchPosition, Quaternion.identity);
                //newBranch.GetComponent<BranchController>().RandomizeScale();
                branchList.Add(newBranch);
                branchIndex++;
                newBranch.transform.parent = this.gameObject.transform;

                // Ajustar la rotación de la rama para que apunte al centro
                newBranch.transform.LookAt(transform.position);

                // Ajustar el ángulo en X a 90 grados
                Vector3 currentRotation = newBranch.transform.rotation.eulerAngles;
                newBranch.transform.rotation = Quaternion.Euler(90, currentRotation.y, currentRotation.z);
            }
        }

        branches = branchList.ToArray(); // Convertir la lista en un arreglo

        //DisableRandomObjects(branches, numberOfBranchesPairs);
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
            Color newColor = colors.GetColor();

            // Aplicar este color a los dos objetos del par actual
            objects[activeIndices[i]].GetComponent<BranchController>().ChangeFlowerType(newColor);
            objects[activeIndices[i + 1]].GetComponent<BranchController>().ChangeFlowerType(newColor);
        }
    }
    public void DestroyBranches()
    {
        for (int i = 0; i < branches.Length; i++)
        {
            Destroy(branches[i].gameObject);
        }

    }

    public void CheckGameCompleted()
    {
        foreach (GameObject branch in branches)
        {
            BranchController controller = branch.GetComponent<BranchController>();
            if (controller.HasFlower() && !controller.IsFlowerPaired())
            {
                Debug.Log(controller);
                return;
            }
        }

        bee.CanMove = true;
        endPanel.SetActive(true);
        endPanelText.text = "You won!";
        counter.StopTimer();
    }

}
