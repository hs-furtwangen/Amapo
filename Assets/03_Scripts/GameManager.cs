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

        LockCursor(true);
    }

    private void Start()
    {
        goal.OnAllPlayerOnGoal += GameWin;

        OnDaytimeChanged?.Invoke(daytime);

        TogglePlayerCameras(false);

        ChangeDaytime(false, false, false);

        StartCoroutine(StartGameIn(startGameDelay));
    }

    private void StartGame()
    {
        gameState = GameState.Playing;

        ChangeDaytime(true, true, false);

        OnGameStart?.Invoke();
    }

    private void LockCursor(bool _locked)
    {
        Cursor.lockState = _locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !_locked;
    }

    private void Update()
    {
        backgroundMusic.pitch = Mathf.Lerp(backgroundMusic.pitch, daytime == Daytime.Day ? backgroundMusicPitchDay : backgroundMusicPitchNight, backgroundMusicLerpSpeed * Time.deltaTime);

        if (gameState != GameState.Playing)
            return;

        InputData inputData = InputController.GetInputData();
        if (inputData.ChangeDaytime)
            ChangeDaytime();

        if(inputData.PauseGame)
            LockCursor(false);

        if(inputData.ResumeGame)
            LockCursor(true);

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

        LockCursor(false);

        StartCoroutine(TogglePlayerCamerasIn(toggleCamChangeDelay, false));

        OnGameOver?.Invoke();
    }

    private void GameWin()
    {
        gameState = GameState.GameEnded;

        LockCursor(false);

        StartCoroutine(TogglePlayerCamerasIn(toggleCamChangeDelay, false));

        OnGameWin?.Invoke();
    }

    public void ChangeDaytime(bool delay = true, bool toggleCameras = true, bool useSwitches = true)
    {
        gameState = GameState.ChangingDaytime;
        daytime = daytime == Daytime.Day ? Daytime.Night : Daytime.Day;

        if (useSwitches)
        {
            switchCount++;
            switchesLeft--;
        }

        OnDaytimeChanged?.Invoke(daytime);

        if (switchesLeft < 0)
        {
            GameOver();
            return;
        }


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

    public int GetSwitchesLeft() => switchesLeft;
    public int GetSwitchCount() => switchCount;

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

    IEnumerator TogglePlayerCamerasIn(float _seconds, bool _value)
    {
        yield return new WaitForSeconds(_seconds);
        TogglePlayerCameras(_value);
    }

    IEnumerator ChangeGameState(GameState _gameState, float _seconds)
    {
        yield return new WaitForSeconds(_seconds);

        gameState = _gameState;
    }
}
