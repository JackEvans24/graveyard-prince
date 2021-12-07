using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class OpeningCutscene : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private CanvasGroup[] images;

    [Header("Variables")]
    [SerializeField] private float textFadeSpeed = 1f;
    [SerializeField] private float imageFadeSpeed = 1f;
    [SerializeField] private float imagePauseTime = 1f;
    [SerializeField] private float textPauseTime = 3f;

    [Header("Cutscene")]
    [TextArea(1, 5)]
    [SerializeField] private string[] lines;

    private void Start()
    {
        GameController.HideBackground();

        StartCoroutine(RunScene());
    }

    private IEnumerator RunScene()
    {
        var sceneRegex = new Regex("^\\*{2}\\d\\*{2}$");
        var menuRegex = new Regex("^\\*{2}menu\\*{2}$");

        foreach (var line in this.lines)
        {
            if (sceneRegex.IsMatch(line))
                yield return LoadImage(line);
            else if (menuRegex.IsMatch(line))
            {
                GameController.ReturnToMenu();
                yield break;
            }
            else
                yield return LoadText(line);
        }

        GameController.NextLevel();
    }

    private IEnumerator LoadImage(string line)
    {
        line = line.Replace("*", string.Empty);
        var imageNumber = int.Parse(line);

        this.images[imageNumber - 1].DOFade(1, this.imageFadeSpeed);

        yield return new WaitForSeconds(this.imageFadeSpeed);

        yield return new WaitForSeconds(this.imagePauseTime);
    }

    private IEnumerator LoadText(string line)
    {
        if (this.text.alpha > 0)
        {
            this.text.DOFade(0, this.textFadeSpeed);
            yield return new WaitForSeconds(this.textFadeSpeed);
        }

        this.text.text = line;

        this.text.DOFade(1, this.textFadeSpeed);
        yield return new WaitForSeconds(this.textFadeSpeed);

        yield return new WaitForSeconds(this.textPauseTime);
    }
}
