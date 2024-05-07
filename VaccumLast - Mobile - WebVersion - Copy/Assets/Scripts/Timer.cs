using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float startTime = 0;
    private float MaxTimeGiven = 18f;
    //private float MaxTimeGiven = 30f;
    [SerializeField] private Slider slider;
    private float pausedTime;
    public float maxTimeGiven
    {
        get { return MaxTimeGiven; }
    }
    public bool gameStarted = true;

    private float ElapsedTime;
    public float elapsedTime
    {
        get { return ElapsedTime; }
    }

    [SerializeField] private MapCreation mapCreation;

  /*  void Start()
    {
       // StartTimer();
    }*/

    void Update()
    {
        if (gameStarted)
        {
            float ElapsedTime = Time.time - startTime;

            float ratio = 1 - (ElapsedTime / MaxTimeGiven);

            ratio = Mathf.Clamp01(ratio);

            slider.value = ratio;

            if (ElapsedTime >= MaxTimeGiven)
            {
                StopTimer();
                mapCreation.NoTime();
            }
        }
    }

    public void StartTimer(int maxtime)
    {
        MaxTimeGiven = maxtime;
        startTime = Time.time;
        gameStarted = true;
        Debug.Log("Timer started.");
    }

    public void StopTimer()
    {
        gameStarted = false;
        Debug.Log("Timer stopped.");
    }
    public void AddTIme(float addintime)
    {
       // MaxTimeGiven += addintime;
    }
    public void ChangeTimerEnabpled(bool IsTimerOn)
    {
        gameStarted = IsTimerOn;
    }
    public void PauseTimer()
    {
        pausedTime += Time.time - startTime; // Add the time passed since the timer started to pausedTime
        gameStarted = false;
    }

    // Call this function to resume the timer
    public void ResumeTimer()
    {
        startTime = Time.time;
        gameStarted = true;
    }
/*    public void PauseTimer()
    {
        Time.timeScale = 0;
        gameStarted = false;
    }

    // Call this function to resume the timer
    public void ResumeTimer()
    {
        Time.timeScale = 1;
        gameStarted = true;
    }*/
}
