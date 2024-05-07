using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUi : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TittleResultText;
    [SerializeField] private TextMeshProUGUI ScoreText;

    public void SetGameOverScreen(bool iswin, int score)
    {
        if (iswin)
        {
            TittleResultText.text = "Finished";
        }
        else
        {
            TittleResultText.text = "Game Over";
        }
        ScoreText.text = score.ToString();
    }
}
