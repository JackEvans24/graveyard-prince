using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    [Header("Background")]
    [SerializeField] private GameObject background;

    [Header("Skips")]
    [SerializeField] private bool SkipIntro;

    [Header("Level overlay")]
    [SerializeField] private CanvasGroup endOfLevelCanvas;
    [SerializeField] private float canvasFadeTime;

    private bool loading = false;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        SceneManager.sceneLoaded += (scene, loadMode) => this.HideCanvas();
    }

    private void HideCanvas()
    {
        this.endOfLevelCanvas.DOFade(0, this.canvasFadeTime);
    }

    private IEnumerator ShowCanvas()
    {
        this.endOfLevelCanvas.DOFade(1, this.canvasFadeTime);

        yield return new WaitForSeconds(this.canvasFadeTime);
    }

    public void StartGame()
    {
        var index = SkipIntro ? 2 : 1;
        StartCoroutine(LoadLevelAfterCanvas(index));
    }

    public static void Play()
    {
        Instance.StartGame();
    }

    public static void CutToBlack()
    {
        Instance.endOfLevelCanvas.alpha = 1;
    }

    public static void ReturnToMenu()
    {
        Instance.StartCoroutine(LoadLevelAfterCanvas(0));
    }

    public static void NextLevel()
    {
        var index = SceneManager.GetActiveScene().buildIndex + 1;
        if (index >= 24)
            index = 0;

        Instance.StartCoroutine(LoadLevelAfterCanvas(index));
    }

    private static IEnumerator LoadLevelAfterCanvas(int buildIndex)
    {
        if (Instance.loading)
            yield break;

        Instance.loading = true;
        PauseMenu.CanPause = false;

        yield return Instance.ShowCanvas();
        SceneManager.LoadScene(buildIndex);

        if (!Instance.background.activeInHierarchy)
            ShowBackground();

        Instance.loading = false;
        PauseMenu.CanPause = buildIndex != 0;
    }

    public static void ResetLevel()
    {
        Instance.StartCoroutine(LoadLevelAfterCanvas(SceneManager.GetActiveScene().buildIndex));
    }

    public static void ShowBackground()
    {
        Instance.background.SetActive(true);
    }

    public static void HideBackground()
    {
        Instance.background.SetActive(false);
    }
}
