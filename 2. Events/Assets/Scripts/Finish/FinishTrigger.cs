using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private int requiredCoins = 7;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float messageDuration = 2f;
    private bool levelCompleted = false;

    private void Start()
    {
        if (messageText is not null)
            messageText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !levelCompleted)
        {
            int collectedCoins = CoinManager.instance.GetCoins();

            if (collectedCoins >= requiredCoins)
            {
                levelCompleted = true;
                PlayerPrefs.SetInt("Level1Completed", 1);
                PlayerPrefs.Save();

                EventManager.Instance.LevelCompleted();
                SceneManager.LoadScene("LevelMenu");
            }
            else
            {
                StartCoroutine(ShowMessage($"Собрано {collectedCoins} из {requiredCoins} монеток!"));
            }
        }
    }

    private IEnumerator ShowMessage(string message)
    {
        if (messageText is not null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);
            yield return new WaitForSeconds(messageDuration);
            messageText.gameObject.SetActive(false);
        }
    }
}