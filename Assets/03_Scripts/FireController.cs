using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    [SerializeField] private ParticleSystem fire;
    [SerializeField] private Light fireLight;
    [SerializeField] private AudioSource fireAudio;
    [SerializeField] private float emissionRate = 20f;
    [SerializeField] private float emmissionSpeed = 1f;
    [SerializeField] private float intensity = 5f;
    [SerializeField] private float intensitySpeed = 1f;
    [SerializeField] private float audioVolume = 0.5f;
    [SerializeField] private float audioVolumeSpeed = 1f;
    [SerializeField] private bool randomOffset = true;
    float targetEmissionRate = 0f;
    float targetIntensity = 0f;
    float targetAudioVolume = 0f;
    public void Start()
    {
        GameManager.instance.OnDaytimeChanged += OnDaytimeChanged;

        if (randomOffset)
            StartCoroutine(StartSoundIn(Random.Range(0f, 1f)));

    }
    
    public void Update()
    {
        fire.emissionRate = Mathf.Lerp(fire.emissionRate, targetEmissionRate, emmissionSpeed * Time.deltaTime);
        fireLight.intensity = Mathf.Lerp(fireLight.intensity, targetIntensity, intensitySpeed * Time.deltaTime);
        fireAudio.volume = Mathf.Lerp(fireAudio.volume, targetAudioVolume, audioVolumeSpeed * Time.deltaTime);
    }

    private void OnDaytimeChanged(Daytime _daytime)
    {
        targetEmissionRate = _daytime == Daytime.Day ? 0f : emissionRate;
        targetIntensity = _daytime == Daytime.Day ? 0f : intensity;
        targetAudioVolume = _daytime == Daytime.Day ? 0f : audioVolume;
    }

    IEnumerator StartSoundIn(float _time)
    {
        yield return new WaitForSeconds(_time);
        fireAudio.Play();
    }
}
