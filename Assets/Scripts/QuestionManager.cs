using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// struct para armazenar dados das perguntas
public struct Questions {
    public int Difficulty; // recebe 0 para facil, 1 para medio e 2 para dificil
    public string Question;
    public int CorrectAlternativeIndex;
    public string Alternative1;
    public string Alternative2;
    public string Alternative3;
    public string Alternative4;
}

// classe responsavel por sortear e armazenar as perguntas dos bancos de questoes
public class QuestionManager : MonoBehaviour {
    public TextAsset PerguntasDificeis;
    public TextAsset PerguntasMedias;
    public TextAsset PerguntasFaceis;

    public static Questions[] Questionary;

    static Random Randomizer = new Random();

    // start é chamado antes da atualização do primeiro frame
    void Start() {
        Debug.Log("Inicio do questionManager");
        CreateQuestionary();
    }

    // cria o questionario de perguntas do quiz que armazena 15 perguntas
    // dessas 15 perguntas, 5 são fáceis, 5 médias e 5 difíceis
    void CreateQuestionary() {
        Questionary = new Questions[15];
        // armazena o index das perguntas escolhidas de cada dificuldade
        List<int> HardQuestionsIndexes = new List<int>();
        List<int> MediumQuestionsIndexes = new List<int>();
        List<int> EasyQuestionsIndexes = new List<int>();
        // armazena a sequencia de indexes do Questionary em que foram armazenadas uma pergunta
        List<int> QuestionaryIndexes = new List<int>();
        // obtem e armazena 5 numeros aleatorios distintos de 1 a 20, que irao servir de index number, para cada tipo de questão
        int number;
        for (int i = 0; i < 5; i++) {
            // seleciona HardQuestionsIndexes do banco de questoes dificeis
            number = Random.Range(1, 21);
            while (HardQuestionsIndexes.Contains(number))
                number = Random.Range(1, 21);
            HardQuestionsIndexes.Add(number);
            // seleciona MediumQuestionsIndexes do banco de questoes médias
            number = Random.Range(1, 21);
            while (MediumQuestionsIndexes.Contains(number))
                number = Random.Range(1, 21);
            MediumQuestionsIndexes.Add(number);
            // seleciona EasyQuestionsIndexes do banco de questoes faceis
            number = Random.Range(1, 21);
            while (EasyQuestionsIndexes.Contains(number))
                number = Random.Range(1, 21);
            EasyQuestionsIndexes.Add(number);
        }
        // armazena o banco de questões de cada dificuldade em uma string
        string PerguntasDificeisString = PerguntasDificeis.text;
        string PerguntasMediasString = PerguntasMedias.text;
        string PerguntasFaceisString = PerguntasFaceis.text;
        // faz leitura das perguntas selecionadas para armazenar em Questionary
        foreach (int index in EasyQuestionsIndexes) {
            StoreQuestion(index, PerguntasFaceisString, 0, QuestionaryIndexes);
        } foreach (int index in MediumQuestionsIndexes) {
            StoreQuestion(index, PerguntasMediasString, 1, QuestionaryIndexes);
        } foreach (int index in HardQuestionsIndexes) {
            StoreQuestion(index, PerguntasDificeisString, 2, QuestionaryIndexes);
        }
    }

    // recebe o index de uma pergunta, que vai de 1 a 20, do arquivo do banco de questoes cujo conteudo está em QuestionList
    // recebe o valor de dificuldade dessa pergunta, onde 0 é fácil, 1 é médio e 2 é difícil
    // por fim, essa função armazena os dados da pergunta no Questionary
    private void StoreQuestion(int questionIndex, string QuestionList, int difficultyValue, List<int> QuestionaryList) {
        // obtem uma posicao de Questionary aleatoria para armazenar a pergunta atual
        int IndexQuestionary = Random.Range(0, 15);
        // verifica se a posição 'IndexQuestionary' do Questionary já está ocupada com uma pergunta
        while (QuestionaryList.Contains(IndexQuestionary)) {
            IndexQuestionary = Random.Range(0, 15);
        }
        QuestionaryList.Add(IndexQuestionary);
        // obtem do arquivo a pergunta com index dado
        Questionary[IndexQuestionary].Question = GetQuestion(questionIndex, QuestionList);
        // obtem do arquivo o index da alternativa com a resposta correta
        Questionary[IndexQuestionary].CorrectAlternativeIndex = GetCorrectAlternativeIndex(questionIndex, QuestionList);
        // obtem do arquivo as alternativas da pergunta com index dado
        Questionary[IndexQuestionary].Alternative1 = GetAlternative(questionIndex, QuestionList, 1);
        Questionary[IndexQuestionary].Alternative2 = GetAlternative(questionIndex, QuestionList, 2);
        Questionary[IndexQuestionary].Alternative3 = GetAlternative(questionIndex, QuestionList, 3);
        Questionary[IndexQuestionary].Alternative4 = GetAlternative(questionIndex, QuestionList, 4);
        // define dificuldade da pergunta
        Questionary[IndexQuestionary].Difficulty = difficultyValue;
    }

