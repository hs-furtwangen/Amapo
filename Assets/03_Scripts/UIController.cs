using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject background;
    [SerializeField] private Animator blackScreenAnimator;
    [SerializeField] private GameObject blackScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private VideoPlayer gameOverPlayer;
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_Text turnsLeftText;
    [SerializeField] private TMP_Text turnsUsedText;
    [SerializeField] private int nextSceneIndex;
    [SerializeField] private float loadLevelDelay = 2f;

    private void Start()
    {
        if (!GameManager.instance) return;

        GameManager.instance.OnGameStart += UpdateUI;
        GameManager.instance.OnDaytimeChanged += UpdateUI;

        GameManager.instance.OnGameOver += GameOver;
        GameManager.instance.OnGameWin += GameWon;
    }

    private void UpdateUI()
    {
        turnsLeftText.text = GameManager.instance.GetSwitchesLeft().ToString();
        turnsUsedText.text = GameManager.instance.GetSwitchCount().ToString();
    }

    private void UpdateUI(Daytime _daytime)
    {
        UpdateUI();
    }

    private void GameOver(){
        gameOverText.SetActive(true);
        gameOverScreen.SetActive(true);
        gameOverPlayer.Play();
        StartCoroutine(RestartLevelIn(5f));
    }

    IEnumerator RestartLevelIn(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameWon() {
        StartCoroutine(OpenGameWonIn(4f));
    }

    IEnumerator OpenGameWonIn(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        startButton.gameObject.SetActive(true);
        background.SetActive(true);
    }

    private void FadeIn()
    {
        startButton.enabled = false;
        blackScreenAnimator.SetTrigger("FadeIn");
    }

    public void DisableBlackscreen()
    {
        blackScreen.SetActive(false);
    }

    public void StartLevel(int _level)
    {
        FadeIn();
        SceneManager.LoadScene(_level);
    }

    public void StartLevelDelayed(int _level)
    {
        print("StartLevelDelayed");
        FadeIn();
        StartCoroutine(StartLevelIn(_level, loadLevelDelay));
    }

    IEnumerator StartLevelIn(int _level, float _time)
    {
        yield return new WaitForSeconds(_time);
        SceneManager.LoadSceneAsync(_level);
    }
}
