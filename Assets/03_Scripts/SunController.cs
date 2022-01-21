using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunController : MonoBehaviour
{
    [SerializeField] private Vector3 dayRotation;
    [SerializeField] private Vector3 nightRotation;
    [SerializeField] private float rotationSpeed = 1f;
    private Vector3 targetRotation;
    private void Start()
    {
        GameManager.instance.OnDaytimeChanged += OnDaytimeChanged;
    }

    private void Update()
    {
        if (transform.rotation.eulerAngles != targetRotation)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), rotationSpeed * Time.deltaTime);
    }

    private void OnDaytimeChanged(Daytime daytime)
    {
        targetRotation = daytime == Daytime.Day ? dayRotation : nightRotation;
    }
}
