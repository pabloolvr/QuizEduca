/*
Autor: Pablo Ernani Nogueira de Oliveira - 11215702
*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// struct para manipulação de parametros do som
[System.Serializable()]
public struct SoundParameters {
    [Range(0, 1)]
    public float Volume;
    [Range(-3, 3)]
    public float Pitch;
    public bool Loop;
}

// classe para armazenar os dados de som
[System.Serializable()]
public class Sound {
    [SerializeField] string name = string.Empty;
    public string Name { get { return name; } }

    [SerializeField] AudioClip clip = null;
    public AudioClip Clip { get { return clip; } }

    [SerializeField] SoundParameters parameters = new SoundParameters();
    public SoundParameters Parameters { get { return parameters; } }

    public AudioSource Source = null;

    // toca audio do tipo Sound
    public void Play() {
        Source.clip = Clip;
        Source.volume = Parameters.Volume;
        Source.pitch = Parameters.Pitch;
        Source.loop = Parameters.Loop;

        Source.Play();
    }
    // para o audio do tipo Sound
    public void Stop() {
        Source.Stop();
    }
}

// objeto responsavel por administrar dados de som e musica do jogo
public class AudioManager : MonoBehaviour {
    public static AudioManager Instance = null; // instancia estatic de AudioManager que permite ser acessado de qualquer outro script
    [SerializeField] Sound[] sounds = null; // array para armazena os sons do sistema de audio do jogo
    [SerializeField] AudioSource soundsSource = null; // referecia para origem do audio
    [SerializeField] string startUpTrack = null; // nome da musica que começa a tocar inicialmente

    // função chamada logo antes da função Start ser chamada
    void Awake() {
        if (Instance != null) { // caso ja exista uma instancia de AudioManager 
            Destroy(gameObject); // destroi essa instancia pre existente
        } else { // caso ainda nao exista uma instancia de AudioManager
            Instance = this; // instancia para AudioManager
            DontDestroyOnLoad(gameObject); // garante que AudioManager nao seja destruido ao carregar um scene
        }
        InitSounds();
    }

    // Start é chamado antes da atualização do primeiro frame
    void Start() {
        if (string.IsNullOrEmpty(startUpTrack) != true) { // caso startupTrack nao seja null ou vazio
            PlaySound(startUpTrack);
        }
    }

    // preenche o array de Sound 'sound[]'
    private void InitSounds() {
        foreach (Sound sound in sounds) {
            AudioSource source = (AudioSource)Instantiate(soundsSource, gameObject.transform);
            source.name = sound.Name;

            sound.Source = source;
        }
    }

    // toca um som identificado por 'name'
    public void PlaySound(string name) {
        Sound sound = GetSound(name);
        if (sound != null) {
            sound.Play();
        }
    }

    // para um som, identificado por 'name', que está sendo tocado
    public void StopSound(string name) {
        Sound sound = GetSound(name);
        if (sound != null) {
            sound.Stop();
        }
    }

    // obtem um som, identificado por 'name', do array de Sounds 
    Sound GetSound(string name) {
        foreach (Sound sound in sounds) {
            if (sound.Name == name) {
                return sound;
            }
        }
        return null;
    }
}
