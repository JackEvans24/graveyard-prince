using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class PauseMenu : MonoBehaviour
{
    private CanvasGroup canvas;
    private bool paused;

    public static bool CanPause;

    private void Awake()
    {
        this.canvas = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (!CanPause) return;

        if (Input.GetButtonDown("Cancel"))
        {
            if (paused)
                Resume();
            else
            {
                paused = true;
                //Time.timeScale = 0.1f;
                canvas.alpha = 1;
            }
        }
    }

    public void Resume()
    {
        paused = false;
        canvas.alpha = 0;
        Time.timeScale = 1;
    }

    public void Quit()
    {
        if (!this.paused)
            return;

        paused = false;
        canvas.alpha = 0;
        Time.timeScale = 1;
        GameController.ReturnToMenu();
    }
}
