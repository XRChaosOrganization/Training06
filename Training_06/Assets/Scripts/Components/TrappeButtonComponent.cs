using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrappeButtonComponent : MonoBehaviour
{
    public enum FunctionToCall { START_GAME, QUIT };
    public FunctionToCall functionToCall; 
    public List<GameObject> currentPlayerOnTrappe;
    public float timeBeforeCallingFunction = 3.0f;
    public Renderer meshRenderer; 
    public bool isLoading = false; 
    public bool isCallingFunction = false; 
    private float timer;
    private int numberOfPlayersInGame = 0;

    void Update()
    {
        numberOfPlayersInGame = GameManager.Instance.GetComponent<PlayerInputManager>().playerCount;

        if(isLoading)
        {
            timer += Time.deltaTime;
            timer = Mathf.Clamp(timer, 0.0f, timeBeforeCallingFunction);

            float completion = (timer / 1) / timeBeforeCallingFunction + 0.5f; 

            meshRenderer.materials[1].SetFloat("_Completion", completion);

            if(timer >= timeBeforeCallingFunction && isCallingFunction == false)
            {
                isCallingFunction = true; 
                switch (functionToCall)
                {
                    case FunctionToCall.START_GAME:
                        GameManager.Instance.LoadLevel();
                        break;
                    case FunctionToCall.QUIT:
                        GameManager.Instance.QuitGame();
                        break;
                }


            }
        }
        else
        {
            timer = 0.0f;
            meshRenderer.materials[1].SetFloat("_Completion", 0.5f);
        }
    }

    public void RegisterPlayerOnTrappe(bool _isEntering, GameObject _player)
    {
        if(_isEntering)
        {
            if(currentPlayerOnTrappe.Contains(_player) == false)
            {
                currentPlayerOnTrappe.Add(_player);
            }

            if(currentPlayerOnTrappe.Count >= numberOfPlayersInGame)
            {
                isLoading = true; 
            }
        }
        else
        {
            if(currentPlayerOnTrappe.Contains(_player))
            {
                currentPlayerOnTrappe.Remove(_player);
            }
            
            if(currentPlayerOnTrappe.Count < numberOfPlayersInGame)
            {
                isLoading = false; 
            }
        }
    }
}
