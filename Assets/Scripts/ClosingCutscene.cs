using DG.Tweening;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ClosingCutscene : MonoBehaviour
{
    public bool SkipText;

    [Header("References")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private CanvasGroup[] images;
    [SerializeField] private SpriteRenderer[] bodyParts;
    [SerializeField] private Transform[] sacrificePoints;
    [SerializeField] private BloodSplat splat;
    [SerializeField] private Transform circle;

    [Header("Cutscene Variables")]
    [SerializeField] private float textFadeSpeed = 1f;
    [SerializeField] private float imageFadeSpeed = 1f;
    [SerializeField] private float imagePauseTime = 1f;
    [SerializeField] private float textPauseTime = 3f;
    [SerializeField] private float endPauseTime = 3f;

    [Header("Ritual Variables")]
    [SerializeField] private Vector2 altarPosition;
    [SerializeField] private float partSpeed;
    [SerializeField] private float partInterval;
    [SerializeField] private float[] circleSteps;
    [SerializeField] private float circleStepSpeed;

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
        if (this.SkipText)
        {
            yield return StartRitual();
            yield break;
        }

        var imageRegex = new Regex("^\\*{2}\\d\\*{2}$");
        var ritualRegex = new Regex("^\\*{2}ritual\\*{2}$");

        foreach (var line in this.lines)
        {
            if (imageRegex.IsMatch(line))
                yield return LoadImage(line);
            else if (ritualRegex.IsMatch(line))
                yield return StartRitual();
            else
                yield return LoadText(line);
        }

        yield return new WaitForSeconds(this.endPauseTime);

        GameController.NextLevel();
    }

    private IEnumerator LoadImage(string line)
    {
        line = line.Replace("*", string.Empty);
        var imageNumber = int.Parse(line);

        var image = this.images[imageNumber - 1];
        var alpha = image.alpha > 0 ? 0 : 1;
        this.images[imageNumber - 1].DOFade(alpha, this.imageFadeSpeed);

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

        if (string.IsNullOrWhiteSpace(line))
            yield break;

        this.text.text = line;

        this.text.DOFade(1, this.textFadeSpeed);
        yield return new WaitForSeconds(this.textFadeSpeed);

        yield return new WaitForSeconds(this.textPauseTime);
    }

    private IEnumerator StartRitual()
    {
        var index = 0;
        foreach (var part in bodyParts)
        {
            var sacrificePoint = this.sacrificePoints[index % this.sacrificePoints.Length];
            index++;

            if (index >= bodyParts.Length)
                yield return SendPartToAltar(part, sacrificePoint);
            else
                StartCoroutine(SendPartToAltar(part, sacrificePoint));

            yield return new WaitForSeconds(this.partInterval);
        }

        foreach (var step in this.circleSteps)
        {
            this.circle.DOScale(step, this.circleStepSpeed).SetEase(Ease.InOutBounce);
            yield return new WaitForSeconds(this.circleStepSpeed);
        }
    }

    private IEnumerator SendPartToAltar(SpriteRenderer bodyPart, Transform sacrificePoint)
    {
        var tween = bodyPart.transform.DOMove(sacrificePoint.position, this.partSpeed);

        yield return tween.WaitForCompletion();

        tween = bodyPart.transform.DOMove(altarPosition, this.partSpeed);

        yield return tween.WaitForCompletion();

        bodyPart.enabled = false;

        yield return splat.Play();
    }
}
