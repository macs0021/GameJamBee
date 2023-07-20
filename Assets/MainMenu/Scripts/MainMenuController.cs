using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.None;
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
        animator.SetBool("helpActive", true);
    }

    public void Back()
    {
        //AudioController.Instance.Play("HelpButton");
        animator.SetBool("helpActive", false);
    }
}
