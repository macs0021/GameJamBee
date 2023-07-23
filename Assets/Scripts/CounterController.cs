using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CounterController : MonoBehaviour
{
    [SerializeField] private RectTransform countdownRect;
    [SerializeField] private TextMeshProUGUI countdownText; 
    [SerializeField] private int countdownTime;

    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private BeeController bee;

    private bool countdownActive;
    private float countdownFinalPosX;

    private void Awake()
    {
        losePanel.SetActive(false);

        countdownActive = false;
        countdownFinalPosX = countdownRect.anchoredPosition.x;
        countdownRect.anchoredPosition = new Vector2(countdownFinalPosX + 200.0f, countdownRect.anchoredPosition.y);
    }

    public void StartTimer()
    {
        if (!countdownActive)
        {
            countdownActive = true;
            countdownRect.DOAnchorPosX(countdownFinalPosX, 0.5f).SetEase(Ease.InOutSine);
            StartCoroutine(StartCountdown());
        }
    }

    public void StopTimer()
    {
        StopAllCoroutines();
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
        AudioController.Instance.Play("Lose");
        losePanel.SetActive(true);
        losePanel.GetComponent<RectTransform>().DOAnchorPosY(0, 0.8f).SetEase(Ease.InOutSine);
        text.text = "Oh no... The bee didn't polinize all the flowers in time";
        bee.CanMove = false;
        countdownText.text = "End!";
    }
}
