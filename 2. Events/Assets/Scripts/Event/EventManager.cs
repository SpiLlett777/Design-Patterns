using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public event Action<int> OnCoinCollected;
    public void CoinCollected(int value) => OnCoinCollected?.Invoke(value);

    public event Action OnLevelCompleted;
    public void LevelCompleted() => OnLevelCompleted?.Invoke();
}