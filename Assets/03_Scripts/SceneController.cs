using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    public void StartLevel(int _level)
    {
        SceneManager.LoadScene(_level);
    }
}
