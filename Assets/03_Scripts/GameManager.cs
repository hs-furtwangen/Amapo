using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event Action<Daytime> OnDaytimeChanged;
    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnGameWin;

    [SerializeField] private GameState gameState = GameState.NotStarted;
    [SerializeField] private Daytime daytime = Daytime.Day;
    [SerializeField] private CinemachineFreeLook worldCam;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private float backgroundMusicPitchDay = 1f;
    [SerializeField] private float backgroundMusicPitchNight = 0.5f;
    [SerializeField] private float backgroundMusicLerpSpeed = 1f;
    [SerializeField] private PlayerController dayPlayer;
    [SerializeField] private PlayerController nightPlayer;
    [SerializeField] private Goal goal;
    [SerializeField] private int switchCount = 0;
    [SerializeField] private int switchesLeft = 10;
    [SerializeField] private float startGameDelay = 3f;

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

        TurnOffPlayerCameras();

        StartCoroutine(StartGameIn(startGameDelay));
    }

    private void StartGame()
    {
        gameState = GameState.Playing;

        worldCam.enabled = false;
        ChangeDaytime();
        ChangeDaytime();

        OnGameStart?.Invoke();
    }

    private void Update() {
        if(gameState != GameState.Playing)
            return;

        InputData inputData = InputController.GetInputData();
        if(inputData.ChangeDaytime)
            ChangeDaytime();

        switch(daytime) {
            case Daytime.Day:
                dayPlayer.TakeInput(inputData);
                break;
            case Daytime.Night:
                nightPlayer.TakeInput(inputData);
                break;
        }

        backgroundMusic.pitch = Mathf.Lerp(backgroundMusic.pitch, daytime == Daytime.Day ? backgroundMusicPitchDay : backgroundMusicPitchNight, backgroundMusicLerpSpeed * Time.deltaTime);
    }

    private void GameOver()
    {
        gameState = GameState.GameEnded;

        TurnOffPlayerCameras();

        print("Game over!");

        OnGameOver?.Invoke();
    }

    private void GameWin()
    {
        gameState = GameState.GameEnded;

        TurnOffPlayerCameras();

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

    public void TurnOffPlayerCameras() {
        dayPlayer?.ToggleCamera(false);
        nightPlayer?.ToggleCamera(false);

        worldCam.enabled = true;
    }

    public List<PlayerController> GetCharacters()  {
        return new List<PlayerController>() { dayPlayer, nightPlayer };
    }

    IEnumerator StartGameIn(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        StartGame();
    }
}
