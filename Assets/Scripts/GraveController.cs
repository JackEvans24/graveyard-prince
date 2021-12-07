using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GraveController : MonoBehaviour
{
    public bool HasPrincess;

    private Animator anim;

    [SerializeField] private GameObject princessPrefab;
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private float canvasFadeTime = 0.1f;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AudioSource dig;


    private void Awake()
    {
        this.anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.HasPrincess && collision.CompareTag("Player"))
            canvas.DOFade(1, this.canvasFadeTime);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            canvas.DOFade(0, this.canvasFadeTime);
    }

    public bool TryGetPrincess(out GameObject princess)
    {
        princess = null;
        if (!HasPrincess)
            return false;

        this.dig.Play();
        this.particles.Play();
        this.anim.SetTrigger("Action");

        princess = Instantiate(princessPrefab, transform.position, transform.rotation);

        canvas.DOFade(0, this.canvasFadeTime);
        this.HasPrincess = false;

        return true;
    }
}
