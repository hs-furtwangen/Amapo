using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]  private TMP_Text turnsLeftText;
    [SerializeField]  private TMP_Text turnsUsedText;

    private void Start()
    {
        GameManager.instance.OnGameStart += UpdateUI;
        GameManager.instance.OnDaytimeChanged += UpdateUI;
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
}
