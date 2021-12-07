using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private PlayerController player;
    [SerializeField] private SpriteRenderer arrow;
    [SerializeField] private AltarController altar;

    [Header("Variables")]
    [SerializeField] private float textFadeSpeed = 0.2f;
    [SerializeField] private float arrowFadeSpeed = 0.5f;
    [SerializeField] private float arrowMoveSpeed = 1f;
    [SerializeField] private Vector2 altarOffset;

    private int tutorialStage;

    private void Update()
    {
        if (tutorialStage == 0 && Input.GetAxis("Horizontal") != 0)
        {
            tutorialStage++;
            StartCoroutine(ChangeText("Collect an unwilling donor"));
            ShowArrow();
        }
        else if (tutorialStage == 1 && player.HasPrincess)
        {
            tutorialStage++;
            StartCoroutine(ChangeText("Sacrifice their parts for your new friend"));
            MoveArrow();
        }
        else if (tutorialStage == 2 && altar.HasPrincess)
        {
            tutorialStage++;
            StartCoroutine(ChangeText(string.Empty));
            HideArrow();
        }
    }

    private IEnumerator ChangeText(string newText)
    {
        text.DOFade(0, this.textFadeSpeed);

        yield return new WaitForSeconds(this.textFadeSpeed);

        if (string.IsNullOrWhiteSpace(newText))
            yield break;

        text.text = newText;

        text.DOFade(1, this.textFadeSpeed);

        yield return new WaitForSeconds(this.textFadeSpeed);
    }

    private void ShowArrow()
    {
        this.arrow.DOFade(1, this.arrowFadeSpeed).SetEase(Ease.InOutSine);
    }

    private void MoveArrow()
    {
        this.arrow.transform.DOMove(altar.transform.position + altarOffset.ToVector3(), this.arrowMoveSpeed).SetEase(Ease.OutSine);
    }

    private void HideArrow()
    {
        this.arrow.DOFade(0, this.arrowFadeSpeed).SetEase(Ease.InOutSine);
    }
}
