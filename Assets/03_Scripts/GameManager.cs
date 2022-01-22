using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action<Daytime> OnDaytimeChanged;
    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnGameWin;

    [SerializeField] private GameState gameState = GameState.NotStarted;
    [SerializeField] private Daytime daytime = Daytime.Day;
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    [SerializeField] private int switchCount = 0;
    [SerializeField] private int switchesLeft = 10;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void StartGame()
    {
        gameState = GameState.Playing;

        OnGameStart?.Invoke();
    }

    private void GameOver()
    {
        gameState = GameState.GameEnded;

        OnGameOver?.Invoke();
    }

    private void GameWin()
    {
        gameState = GameState.GameEnded;

        OnGameWin?.Invoke();
    }

    public void ChangeDaytime()
    {
        daytime = daytime == Daytime.Day ? Daytime.Night : Daytime.Day;

        switchCount++;
        switchesLeft--;

        OnDaytimeChanged?.Invoke(daytime);

        if(switchesLeft <= 0)
            GameOver();
    }

    public void CharacterOnGoal(GameObject _character) {

    }
}