    // recebe o index de uma pergunta no arquivo, que vai de 1 a 20, e retorna o conteudo dessa pergunta em uma string
    private string GetQuestion(int QuestionIndex, string QuestionList) {
        // armazena a string num array de char para facilitar manipulação
        char[] Questions = QuestionList.ToCharArray();
        int contadorIndex = 0;
        int IndexString = 0;
        // laco que busca o index do array em que se inicia a pergunta buscada
        while (contadorIndex < QuestionIndex) {
            // caso encontre um delimitador de inicio de pergunta (achou a pergunta buscada)
            if (Questions[IndexString] == '<') {
                contadorIndex++;
            }
            IndexString++;
        }
        int IndexInicial = IndexString; // armazena o index do array em que se inicia a pergunta buscada
        int IndexFinal; // armazena o index do array em que se termina a pergunta buscada
        // laço que busca o index que merca o término da pergunta
        while (Questions[IndexString] != '>') {
            IndexString++;
        }
        IndexFinal = IndexString;
        // retorna em uma string a pergunta desejada
        return QuestionList.Substring(IndexInicial, IndexFinal - IndexInicial);
    }

    // recebe o index de uma pergunta no arquivo, que vai de 1 a 20, armazenada numa string com a lista de perguntas
    // recebe o index de uma alternativa dessa pergunta, que vai de 1 a 4
    // retorna em uma string o conteudo da alternativa, identificada por 'AlternativeIndex', que pertence a pergunta identificada por 'QuestionIndex'
    private string GetAlternative(int QuestionIndex, string QuestionList, int AlternativeIndex) {
        // armazena a string num array de char para facilitar manipulação
        char[] Questions = QuestionList.ToCharArray();
        int contadorQuestionIndex = 0;
        int IndexString = 0;
        // laco que busca o index do array em que se finaliza a pergunta buscada
        while (contadorQuestionIndex < QuestionIndex) {
            // caso encontre um delimitador de fim de pergunta (achou a pergunta buscada)
            if (Questions[IndexString] == '>') {
                contadorQuestionIndex++;
            }
            IndexString++;
        }

        int IndexInicial = IndexString; // armazena o index do array em que se inicia a alternativa
        int IndexFinal; // armazena o index do array em que se termina a alternativa buscada
        int contadorAlternativeIndex = 0; // conta quantas perguntas foram achadas e verificadas
        while (true) {
            // caso ache uma alternativa
            if (Questions[IndexString] == '@' || Questions[IndexString] == '&') {
                contadorAlternativeIndex++;
                IndexFinal = IndexString;
                if (contadorAlternativeIndex != AlternativeIndex) { // verifica se a pergunta achada é a buscada
                    IndexInicial = IndexString + 1;
                } else {
                    break;
                }    
            }
            IndexString++;
        }
        // retorna em uma string a alternativa desejada
        return QuestionList.Substring(IndexInicial, IndexFinal - IndexInicial);
    }

    // recebe o index de uma pergunta no arquivo, que vai de 1 a 20, e uma string com a lista de perguntas
    // retorna o index da alternativa correta
    private int GetCorrectAlternativeIndex(int QuestionIndex, string QuestionList) {
        char[] Questions = QuestionList.ToCharArray();
        int contadorQuestionIndex = 0;
        int IndexString = 0;
        while (contadorQuestionIndex < QuestionIndex) {
            if (Questions[IndexString] == '>')
            {
                contadorQuestionIndex++;
            }
            IndexString++;
        }
        // faz a busca da alternativa correta
        int AlternativeIndex = 0;
        while (true) {         
            if (Questions[IndexString] == '&') { // caso acha uma alternativa incorreta
                AlternativeIndex++;
            } else if (Questions[IndexString] == '@') { // caso acha uma alternativa correta
                AlternativeIndex++;
                return AlternativeIndex;
            }
            IndexString++;
        }
    }
}
