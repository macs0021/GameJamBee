using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using System;

[Serializable]
public struct Letter
{
    public RectTransform rect;
    public Vector2 finalPosition;
}

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private List<Letter> letters;

    [SerializeField] private RectTransform mainMenu;
    [SerializeField] private RectTransform helpMenu;

    private float mainMenuStartYPosition;
    private float helpMenuStartYPosition;
    [SerializeField] private float mainMenuEndYPosition;
    [SerializeField] private float helpMenuEndYPosition;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;

        mainMenuStartYPosition = mainMenu.anchoredPosition.y;
        helpMenuStartYPosition = helpMenu.anchoredPosition.y;

        helpMenu.anchoredPosition = new Vector2(helpMenu.anchoredPosition.x, helpMenuEndYPosition);
    }

    private void Start()
    {
        StartLettersAnimation();
    }

    private void StartLettersAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(1.0f);

        foreach (Letter letter in letters)
        {
            float randomTime = UnityEngine.Random.Range(0.1f, 0.3f);
            sequence.Append(letter.rect
                .DOAnchorPos(letter.finalPosition, randomTime).SetEase(Ease.OutBack));
        }

        sequence.OnComplete(() => StartLetterShakeLoop());
    }

    private void StartLetterShakeLoop()
    {
        float randomWaitTime = UnityEngine.Random.Range(2.0f, 4.0f);
        int randomIndex = UnityEngine.Random.Range(0, letters.Count);

        letters[randomIndex].rect.DOShakeAnchorPos(0.7f, 10)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                DOVirtual.DelayedCall(randomWaitTime, StartLetterShakeLoop);
            });
    }

    public void Play()
    {
        SceneController.Instance.LoadScene(SceneController.SceneType.GAME);
    }

    public void Exit()
    {
        //AudioController.Instance.Play("Button");
        Application.Quit();
    }

    public void Help()
    {
        //AudioController.Instance.Play("HelpButton");
        mainMenu.DOAnchorPos(new Vector2(mainMenu.anchoredPosition.x, mainMenuEndYPosition), 0.8f)
            .SetEase(Ease.InOutSine);

        helpMenu.DOAnchorPos(new Vector2(helpMenu.anchoredPosition.x, helpMenuStartYPosition), 0.6f)
            .SetEase(Ease.InOutSine);
    }

    public void Back()
    {
        //AudioController.Instance.Play("HelpButton");
        helpMenu.DOAnchorPos(new Vector2(helpMenu.anchoredPosition.x, helpMenuEndYPosition), 0.8f)
            .SetEase(Ease.InOutSine);

        mainMenu.DOAnchorPos(new Vector2(mainMenu.anchoredPosition.x, mainMenuStartYPosition), 0.6f)
            .SetEase(Ease.InOutSine);
    }
}
