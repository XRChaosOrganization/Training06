using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class Question
{
    public int difficulty;
    public string theme;
    public string intitule;
    public string[] reponses = new string[4];
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Flow")]
    public GameObject menu;
    public GameObject game;

    [Header("Question display")]
    public TextAsset questionsData;
    public TextMeshPro questionDisplayText;
    
    [Header("Timer display")]
    public TextMeshPro timerDisplayText;

    [Header("Trappes")]
    Transform trappesContainer;
    
    [Header("Game Settings")]
    public Animator captainAnimator; 
    public Animator RopeAnimator; 
    public Animator LevelAnimator; 
    public Transform captainGamePosition; 

    [Header("Game Settings")]
    public int nombreQuestionsParPhase = 6;
    public float timePerLevel = 20;
    public float firstLeveldelay;
    public float questionDisplayDelay;
    public float answersDisplayDelay;
    public float noAnswerdelay;
    public float goodAnswerDelay;
    public float wrongAnswerDelay;
    public List<PlayerComponent> playerList;
    public List<GameObject> trappes;

    private List<Question> questions1;
    private List<Question> questions2;
    private List<Question> questions3;
    private int nombreQuestionsposes;
    float timer;
    bool isTimerEnabled;
    string previousTheme;

    private void Awake()
    {
        Instance = this;
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
                //Lock character controls 
                //SFX
                //VFX
            }
            if (timer <=0)
            {
                isTimerEnabled = false;
                StartCoroutine(EndQuestion());
            }
        }
    }

    IEnumerator EndQuestion()
    {
        foreach (PlayerComponent _player in playerList)
        {
            if (_player.playerState == PlayerComponent.PlayerState.IS_RIGHT)
            {
                //Anim bonne réponse
                yield return new WaitForSeconds(goodAnswerDelay);
            }
            else if (_player.playerState == PlayerComponent.PlayerState.IS_WRONG)
            {
                StartCoroutine(KillPlayer(_player));
            }
            else if (_player.vies > 0 && _player.playerState == PlayerComponent.PlayerState.DO_NOT_KNOW)
            {
                //Anim pas de réponse / anim enlever une vie
                yield return new WaitForSeconds(noAnswerdelay);
                _player.vies--;
            }
            else
            {
                StartCoroutine(KillPlayer(_player));
            }
        }
        LoadNewQuestion();
    }
    
    //Upon starting a new game (coming from the menu)
    public void LoadLevel()
    {
        captainAnimator.SetTrigger("SwingRope");
        RopeAnimator.SetTrigger("SwingRope");
        StartCoroutine(PlayCaptainArrival(1.5f));


        timer = timePerLevel;
        
        menu.SetActive(false);
        game.SetActive(true);

        StartCoroutine(GenerateNewQuestion(nombreQuestionsposes));
    }
        
    public void QuitGame ()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    //Going to next question (already in game)
    public void LoadNewQuestion ()
    {
        timer = timePerLevel;
        StartCoroutine(GenerateNewQuestion(nombreQuestionsposes));
    }

    private IEnumerator GenerateNewQuestion (int _nbDeQuestionsPosees)
    {
        List<int> randomizator = FillRandomizer();
        List<Question> questionTier = new List<Question>();

        if (_nbDeQuestionsPosees == 0)
        {
            yield return new WaitForSeconds(firstLeveldelay);
        }

        //Select the question tier
        if(_nbDeQuestionsPosees < nombreQuestionsParPhase)
            questionTier = questions1;
        else if (_nbDeQuestionsPosees < 2 * nombreQuestionsParPhase)
            questionTier = questions2;
        else if (_nbDeQuestionsPosees < 3 * nombreQuestionsParPhase)
            questionTier = questions3;

        //Select a random question among the current tier
        Question question = questionTier[UnityEngine.Random.Range(0, questionTier.Count)];

        //Display question
        yield return new WaitForSeconds(questionDisplayDelay);
        questionDisplayText.text = question.intitule;
        questionTier.Remove(question);

        yield return new WaitForSeconds(answersDisplayDelay);
        //Display question's answers in a random trap & set its tag
        for (int i = 0; i < randomizator.Count; i++)
        {
            if(i == 0)
            {
                trappes[randomizator[i]].GetComponent<TrappeComponent>().UpdateTrappeText(question.reponses[0]);
                trappes[randomizator[i]].GetComponentInChildren<BoxCollider>().tag = "Valid";
            }
            else
            {
                trappes[randomizator[i]].GetComponent<TrappeComponent>().UpdateTrappeText(question.reponses[i]);
                trappes[randomizator[i]].GetComponentInChildren<BoxCollider>().tag = "Unvalid";
            }
        }

        //Increment + enable timer
        nombreQuestionsposes ++;
        isTimerEnabled = true;
    }

    //Fill a list of size 4 with random numbers between 0 & 3 
    private List<int> FillRandomizer ()
    {
        List<int> randomizator = new List<int>();
        while (randomizator.Count < 4)
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
        return randomizator; 
    }

    public void RegisterPlayer (GameObject _player)
    {
        playerList.Add(_player.GetComponent<PlayerComponent>());
    }

    IEnumerator KillPlayer(PlayerComponent _player)
    {
        //Anim mauvaise réponse
        yield return new WaitForSeconds(wrongAnswerDelay);
        playerList.Remove(_player);
        _player.gameObject.SetActive(false);
    }

    private IEnumerator PlayCaptainArrival (float _time)
    {
        yield return new WaitForSeconds(_time);
        LevelAnimator.SetTrigger("StartGame");
        yield return new WaitForSeconds(0.5f);
        captainAnimator.gameObject.transform.position = captainGamePosition.position;
        captainAnimator.SetBool("IsIdle", true);
    }

    void ConvertCSV()
    {
        questions1 = new List<Question>();
        questions2 = new List<Question>();
        questions3 = new List<Question>();
        string[] data = questionsData.text.Split(new string[] { ",", "\n" }, StringSplitOptions.None);
        int questionsNumber = data.Length / 7;
        for (int i = 0; i < questionsNumber; i++)
        {
            switch (int.Parse(data[7*i]))
            {
                case 1:
                    questions1.Add(new Question());
                    questions1[questions1.Count-1].theme = data[7 * i + 1];
                    questions1[questions1.Count - 1].intitule = data[7 * i + 2];
                    for (int k = 0; k < 4; k++)
                    {
                        
                        questions1[questions1.Count - 1].reponses[k] = data[7 * i + 3 + k];
                    }
                    break;
                case 2:
                    questions2.Add(new Question());
                    questions2[questions2.Count - 1].theme = data[7 * i + 1];
                    questions2[questions2.Count - 1].intitule = data[7 * i + 2];
                    for (int k = 0; k < 4; k++)
                    {
                        questions2[questions2.Count - 1].reponses[k] = data[7 * i + 3 + k];
                    }
                    break;
                case 3:
                    questions3.Add(new Question());
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