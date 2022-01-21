using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public event Action<Daytime> OnDaytimeChanged;
    public static GameManager instance;
    [SerializeField] private Daytime daytime;
    [SerializeField] private List<GameObject> characters;
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

    public void ChangeDaytime()
    {
        daytime = daytime == Daytime.Day ? Daytime.Night : Daytime.Day;
        switchCount++;
        switchesLeft--;
        OnDaytimeChanged?.Invoke(daytime);
    }
}
