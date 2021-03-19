/*
Autor: Pablo Ernani Nogueira de Oliveira - 11215702
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// classe responsável por administrar dados e elementos de UI do jogo principal
public class GameManager : MonoBehaviour {
    public Timer timer; // objeto relacionado ao cronometro
    public static GameManager Instance;
    // objetos relacionados ao elementos de UI do game
    public Button pergunta;
    public Button alternativa1;
    public Button alternativa2;
    public Button alternativa3;
    public Button alternativa4;
    public Button timerBox;
    public Text score;
    public Text correctAnswers;
    public Text incorrectAnswers;
    public Text timerText;
    public Text finalScore;
    public Text finalCorrectAnswers;
    public Text finalIncorrectAnswers;
    // variaveis para armazenamento dos dados do game princpal
    public int questionIndexCounter; // armazena o index da pergunta armazenada na struct Questionary que será mostrada ao jogador
    private bool questionAnswered;  // controla se a questão sendo mostrada foi respondida ou não
    private int scoreVal; // armazena o valor da pontuação do jogador
    private int correctlyAnswered; // armazena o numero de perguntas respondidas corretamente
    private int incorrectlyAnswered; // armazena o numero de perguntas respondidas incorretamente
    private int minTime; // armazena o valor do tempo (em segundos) que o timer deverá atingir para que o som de tick começe a ser tocado a cada segundo
    private int tickPlayed; // armazena a quantidade de vezes que o tick foi tocado a partir do momento que o timer atingiu minTime

    // função chamada logo antes da função Start ser chamada
    void Awake() {
        // inicialização das variáveis
        Instance = this;
        questionIndexCounter = 0;
        questionAnswered = false;
        scoreVal = 0;
        correctlyAnswered = 0;
        incorrectlyAnswered = 0;
        minTime = 10;
        // adiciona, para cada botão das alternativas, um listener que executara a funcao ChangeScore
        alternativa1.onClick.AddListener(delegate { ChangeScore(1); });
        alternativa2.onClick.AddListener(delegate { ChangeScore(2); });
        alternativa3.onClick.AddListener(delegate { ChangeScore(3); });
        alternativa4.onClick.AddListener(delegate { ChangeScore(4); });
    }

    // start é chamado antes da atualização do primeiro frame
    void Start() {
        // obtem perguntas do Questionary
        GetQuestion(questionIndexCounter);
        timer.SetTimer(31); // define o cronometro com 30 segundos
        tickPlayed = 0;
        questionIndexCounter++;
        DisplayGameData();
    }

    // Update is called once per frame
    void Update() {
        DisplayGameData();
        // mostra nova pergunta assim que a pergunta anterior for respondida
        if (questionAnswered) {
            questionAnswered = false;
            if (questionIndexCounter < 15) {
                GetQuestion(questionIndexCounter);
                timer.SetTimer(31); // define o cronometro com 30 segundos
                tickPlayed = 0;
                Debug.Log("IndexQuestion: " + questionIndexCounter);
                Debug.Log("Resposta: " + QuestionManager.Questionary[questionIndexCounter].CorrectAlternativeIndex);
            }
            questionIndexCounter++;
        }
        // caso o timer chegue a minTime, a cor do cronômetro na tela muda pra vermelho
        // e começa a tocar um som de tick a cada segundo corrido
        if (timer.GetTime() <= minTime) {
            // a partir do momento que um tick é tocado, o próximo só tocará novamente após 1 segundo
            if (minTime - tickPlayed == timer.GetTime()) {
                AudioManager.Instance.PlaySound("TimerTickSFX"); // toca tick do timer
                tickPlayed++; // acrescenta o numero de ticks tocados
                timerBox.GetComponent<Image>().color = new Color32(215, 45, 45, 255); // muda a cor do Timer para vermelho
            }
        } else {
            timerBox.GetComponent<Image>().color = new Color32(255, 255, 255, 255); // muda a cor do Timer para branco
        }
    }

    // funcao que obtem a proxima questão e mostra ela na tela
    // além disso, ela marca a questão anterior como respondida incorretamente
    // ela é chamada quando o tempo de resposta é excedido
    public void GetNextQuestion() {
        if (questionIndexCounter < 15) {
            GetQuestion(questionIndexCounter);
            timer.SetTimer(31); // define o cronometro com 30 segundos
            tickPlayed = 0;
            UpdateIncorrectlyAnswered();
        }
        questionIndexCounter++;
    }

    // mostra os dados no fim da partida na tela de EndGame
    public void DisplayFinalGameData() {
        finalScore.text = "" + scoreVal;
        finalCorrectAnswers.text = "Acertos: " + correctlyAnswered;
        finalIncorrectAnswers.text = "Erros: " + incorrectlyAnswered;
    }

    // mostra os dados da partida na tela do jogo
    private void DisplayGameData() {
        score.text = "   " + scoreVal;
        correctAnswers.text = "     " + correctlyAnswered;
        incorrectAnswers.text = "     " + incorrectlyAnswered;
        timerText.text = timer.GetTime().ToString();
    }

    // obtem pergunta de indice igual a QuestionIndex do questionario para mostra-la na tela do jogo
    private void GetQuestion(int QuestionIndex) {
        pergunta.transform.GetComponentInChildren<Text>().text = QuestionManager.Questionary[QuestionIndex].Question;
        alternativa1.transform.GetComponentInChildren<Text>().text = QuestionManager.Questionary[QuestionIndex].Alternative1;
        alternativa2.transform.GetComponentInChildren<Text>().text = QuestionManager.Questionary[QuestionIndex].Alternative2;
        alternativa3.transform.GetComponentInChildren<Text>().text = QuestionManager.Questionary[QuestionIndex].Alternative3;
        alternativa4.transform.GetComponentInChildren<Text>().text = QuestionManager.Questionary[QuestionIndex].Alternative4;
    }

    // método que é executado sempre que um botão de alternativa cujo indice é AlternativeIndex é clicado
    // caso a alternativa selecionada seja a resposta certa, soma-se pontos baseado na dificuldade da pergunta
    private void ChangeScore(int AlternativeIndex) {
        questionAnswered = true;
        // verifica se a alternativa selecionada é a correta
        if (AlternativeIndex == QuestionManager.Questionary[questionIndexCounter - 1].CorrectAlternativeIndex) {
            // soma pontos no score baseado no tempo que o jogador levou para responder
            if (QuestionManager.Questionary[questionIndexCounter - 1].Difficulty == 0) { // caso seja pergunta facil
                scoreVal += timer.GetTime() * 3; // soma pont
            } else if (QuestionManager.Questionary[questionIndexCounter - 1].Difficulty == 1) { // caso seja pergunta media
                scoreVal += timer.GetTime() * 5;
            } else { // caso seja pergunta dificil
                scoreVal += timer.GetTime() * 7;
            }
            UpdateCorrectlyAnswered();
        } else {
            UpdateIncorrectlyAnswered();
        }        
    }

    // incrementa 'correctlyAnswered' e ativa um som
    private void UpdateCorrectlyAnswered() {
        correctlyAnswered++;
        AudioManager.Instance.PlaySound("CorrectlyAnsweredSFX"); // toca som de pergunta respondida corretamente
    }

    // incrementa 'incorrectlyAnswered' e ativa um som
    private void UpdateIncorrectlyAnswered() {
        incorrectlyAnswered++;
        AudioManager.Instance.PlaySound("IncorrectlyAnsweredSFX"); // toca som de pergunta respondida incorretamente
    }
}
