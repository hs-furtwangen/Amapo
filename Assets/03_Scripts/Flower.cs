using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : MonoBehaviour
{
    [SerializeField] private GameObject flowerDay;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<Renderer> dayObjects = new List<Renderer>();
    [SerializeField] private GameObject flowerNight;
    [SerializeField] private List<Renderer> nightObjects = new List<Renderer>();
    [SerializeField] private float targetScale = 2f;
    [SerializeField] private float fadeSpeed = 0.5f;
    [SerializeField] private float scaleSpeed = 0.5f;
    [SerializeField] private float alphaThreshold = 0.1f;
    private Daytime daytime;
    private void Start()
    {
        GameManager.instance.OnDaytimeChanged += ChangeFlower;
    }

    private void Update()
    {
        float allAlpha = 0f;
        foreach (Renderer dayObject in dayObjects)
        {
            Color color = dayObject.material.color;
            color.a = Mathf.Clamp01(color.a + (daytime == Daytime.Day ? 1 : -1) * fadeSpeed * Time.deltaTime);
            allAlpha += color.a;
            dayObject.material.color = color;
        }

        flowerDay.SetActive((allAlpha / dayObjects.Count) > alphaThreshold);

        allAlpha = 0f;
        foreach (Renderer nightObject in nightObjects)
        {
            Color color = nightObject.material.color;
            color.a = Mathf.Clamp01(color.a + (daytime == Daytime.Day ? -1 : 1) * fadeSpeed * Time.deltaTime);
            allAlpha += color.a;
            nightObject.material.color = color;
        }

        flowerNight.SetActive((allAlpha / nightObjects.Count) > alphaThreshold);

        // flowerDay.transform.localScale = Vector3.Lerp(flowerDay.transform.localScale, daytime == Daytime.Day ? Vector3.one * targetScale : Vector3.zero, scaleSpeed * Time.deltaTime);
        // flowerNight.transform.localScale = Vector3.Lerp(flowerNight.transform.localScale, daytime == Daytime.Day ? Vector3.zero : Vector3.one * targetScale, scaleSpeed * Time.deltaTime);
    }

    private void ChangeFlower(Daytime daytime)
    {
        this.daytime = daytime;
        audioSource.Play();
        // if (daytime == Daytime.Day)
        // {
        //     flowerDay.SetActive(true);
        //     flowerNight.SetActive(false);
        // }
        // else
        // {
        //     flowerDay.SetActive(false);
        //     flowerNight.SetActive(true);
        // }
    }
}
