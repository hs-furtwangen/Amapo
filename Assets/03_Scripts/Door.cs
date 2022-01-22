using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Trigger trigger;
    [SerializeField] private GameObject doorClosed;
    bool isOpen = false;
    private void Start()
    {
        trigger.OnEnter += Open;
    }

    private void Open()
    {
        if (isOpen)
            return;

        doorClosed.SetActive(false);
        isOpen = true;
        trigger.OnEnter -= Open;
    }
}
