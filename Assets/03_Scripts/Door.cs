using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour
{
    [SerializeField] private Trigger trigger;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float waitTillOpen = 0.3f;
    private bool isOpen = false;
    private void Start()
    {
        trigger.OnEnter += Open;
    }

    private void Open()
    {
        if (isOpen)
            return;

        isOpen = true;
        trigger.OnEnter -= Open;
        StartCoroutine(OpenDoor());
    }

    IEnumerator OpenDoor()
    {
        yield return new WaitForSeconds(waitTillOpen);
        GetComponent<Animator>()?.SetTrigger("Open");
        audioSource.Play();
    }
}
