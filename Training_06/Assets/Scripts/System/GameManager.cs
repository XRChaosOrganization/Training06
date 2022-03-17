using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class Questions
{
    public int difficulty;
    public string theme;
    public string intitule;
    public string[] reponses = new string[4];
}
public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    public Transform startLevel;
    public GameObject levelPrefab;
    public TextAsset questionsData;
    TextMeshPro questionDisplayText;
    TextMeshPro timerDisplayText;
    Transform trappesContainer;
    private List<Questions> questions1;
    private List<Questions> questions2;
    private List<Questions> questions3;
    private int nombreQuestionsposées;
    GameObject currentLevel;
    public int nombreQuestionsParPhase = 6;
    public float timePerLevel = 20;
    public List<PlayerComponent> playerList;
    public List<GameObject> trappes;
    float timer;
    bool isTimerEnabled;

    private void Awake()
    {
        gm = this;
        ConvertCSV();
    }
    
    private void Update()
    {
        if (isTimerEnabled)
        {
            timerDisplayText.text = string.Format("{0:00}", timer);
            timer -= Time.deltaTime;
            if (timer<=0.2f)
            {
                trappes.Clear();
                Destroy(currentLevel);
            }
            if (timer <=0)
            {
                isTimerEnabled = false;
                EndLevel();
            }
        }
    }
    void EndLevel()
    {
        
        foreach (PlayerComponent _player in playerList)
        {
            if (_player.isRight)
            {
                Debug.Log("Bonne réponse");
            }
            else if (_player.isWrong)
            {
                Debug.Log("Mauvaise réponse");
            }
            else
            {
                Debug.Log("Pas de réponse");
            }
        }
        LoadLevel();
    }
    
    public void LoadLevel()
    {
        timer = timePerLevel;
        if (nombreQuestionsposées ==0)
        {
            Destroy(startLevel.gameObject);
        }
        currentLevel=(GameObject) Instantiate(levelPrefab);
        trappesContainer = currentLevel.GetComponentInChildren<Transform>().Find("Trappes");
        questionDisplayText = GameObject.Find("QuestionDisplay").GetComponent<TextMeshPro>();
        timerDisplayText = GameObject.Find("TimerDisplay").GetComponent<TextMeshPro>();
        
        foreach (Transform _trappes in trappesContainer)
        {
            trappes.Add(_trappes.gameObject);
        }
        RandomizeQuestionAndAnswers();
    }
    void RandomizeQuestionAndAnswers()
    {
        List<int> randomizator = new List<int>();
        while (randomizator.Count <4)
        {
            int r = UnityEngine.Random.Range(0, 4);
            if (randomizator.Contains(r))
            {
                continue;
            }
            else
            {
                randomizator.Add(r);
            }
        }
        switch (nombreQuestionsposées)
        {
            case int n when (n < nombreQuestionsParPhase):
                int r1 = UnityEngine.Random.Range(0, questions1.Count);
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        trappes[randomizator[i]].GetComponentInChildren<TextMeshPro>().text = questions1[r1].reponses[i];
                        trappes[randomizator[i]].GetComponentInChildren<BoxCollider>().tag = "Valid";
                    }
                    else
                    {
                        trappes[randomizator[i]].GetComponentInChildren<TextMeshPro>().text = questions1[r1].reponses[i];
                        trappes[randomizator[i]].GetComponentInChildren<BoxCollider>().tag = "Unvalid";
                    }
                }
                questionDisplayText.text = questions1[r1].intitule;
                questions1.Remove(questions1[r1]);
                
                nombreQuestionsposées++;
                break;
            case int n when (n < 2*nombreQuestionsParPhase):
                int r2 = UnityEngine.Random.Range(0, questions2.Count);
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        trappes[randomizator[i]].GetComponentInChildren<TextMeshPro>().text = questions2[r2].reponses[i];
                        trappes[randomizator[i]].GetComponentInChildren<BoxCollider>().tag = "Valid";
                    }
                    else
                    {
                        trappes[randomizator[i]].GetComponentInChildren<TextMeshPro>().text = questions2[r2].reponses[i];
                        trappes[randomizator[i]].GetComponentInChildren<BoxCollider>().tag = "Unvalid";
                    }
                }
                questionDisplayText.text = questions2[r2].intitule;
                questions2.Remove(questions2[r2]);
                nombreQuestionsposées++;
                break;
            case int n when (n < 3*nombreQuestionsParPhase):
                //Changer nombre question par phase pour infiniser jusqu'a une victoire
                int r3 = UnityEngine.Random.Range(0, questions3.Count);
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        trappes[randomizator[i]].GetComponentInChildren<TextMeshPro>().text = questions3[r3].reponses[i];
                        trappes[randomizator[i]].GetComponentInChildren<BoxCollider>().tag = "Valid";
                    }
                    else
                    {
                        trappes[randomizator[i]].GetComponentInChildren<TextMeshPro>().text = questions3[r3].reponses[i];
                        trappes[randomizator[i]].GetComponentInChildren<BoxCollider>().tag = "Unvalid";
                    }
                }
                questionDisplayText.text = questions3[r3].intitule;
                questions3.Remove(questions3[r3]);
                nombreQuestionsposées++;
                break;
        }
        isTimerEnabled = true;
        
        
    }
    void ConvertCSV()
    {
        questions1 = new List<Questions>();
        questions2 = new List<Questions>();
        questions3 = new List<Questions>();
        string[] data = questionsData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int questionsNumber = data.Length / 7;
        for (int i = 0; i < questionsNumber; i++)
        {
            switch (int.Parse(data[7*i]))
            {
                case 1:
                    questions1.Add(new Questions());
                    questions1[questions1.Count-1].theme = data[7*i + 1];
                    questions1[questions1.Count - 1].intitule = data[7 * i + 2];
                    for (int k = 0; k < 4; k++)
                    {
                        
                        questions1[questions1.Count - 1].reponses[k] = data[7*i + 3 + k];
                    }
                    break;
                case 2:
                    questions2.Add(new Questions());
                    questions2[questions2.Count - 1].theme = data[7 * i + 1];
                    questions2[questions2.Count - 1].intitule = data[7 * i + 2];
                    for (int k = 0; k < 4; k++)
                    {
                        questions2[questions2.Count - 1].reponses[k] = data[7 * i + 3 + k];
                    }
                    break;
                case 3:
                    questions3.Add(new Questions());
                    questions3[questions3.Count - 1].theme = data[7 * i + 1];
                    questions3[questions3.Count - 1].intitule = data[7 * i + 2];
                    for (int k = 0; k < 4; k++)
                    {
                        questions3[questions3.Count - 1].reponses[k] = data[7 * i + 3 + k];
                    }
                    break;
                default:
                    break;
            }
        }
    }
}