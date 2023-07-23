using UnityEngine;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
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
