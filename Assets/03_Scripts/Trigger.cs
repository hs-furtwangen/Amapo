using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Trigger : MonoBehaviour
{
    public event Action OnEnter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            OnEnter?.Invoke();
    }
}
