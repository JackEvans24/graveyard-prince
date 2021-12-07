using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowBodyPart : MonoBehaviour
{
    private Image image;
    private TMP_Text text;

    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private float balloonTime = 0.5f;
    [SerializeField] private float pauseTime = 2f;

    private void Awake()
    {
        this.image = GetComponentInChildren<Image>();
        this.text = GetComponentInChildren<TMP_Text>();
    }

    public IEnumerator Animate(BodyPart part)
    {
        this.image.sprite = part.Image;

        this.image.transform.DOScale(1, this.balloonTime);
        yield return new WaitForSeconds(this.balloonTime);

        this.text.text = string.Format("You got {0}", part.PartName);
        this.text.DOFade(1, this.fadeTime);
        yield return new WaitForSeconds(this.fadeTime);

        yield return new WaitForSeconds(this.pauseTime);

        this.text.DOFade(0, this.fadeTime);
        this.image.DOFade(0, this.fadeTime);

        yield return new WaitForSeconds(this.fadeTime);
    }
}
