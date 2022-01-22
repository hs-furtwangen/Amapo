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
    [SerializeField] private PlayerController dayPlayer;
    [SerializeField] private PlayerController nightPlayer;
    [SerializeField] private List<PlayerController> characters = new List<PlayerController>();
    [SerializeField] private Goal goal;
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

    private void Start()
    {
        goal.OnAllPlayerOnGoal += GameWin;

        StartGame();
    }

    private void Update() {
        InputData inputData = InputController.GetInputData();
        if(inputData.ChangeDaytime) {
            ChangeDaytime();
        }

        switch(daytime) {
            case Daytime.Day:
                dayPlayer.TakeInput(inputData);
                break;
            case Daytime.Night:
                nightPlayer.TakeInput(inputData);
                break;
        }
    }

    private void StartGame()
    {
        gameState = GameState.Playing;

        OnGameStart?.Invoke();
    }

    private void GameOver()
    {
        gameState = GameState.GameEnded;

        print("Game over!");

        OnGameOver?.Invoke();
    }

    private void GameWin()
    {
        gameState = GameState.GameEnded;

        print("Game won!");

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

        dayPlayer.ToggleCamera(daytime == Daytime.Day);
        nightPlayer.ToggleCamera(daytime == Daytime.Night);
    }

    public List<PlayerController> GetCharacters() => characters;
}
