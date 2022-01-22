using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Trigger))]
public class PlatformButtom : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject knob;
    [SerializeField, ColorUsage(true, true)] private Color targetEmission;

    private void Start() {
        GetComponent<Trigger>().OnEnter += BecomeActive;
    }

    private void BecomeActive() {
        knob.SetActive(true);
        audioSource.Play();
        knob.GetComponent<Renderer>().material.SetColor("_EmissionColor", targetEmission);
    }
}
