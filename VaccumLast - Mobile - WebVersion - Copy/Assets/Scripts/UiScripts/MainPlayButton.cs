using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainPlayButton : MonoBehaviour
{
    public void LoadPlayScene()
    {
        SceneManager.LoadScene("Game");
    }
}
