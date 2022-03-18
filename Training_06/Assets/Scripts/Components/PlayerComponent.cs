using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComponent : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 input;
    public bool isRight;
    public bool isWrong;
    public int vies;
    public float startTimer;
    float _startTimer;

    private Rigidbody rb;

    private void Awake() 
    {
        //GameManager.Instance.RegisterPlayer(this.gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        //Hack to allow the player prefabs to spawn on the ground level
        transform.position = new Vector3 (transform.position.x, transform.position.y + 1.50f, transform.position.z);
    }

    private void Update()
    {
        input.z = input.y;
        rb.velocity = input * moveSpeed;


    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        input = _context.ReadValue<Vector2>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Start"))
        {
            GameManager.Instance.RegisterPlayer(this.gameObject);
            if (GameManager.Instance.playerList.Count == GameManager.Instance.GetComponent<PlayerInputManager>().playerCount)
            {
                _startTimer = startTimer;
                _startTimer -= Time.deltaTime;
                if (_startTimer <= 0)
                {
                    GameManager.Instance.LoadLevel();
                }
            }
        }
        if (col.CompareTag("Valid"))
        {
            isWrong = false;
            isRight = true;
        }
        if (col.CompareTag("Unvalid"))
        {
            isRight = false;
            isWrong = true;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Valid"))
        {
            isRight = false;
        }
        if (col.CompareTag("Unvalid"))
        {
            isWrong = false;
        }
    }
}
