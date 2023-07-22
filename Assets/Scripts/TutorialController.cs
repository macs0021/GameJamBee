using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

[System.Serializable]
public class Tutorial
{
    public string name;
    public List<string> textList;
}

public enum TutorialType
{
    ExplainMovement,
    ExplainFlower,
    ExplainCollision,
    ExplainPairs,
    ExplainCompleteAndNest
}

public class TutorialController : MonoBehaviour
{
    [SerializeField] private List<Tutorial> tutorialFrames;
    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("Position")]
    private float startPositionX;
    [SerializeField] private float endPositionX;

    private RectTransform rectTransform;

    [Header("Typing Speed")]
    [SerializeField]
    private float timePerCharacter = 0.05f;

    [SerializeField] private float timePerCharacterMultiplier = 3.0f;
    private float waitTimePerCharacter;

    private bool canContinue;
    private int currentFrame;
    private int currentFrameText;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPositionX = rectTransform.anchoredPosition.x;

        /*if (!PlayerPrefs.HasKey("tutorial"))
        {
            PlayerPrefs.SetInt("tutorial", 1);
            PlayerPrefs.Save();
        }
        else
        {
            Destroy(gameObject);
        }*/
    }

    void Start()
    {
        waitTimePerCharacter = timePerCharacter;

        canContinue = false;
        currentFrame = 0;
        currentFrameText = 0;

        Invoke("StartCurrentFrame", 1f);
    }

    public bool NextTutorial()
    {
        if (canContinue)
        {
            Continue();
            return true;
        }
        else
        {
            waitTimePerCharacter = timePerCharacter / timePerCharacterMultiplier;
            return false;
        }
    }

    private void StartCurrentFrame()
    {
        if (currentFrame < tutorialFrames.Count)
        {
            rectTransform.DOAnchorPos(new Vector2(endPositionX, rectTransform.anchoredPosition.y), 1.0f);
            StartCoroutine(WriteText(false));
        }
    }

    private void Continue()
    {
        if (currentFrame < tutorialFrames.Count)
        {
            if (currentFrameText < tutorialFrames[currentFrame].textList.Count)
            {
                StartCoroutine(WriteText(false));
            }
            else
            {
                StartCoroutine(ChangeFrame());
            }
        }
        else
        {
            endTutorial();
        }
    }

    IEnumerator WriteText(bool startFadeOut)
    {
        canContinue = false;

        if (startFadeOut)
        {
            yield return new WaitForSeconds(1.0f);
        }

        tutorialText.text = "";

        foreach (char c in tutorialFrames[currentFrame].textList[currentFrameText])
        {
            tutorialText.text += c;
            yield return new WaitForSeconds(waitTimePerCharacter);
        }

        waitTimePerCharacter = timePerCharacter;
        currentFrameText++;

        canContinue = true;
    }

    public void endTutorial()
    {
        rectTransform.DOAnchorPos(new Vector2(startPositionX, rectTransform.anchoredPosition.y), 1.0f);
    }

    IEnumerator ChangeFrame()
    {
        canContinue = false;

        rectTransform.DOAnchorPos(new Vector2(startPositionX, rectTransform.anchoredPosition.y), 1.0f);

        yield return new WaitForSeconds(1.0f);

        currentFrameText = 0;
        currentFrame++;

        if (currentFrame < tutorialFrames.Count)
        {
            rectTransform.DOAnchorPos(new Vector2(endPositionX, rectTransform.anchoredPosition.y), 1.0f);
            StartCoroutine(WriteText(false));
        }
    }
}
