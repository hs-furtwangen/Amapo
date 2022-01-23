using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Unity.Rendering.Universal;
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

    public GameObject fogBox;
    private Renderer fogBoxRender;
    public Vector4 DayColor = new Vector4(0.878f, 0.807f, 0.867f, 0.8831f);
    public Vector4 NightColor = new Vector4(0.278f, 0.207f, 0.267f, 0.97f);
    private Daytime _currentTime;

    private void Start()
    {
        fogBoxRender = fogBox.GetComponent<Renderer>();
        fogBoxRender.sharedMaterial.SetColor("Color_58E0201D", startDaytime == Daytime.Day ? DayColor : NightColor);

        GameManager.instance.OnDaytimeChanged += OnDaytimeChanged;
        OnDaytimeChanged(startDaytime);
    }

    private void Update()
    {
        if (sun.transform.rotation.eulerAngles != targetRotation)
            sun.transform.rotation = Quaternion.Lerp(sun.transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.deltaTime);

        if (sun.intensity != targetIntensity)
            sun.intensity = Mathf.Lerp(sun.intensity, targetIntensity, intensitySpeed * Time.deltaTime);

        Material mat = RenderSettings.skybox;
        mat.SetFloat("_SlideValue", Mathf.Lerp(mat.GetFloat("_SlideValue"), targetIntensity == dayIntensity ? 1f : 0f, Time.deltaTime * intensitySpeed));
        RenderSettings.skybox = mat;

        // Lerp fogboxColor
        var currentColor = fogBoxRender.sharedMaterial.GetVector("Color_58E0201D");
        var targetColor = _currentTime == Daytime.Day ? DayColor : NightColor;
        var tmpColor = new Vector4();

        if (currentColor.x != targetColor.x)
            tmpColor.x = Mathf.Lerp(currentColor.x, targetColor.x, intensitySpeed * Time.deltaTime);
        if (currentColor.y != targetColor.y)
            tmpColor.y = Mathf.Lerp(currentColor.y, targetColor.y, intensitySpeed * Time.deltaTime);
        if (currentColor.z != targetColor.z)
            tmpColor.z = Mathf.Lerp(currentColor.z, targetColor.z, intensitySpeed * Time.deltaTime);
        if (currentColor.w != targetColor.w)
            tmpColor.w = Mathf.Lerp(currentColor.w, targetColor.w, intensitySpeed * Time.deltaTime);

        fogBoxRender.sharedMaterial.SetVector("Color_58E0201D", tmpColor);
    }

    private void OnDaytimeChanged(Daytime _daytime)
    {
        targetRotation = _daytime == Daytime.Day ? dayRotation : nightRotation;
        targetIntensity = _daytime == Daytime.Day ? dayIntensity : nightIntensity;
        _currentTime = _daytime;
    }
}
