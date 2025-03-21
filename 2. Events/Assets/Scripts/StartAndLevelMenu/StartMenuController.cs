using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;

    public void OnStartClick()
    {
        PlaySound();
        SceneManager.LoadScene("LevelMenu");
    }

    private void Start()
    {
        if (audioSource is null)
            audioSource = GetComponent<AudioSource>();
    }

    public void OnExitClick()
    {
        PlaySound();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    private void PlaySound()
    {
        if (audioSource is not null && clickSound is not null)
            audioSource.PlayOneShot(clickSound);
    }
}