using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Rendering.Universal;
public class SunController : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private Daytime startDaytime;
    [SerializeField] private Vector3 dayRotation;
    [SerializeField] private float dayIntensity;
    [SerializeField] private Vector3 nightRotation;
    [SerializeField] private float nightIntensity;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private float intensitySpeed = 1f;
    private Vector3 targetRotation;
    private float targetIntensity;
    private void Start()
    {
        GameManager.instance.OnDaytimeChanged += OnDaytimeChanged;

        OnDaytimeChanged(startDaytime);
    }

    private void Update()
    {
        if (sun.transform.rotation.eulerAngles != targetRotation)
            sun.transform.rotation = Quaternion.Lerp(sun.transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.deltaTime);

        if (sun.intensity != targetIntensity)
            sun.intensity = Mathf.Lerp(sun.intensity, targetIntensity, intensitySpeed * Time.deltaTime);
    }

    private void OnDaytimeChanged(Daytime _daytime)
    {
        targetRotation = _daytime == Daytime.Day ? dayRotation : nightRotation;
        targetIntensity = _daytime == Daytime.Day ? dayIntensity : nightIntensity;
    }
}
