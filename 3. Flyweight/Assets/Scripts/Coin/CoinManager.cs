using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    private int coins;
    [SerializeField] private TMP_Text coinsDisplay;

    private void Awake()
    {
        if (instance is null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() => EventManager.Instance.OnCoinCollected += ChangeCoins;

    private void OnDisable() => EventManager.Instance.OnCoinCollected -= ChangeCoins;

    private void ChangeCoins(int amount)
    {
        coins += amount;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (coinsDisplay is not null)
            coinsDisplay.text = coins.ToString();
    }

    public int GetCoins() => coins;
}