using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    public Button levelOneButton;

    private void Start()
    {
        if (PlayerPrefs.GetInt("Level1Completed", 0) == 1)
            ChangeButtonColor(levelOneButton, Color.green, new Color(0.6f, 1f, 0.6f), new Color(0.4f, 0.8f, 0.4f));
    }

    public void OnLevelOneClick()
    {
        PlaySound();
        SceneManager.LoadScene("TestLevel");
    }

    public void OnBackToMainMenuClick()
    {
        PlaySound();
        SceneManager.LoadScene("MainMenu");
    }

    private void PlaySound()
    {
        if (audioSource is not null && clickSound is not null)
            audioSource.PlayOneShot(clickSound);
    }

    private void ChangeButtonColor(Button button, Color normal, Color highlighted, Color pressed)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = normal;
        cb.highlightedColor = highlighted;
        cb.pressedColor = pressed;
        cb.selectedColor = highlighted;
        button.colors = cb;
    }
}