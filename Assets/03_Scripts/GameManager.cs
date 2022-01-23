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
    [SerializeField] private CinemachineVirtualCamera worldCam;
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
    [SerializeField] private float toggleCamChangeDelay = 1f;
    [SerializeField] private float changeDaytimeDelay = 3f;

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

        OnDaytimeChanged?.Invoke(daytime);

        TogglePlayerCameras(false);

        ChangeDaytime(false, false);
        // ChangeDaytime(false, false);
        // ChangeDaytime(false, false);

        StartCoroutine(StartGameIn(startGameDelay));
        // StartGame();
    }

    private void StartGame()
    {
        gameState = GameState.Playing;

        // worldCam.enabled = false;
        ChangeDaytime(true);
        // ChangeDaytime(false);

        OnGameStart?.Invoke();
    }

    private void Update()
    {
        backgroundMusic.pitch = Mathf.Lerp(backgroundMusic.pitch, daytime == Daytime.Day ? backgroundMusicPitchDay : backgroundMusicPitchNight, backgroundMusicLerpSpeed * Time.deltaTime);

        if (gameState != GameState.Playing)
            return;


        InputData inputData = InputController.GetInputData();
        if (inputData.ChangeDaytime)
            ChangeDaytime();

        switch (daytime)
        {
            case Daytime.Day:
                dayPlayer.TakeInput(inputData);
                break;
            case Daytime.Night:
                nightPlayer.TakeInput(inputData);
                break;
        }

    }

    private void GameOver()
    {
        gameState = GameState.GameEnded;

        TogglePlayerCameras();

        print("Game over!");

        OnGameOver?.Invoke();
    }

    private void GameWin()
    {
        gameState = GameState.GameEnded;

        TogglePlayerCameras();

        print("Game won!");

        OnGameWin?.Invoke();
    }

    public void ChangeDaytime(bool delay = true, bool toggleCameras = true)
    {
        gameState = GameState.ChangingDaytime;
        daytime = daytime == Daytime.Day ? Daytime.Night : Daytime.Day;

        switchCount++;
        switchesLeft--;


        if (switchesLeft <= 0)
        {
            GameOver();
            return;
        }

        OnDaytimeChanged?.Invoke(daytime);

        if (toggleCameras)
        {
            if (delay)
                StartCoroutine(ToggleCamerasIn(toggleCamChangeDelay));
            else
                ToggleCameras();
        }

        if (delay)
            StartCoroutine(ChangeGameState(GameState.Playing, changeDaytimeDelay));
        else
            gameState = GameState.Playing;
    }

    public void TogglePlayerCameras(bool _toggle = false)
    {
        dayPlayer?.ToggleCamera(_toggle);
        nightPlayer?.ToggleCamera(_toggle);

        worldCam.enabled = !_toggle;
    }

    public void ToggleCameras()
    {
        dayPlayer.ToggleCamera(daytime == Daytime.Day);
        nightPlayer.ToggleCamera(daytime == Daytime.Night);

        worldCam.enabled = false;
    }

    public List<PlayerController> GetCharacters()
    {
        return new List<PlayerController>() { dayPlayer, nightPlayer };
    }

    IEnumerator StartGameIn(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        StartGame();
    }

    IEnumerator ToggleCamerasIn(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        ToggleCameras();

    }

    IEnumerator ChangeGameState(GameState _gameState, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        gameState = _gameState;
    }
}
