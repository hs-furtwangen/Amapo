using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Goal : MonoBehaviour
{
    public event Action OnAllPlayerOnGoal;
    [SerializeField] private List<PlayerController> characterOnGoal;

    private void OnTriggerEnter(Collider other)
    {
        List<PlayerController> characters = GameManager.instance.GetCharacters();

        foreach(PlayerController character in characters) {
            if(character.gameObject == other.gameObject) {
                characterOnGoal.Add(character);
            }
        }

        if(characterOnGoal.Count == characters.Count)
            OnAllPlayerOnGoal?.Invoke();
    }

    private void OnTriggerExit(Collider other) {
        foreach(PlayerController character in characterOnGoal) {
            if(character.gameObject == other.gameObject) {
                characterOnGoal.Remove(character);
            }
        }
    }
}
