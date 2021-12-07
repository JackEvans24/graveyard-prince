using DG.Tweening;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AltarController : MonoBehaviour
{
    public bool HasPrincess;

    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private float canvasFadeTime = 0.1f;
    [SerializeField] private BloodSplat splat;
    [SerializeField] private ShowBodyPart showPart;
    [SerializeField] private BodyPart part;

    private Animator anim;

    PlayerController player;

    private void Awake()
    {
        this.anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (this.player == null)
                this.player = collision.GetComponent<PlayerController>();

            if (this.player != null)
            {
                if (this.player.HasPrincess)
                    this.canvas.DOFade(1, this.canvasFadeTime);
                else
                    this.canvas.DOFade(0, this.canvasFadeTime);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.canvas.DOFade(0, this.canvasFadeTime);
            this.player = null;
        }
    }

    public IEnumerator Sacrifice(PrincessController princess)
    {
        HasPrincess = true;

        this.canvas.DOFade(0, this.canvasFadeTime);
        Destroy(princess.gameObject);

        this.anim.SetTrigger("Sacrifice");

        yield return new WaitForEndOfFrame();

        var animation = this.anim.GetCurrentAnimatorStateInfo(this.gameObject.layer);
        yield return new WaitForSeconds(animation.length);

        GameController.CutToBlack();

        yield return this.splat.Play();

        this.showPart.gameObject.SetActive(true);
        yield return this.showPart.Animate(this.part);

        GameController.NextLevel();
    }
}
