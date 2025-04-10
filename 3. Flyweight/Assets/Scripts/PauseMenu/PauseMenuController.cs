using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pausePanel;
    public Button resumeButton;
    public Button quitButton;
    public AudioSource audioSource;
    public AudioClip clickSound;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);

        resumeButton.onClick.AddListener(OnResumeClick);
        quitButton.onClick.AddListener(OnQuitClick);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                PlaySound();
                ResumeGame();
            }
            else
            {
                PlaySound();
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false); 
        Time.timeScale = 1f;
    }

    private void OnResumeClick()
    {
        PlaySound();
        ResumeGame();
    }

    private void OnQuitClick()
    {
        PlaySound();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    private void PlaySound()
    {
        if (audioSource is not null && clickSound is not null)
            audioSource.PlayOneShot(clickSound);
    }
}
