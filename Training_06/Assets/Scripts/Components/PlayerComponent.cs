using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComponent : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody rb;
    public Vector3 input;
    public bool isRight;
    public bool isWrong;

    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
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
            GameManager.gm.LoadLevel();
            GameManager.gm.playerList.Add(this);
            
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
