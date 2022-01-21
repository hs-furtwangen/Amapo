using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveClouds : MonoBehaviour
{
    /// <summary>
    /// Random offset for cos oscillation
    /// </summary>
    public float PhaseOffset { get; private set; }

    /// <summary>
    /// Dampening of oscillation (amplitude)
    /// </summary>
    [Min(float.Epsilon)]
    public float AmplitudeScale;

    void Start()
    {
        PhaseOffset = Random.value * Mathf.PI;
        AmplitudeScale = 0.025f;
    }


    void FixedUpdate()
    {
        transform.position =
            new Vector3(
                transform.position.x,
                transform.position.y + (Mathf.Cos(Time.time + PhaseOffset) * AmplitudeScale),
                transform.position.z
            );
    }
}
