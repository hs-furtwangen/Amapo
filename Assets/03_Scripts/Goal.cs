using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Goal : MonoBehaviour
{
    public event Action OnAllPlayerOnGoal;
    [SerializeField] private List<PlayerController> characterOnGoal;

    private void OnTriggerEnter(Collider _other)
    {
        try
        {
            List<PlayerController> characters = GameManager.instance.GetCharacters();

            foreach (PlayerController character in characters)
            {
                if (character.gameObject == _other.gameObject)
                    characterOnGoal.Add(character);
            }

            if (characterOnGoal.Count == characters.Count)
                OnAllPlayerOnGoal?.Invoke();
        }
        catch (Exception e) { Debug.Log(e); }
    }

    private void OnTriggerExit(Collider _other)
    {
        try
        {
            foreach (PlayerController character in characterOnGoal)
            {
                if (character.gameObject == _other.gameObject)
                    characterOnGoal.Remove(character);
            }
        }
        catch (Exception e) { Debug.Log(e); }
    }
}
