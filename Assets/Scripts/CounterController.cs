using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CounterController : MonoBehaviour
{
    public TextMeshProUGUI countdownText; // referencia a tu componente TextMeshProUGUI
    public int countdownTime; // tiempo inicial en segundos

    void Start()
    {
        //StartCoroutine(StartCountdown());
    }

    public void startCountDown()
    {
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown()
    {
        while (countdownTime > 0)
        {
            // convierte el tiempo en formato minuto:segundo
            int minutes = countdownTime / 60;
            int seconds = countdownTime % 60;

            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        // puedes añadir aquí lo que quieras que suceda cuando el contador llegue a cero
        countdownText.text = "00:00";
    }
}
