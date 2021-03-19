/*
Autor: Pablo Ernani Nogueira de Oliveira - 11215702
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// classe responsavel por administrar as cenas do jogo
public class UIManager : MonoBehaviour {
	// carrega cena do jogo
	public void LoadScene(string sceneName) {
		SceneManager.LoadSceneAsync(sceneName);
	}

	// quit game
	public void ExitGame() {
		Application.Quit();
	}
}
