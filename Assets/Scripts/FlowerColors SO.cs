using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FlowerColorsSO", menuName = "ScriptableObjects/Flower Colors")]
public class FlowerColorsSO : ScriptableObject
{
    [SerializeField] private List<Color> colors;
    private HashSet<Color> usedColors = new HashSet<Color>();

    public Color GetColor()
    {
        // Verificar si se han devuelto todos los colores
        if (usedColors.Count >= colors.Count)
        {
            Debug.LogWarning("Se han devuelto todos los colores disponibles.");
            return Color.white; // O cualquier color predeterminado que desees utilizar
        }

        // Buscar un color no utilizado aleatoriamente
        Color randomColor;
        do
        {
            randomColor = colors[Random.Range(0, colors.Count)];
        }
        while (usedColors.Contains(randomColor));

        // Agregar el color a los colores utilizados
        usedColors.Add(randomColor);

        return randomColor;
    }
}