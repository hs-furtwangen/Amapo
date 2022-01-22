using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] private ParticleSystem fire;
    [SerializeField] private Light fireLight;
    [SerializeField] private float emissionRate = 20f;
    [SerializeField] private float emmissionSpeed = 1f;
    [SerializeField] private float intensity = 5f;
    [SerializeField] private float intensitySpeed = 1f;
    float targetEmissionRate = 0f;
    float targetIntensity = 0f;
    public void Start()
    {
        GameManager.instance.OnDaytimeChanged += OnDaytimeChanged;
    }
    
    public void Update()
    {
        fire.emissionRate = Mathf.Lerp(fire.emissionRate, targetEmissionRate, emmissionSpeed * Time.deltaTime);
        fireLight.intensity = Mathf.Lerp(fireLight.intensity, targetIntensity, intensitySpeed * Time.deltaTime);
    }

    private void OnDaytimeChanged(Daytime daytime)
    {
        targetEmissionRate = daytime == Daytime.Day ? 0f : emissionRate;
        targetIntensity = daytime == Daytime.Day ? 0f : intensity;
    }
}
