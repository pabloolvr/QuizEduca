using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// classe reponsavel por administrar o sistema de pause do jogo
public class PausingSystem : MonoBehaviour {
    // objetos de UI do game
    public GameObject pauseMenuUI;
    public GameObject endGameMenuUI;
    public GameObject gameScreenUI;

    public static bool gameIsPaused; // controla o estado do jogo entre pausado/não pausado
    public static bool matchIsFinished; // controla o estado do jogo entre partida terminada/partida não terminada

    // Update is called once per frame
    void Update() {
        if (!matchIsFinished) {
            // quando todas as perguntas forem respondidas, muda o estado do jogo
            if (GameManager.Instance.questionIndexCounter == 16) {
                matchIsFinished = true;
            }
        } else {
            EndGame();
            GameManager.Instance.DisplayFinalGameData();
        }
    }

    // start é chamado antes da atualização do primeiro frame
    private void Start() {
        Time.timeScale = 1f;
        gameIsPaused = false;
        matchIsFinished = false;
    }

    // despausa game
    public void ResumeGame() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    // pausa game
    public void PauseGame() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    // finaliza o game
    private void EndGame() {
        endGameMenuUI.SetActive(true);
        gameScreenUI.SetActive(false);
    }
}
