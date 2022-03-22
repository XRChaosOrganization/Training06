using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using TMPro;
using UnityEngine.Playables;

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
    public PlayableDirector director; 
    public PlayableAsset transitionToGame; 
    public PlayableAsset gameLoop; 
    public PlayableAsset winnerFlow; 
    public PlayableAsset gameOverFlow; 

    public GameObject barrelPrefab; 

    [Header("Question display")]
    public TextAsset questionsData;
    public TextMeshPro questionDisplayText;
    public Animator blackboardQuestionAnimator; 
    
    [Header("Timer")]
    public float timePerLevel = 20;
    public TextMeshPro timerDisplayText;
    public TimerComponent timerComponent; 

    [Header("Game Settings")]
    public int nombreQuestionsParPhase = 6;

    public Transform winnerSpot; 
    public List<PlayerComponent> playerList;
    public List<GameObject> trappes;
    public List<Transform> spots; 

    private List<Question> questions1;
    private List<Question> questions2;
    private List<Question> questions3;

    private int nombreQuestionsposes;
    private float timer;
    private bool isTimerEnabled;
    private string previousTheme;
    private GameObject winner; 

    private void Awake()
    {
        Instance = this;
        winner = null; 
        ConvertCSV();
        director.playableAsset = transitionToGame;
    }
    
    private void Update()
    {
        if (isTimerEnabled)
        {
            timerDisplayText.text = string.Format("{0:00}", timer);
            timer -= Time.deltaTime;
            //timerComponent.UpdateTimer(timePerLevel, timer);

            if (timer<=0.2f)
            {
                //Lock character controls 
                //SFX
                //VFX
            }
            if (timer <=0)
            {
                isTimerEnabled = false;
            }
        }
    }

    public void LoadGame ()
    {
        director.Play();
    }

    public void ReplaceDirectorPlayable ()
    {
        director.playableAsset = gameLoop; 
        director.extrapolationMode = DirectorWrapMode.Loop;
        director.Play();
    }

    public void RollBlackBoards ()
    {
        blackboardQuestionAnimator.SetTrigger("RollQuestionBlackboard");
        foreach (GameObject item in trappes)
        {
            item.GetComponent<TrappeComponent>().RollAnswerBloackBoard();
        }
    }

    public void DipslayQuestionText (bool _isDisplayed)
    {
        questionDisplayText.gameObject.SetActive(_isDisplayed);

        foreach (GameObject item in trappes)
        {
            item.GetComponent<TrappeComponent>().DisplayTrapText(_isDisplayed);
        }
    }

    public void StartTimer ()
    {
        timer = timePerLevel;
        isTimerEnabled = true;        
    }

    public void OpenHatches ()
    {
        foreach (GameObject hatch in trappes)
        {
            if(hatch.CompareTag("Unvalid") == true)
                hatch.GetComponent<Animator>().SetBool("isOpen", true);
        }
        foreach (PlayerComponent players in playerList)
        {
            if(players.playerState == PlayerComponent.PlayerState.IS_WRONG)
            {
                players.rb.isKinematic = true;
                players.rb.useGravity = false;
                players.GetComponent<CapsuleCollider>().enabled = false; 
                players.playerState = PlayerComponent.PlayerState.IS_DEAD;
                players.GetComponentInChildren<Animator>().SetTrigger("Fall");
            }
        }
    }

    public void CloseHatches ()
    {
        foreach (GameObject hatch in trappes)
        {
            if(hatch.CompareTag("Unvalid") == true)
                hatch.GetComponent<Animator>().SetBool("isOpen", false);
        }
    }

    public void ShootBarrels ()
    {
        bool needsToShootBarrels =  false; 

        foreach (PlayerComponent players in playerList)
        {
            if(players.playerState == PlayerComponent.PlayerState.DO_NOT_KNOW)
                needsToShootBarrels = true; 
        }

        if(needsToShootBarrels)
        {
            foreach (PlayerComponent players in playerList)
            {
                if(players.playerState == PlayerComponent.PlayerState.DO_NOT_KNOW)
                    Instantiate(barrelPrefab, new Vector3(players.transform.position.x, players.transform.position.y + 20.0f, players.transform.position.z), Quaternion.identity, this.transform);
            }
        }
    }

    public void EnablePlayerControl (bool _enabled)
    {
        foreach (PlayerComponent player in playerList)
        {
            if(player.playerState != PlayerComponent.PlayerState.IS_DEAD)
                player.canControlItsCharacter = _enabled;
        }
    }

    public void PlacePlayersOnSpots ()
    {
        foreach (PlayerComponent item in playerList)
        {
            if(item.playerState != PlayerComponent.PlayerState.IS_DEAD)
            {
                item.RepositionPlayerOnSpot();
            }
        }
    }

    public Transform GetPlayerSpot ()
    {
        Transform spot = spots[0];
        spots.RemoveAt(0);
        return spot;
    }

    public void VictoryCheck ()
    {
        //One Player flow
        if(playerList.Count <= 1)
        {
            if(playerList[0].playerState == PlayerComponent.PlayerState.IS_DEAD)
            {
                //Play game over flow 
                director.extrapolationMode = DirectorWrapMode.None;
                director.playableAsset = gameOverFlow; 
                director.Play();
            }
        }
        //Multiple players flow 
        else
        {
            int deadPlayers = 0;
            foreach (PlayerComponent item in playerList)
            {
                if(item.playerState == PlayerComponent.PlayerState.IS_DEAD)
                    deadPlayers ++; 
            }

            print("DEAD PLAYERS = " + deadPlayers);


            //All players are dead 
            if(deadPlayers >= this.GetComponent<PlayerInputManager>().playerCount)
            {
                //Play game over flow 
                director.extrapolationMode = DirectorWrapMode.None;
                director.playableAsset = gameOverFlow; 
                director.Play();
            }

            //One survivor remaining 
            if(deadPlayers == this.GetComponent<PlayerInputManager>().playerCount - 1)
            {
                foreach (PlayerComponent player in playerList)
                {
                    if(player.playerState != PlayerComponent.PlayerState.IS_DEAD)
                    {
                        winner = player.gameObject; 
                        break; 
                    }
                }
                //Play winning flow 
                director.extrapolationMode = DirectorWrapMode.None;
                director.playableAsset = winnerFlow; 
                director.Play();
            }
        }
    }

    public void RestartGame ()
    {
        
    }

    public void DisplayWinnerText ()
    {
        questionDisplayText.text = winner.gameObject.name + " has won ! GG !";
        questionDisplayText.gameObject.SetActive(true);
    }

    public void DisplayGameOverText ()
    {
        questionDisplayText.text = "GAME OVER ! \n Aucun gagnant ...";
        questionDisplayText.gameObject.SetActive(true);
    }

    public void PlaceWinnerOnSpot ()
    {
        winner.GetComponent<Rigidbody>().isKinematic = true;
        winner.GetComponent<Rigidbody>().useGravity = false; 
        winner.transform.position = winnerSpot.position;
        winner.transform.rotation = winnerSpot.rotation;
    }

    public void GenerateNewQuestion ()
    {
        List<int> randomizator = FillRandomizer();
        List<Question> questionTier = new List<Question>();

        //Select the question tier
        if(nombreQuestionsposes < nombreQuestionsParPhase)
            questionTier = questions1;
        else if (nombreQuestionsposes < 2 * nombreQuestionsParPhase)
            questionTier = questions2;
        else if (nombreQuestionsposes < 3 * nombreQuestionsParPhase)
            questionTier = questions3;

        
        //Select a random question among the current tier
        Question question = questionTier[UnityEngine.Random.Range(0, questionTier.Count)];

        //Display question
        questionDisplayText.text = question.intitule;
        questionTier.Remove(question);

        //Display question's answers in a random trap & set its tag
        for (int i = 0; i < randomizator.Count; i++)
        {
            if(i == 0)
            {
                trappes[randomizator[i]].GetComponent<TrappeComponent>().UpdateTrappeText(question.reponses[0]);
                trappes[randomizator[i]].GetComponentInChildren<BoxCollider>().tag = "Valid";
                trappes[randomizator[i]].tag = "Valid";
            }
            else
            {
                trappes[randomizator[i]].GetComponent<TrappeComponent>().UpdateTrappeText(question.reponses[i]);
                trappes[randomizator[i]].GetComponentInChildren<BoxCollider>().tag = "Unvalid";
                trappes[randomizator[i]].tag = "Unvalid";
            }
        }

        //Increment + enable timer
        nombreQuestionsposes ++;
    }
  
    public void QuitGame ()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    public void RegisterPlayer (GameObject _player)
    {
        playerList.Add(_player.GetComponent<PlayerComponent>());
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

    private void ConvertCSV()
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