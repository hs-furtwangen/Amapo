using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Trigger : MonoBehaviour
{
    public event Action OnEnter;
    [SerializeField] private bool isOneTime = false;
    [SerializeField] private Collider coll;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isOneTime && coll)
                coll.enabled = false;

            OnEnter?.Invoke();
        }
    }
}
