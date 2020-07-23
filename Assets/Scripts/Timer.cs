using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// classe 
public class Timer : MonoBehaviour {
    private float timeCounter; // contador de tempo do timer
    private bool timerIsRunning = false; // controla se o timer está sendo executado

    // Update is called once per frame
    void Update() {
        if (timerIsRunning) {
            if (timeCounter > 0) {
                //AudioManager.Instance.PlaySound("TimerTickSFX"); // toca tick do timer
                timeCounter -= Time.deltaTime;
            } else {
                timerIsRunning = false;
                timeCounter = 0;
                GameManager.Instance.GetNextQuestion();
            }
        }
    }
    // dá inicio ao timer com um tempo em segundos igual a 'time'
    public void SetTimer (int time) {
        timeCounter = time;
        timerIsRunning = true;
    }
    // obtem o tempo atual (em inteiro) do timer
    public int GetTime() {
        return (int)timeCounter;
    }
}
