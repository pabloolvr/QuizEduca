using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// classe responsavel por cuidar dos objetos relacionados às configurações de áudio e tela do jogo
public class SettingsManager : MonoBehaviour {
    public static SettingsManager Instance = null; // referencial static de settingsManager
    public AudioMixer audioMixer; // referencia para o audioMixer do game para permitir que o som seja alterado
    public Dropdown resolutionDropdown; // referencia ao dropDown presente nas configurações do menu principal
    public Dropdown resolutionDropdown2; // referencia ao dropDown presente nas configurações do menu de pause
    public Slider volumeSlider; // referencia ao volumeSlider presente nas configurações do menu principal
    public Slider volumeSlider2; // referencia ao volumeSlider presente nas configurações do menu de pause
    public Toggle FullscreenToggle; // referencia ao toggle de fullscreen presente nas configurações do menu principal
    public Toggle FullscreenToggle2; // referencia ao toggle de fullscreen presente nas configurações do menu de pause

    static Resolution[] resolutions; // array de resoluções disponíveis para o game

    // altera a resolução do game para a resolução que foi selecionada no dropDown do menu, identificada por ResolutionIndex
    public void SetResolution(int ResolutionIndex) {
        Resolution resolution = resolutions[ResolutionIndex]; // leitura da resolução selecionada
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); // alteração da resolução do game do jogador
    }

    // altera o volume do som principal do jogo através do audioMixer
    public void SetVolume(float volume) {
        audioMixer.SetFloat("Volume", (float)Math.Log10(volume) * 20); // altera o volume através do audioMixer
        SettingsValues.volumeValue = volume;
    }

    // altera o tamanho da tela do game para fullscreen
    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
        SettingsValues.toggleValue = isFullscreen;
    }

    // função chamada quando a tela de configurações é acessada
    public void SettingsScreenAccessed(bool fromMenu) {
        if (fromMenu) { // caso a tela de configurações é acessada pelo menu principal
            Debug.Log("acessou pelo menu principal");
            GetDropDownValues(resolutionDropdown);
            GetVolumeValue(volumeSlider);
            GetToggleValue(FullscreenToggle);
        } else { // caso a tela de configurações é acessada pelo menu de pause
            Debug.Log("acessou pelo menu de pause");
            GetDropDownValues(resolutionDropdown2);
            GetVolumeValue(volumeSlider2);
            GetToggleValue(FullscreenToggle2);
        }
    }

    // obtem os valores de resolução de tela suportadas pelo monitor do usuario e as armazenam no dropDown referenciado
    public void GetDropDownValues(Dropdown dropdown) {
        int CurrentResolutionIndex = 0; // armazena o index da resolução atual do usuário, que está no array 'resolutions' 
        resolutions = Screen.resolutions; // faz leitura de todas as resoluções suportadas pelo monitor do usuário
        // limpa as opções padrões do dropDown do unity
        dropdown.ClearOptions();
        // lista de opcoes do dropDown
        List<string> options = new List<string>(); 
        // armazena os dados do array 'resolutions' em uma lista de strings
        for (int i = 0; i < resolutions.Length; i++) {
            // lê resolução e armazena na lista 'options'
            string option = resolutions[i].width + " x " + resolutions[i].height;
            // verifica se a opcao a ser inserida no dropDown já existe na lista do dropDown
            if (!options.Contains(option)) {
                options.Add(option);
                // verifica se a resolução atual do usuário é a resolução que acabou de ser lida
                if (resolutions[i].Equals(Screen.currentResolution)) {
                    CurrentResolutionIndex = i;
                }
            }
        }
        dropdown.AddOptions(options); // adiciona a lista de strings para o dropDown do jogo
        // define a opção inicial do dropDown como sendo a que armazena a resolução que o usuário está usando no momento
        dropdown.value = CurrentResolutionIndex;
        dropdown.RefreshShownValue();
    }

    // retorna o valor atual do volumeSlider da tela de configurações
    public void GetVolumeValue(Slider volumeSlider) {
        Debug.Log(SettingsValues.volumeValue);
        volumeSlider.value = (float)SettingsValues.volumeValue;
    }

    // retorna o valor atual do toggle de fullscreen da tela de configurações
    public void GetToggleValue(Toggle fullscreenToggle) {
        fullscreenToggle.isOn = SettingsValues.toggleValue;
    }
}