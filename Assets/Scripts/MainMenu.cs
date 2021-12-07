using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play() => GameController.Play();

    public void Quit() => Application.Quit();
}
