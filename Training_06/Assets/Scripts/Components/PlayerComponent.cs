using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComponent : MonoBehaviour
{
    public enum PlayerState {IS_RIGHT, IS_WRONG, DO_NOT_KNOW };
    public PlayerState playerState;

    [Header("3C Settings")]
    public float moveSpeed;
    public float rotationSpeed; 
    public Vector2 input;

    [Header("Game state")]
    public int vies;

    private Rigidbody rb;

    private void Awake() 
    {
        GameManager.Instance.RegisterPlayer(this.gameObject);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 desiredVelocity = new Vector3(input.x, 0.0f, input.y);
        rb.velocity = desiredVelocity * moveSpeed;

        //Rotation
        Vector3 lastLookingDirection = transform.forward;
        
        if (desiredVelocity.magnitude >= 0.3f)
            lastLookingDirection = desiredVelocity;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lastLookingDirection, Vector3.up), Time.deltaTime * rotationSpeed);
    }

    public void OnMove(InputAction.CallbackContext _context)
    {
        input = _context.ReadValue<Vector2>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("TrappeButton"))
        {
            col.GetComponentInParent<TrappeButtonComponent>().RegisterPlayerOnTrappe(true, this.gameObject);
        }
        if (col.CompareTag("Valid"))
        {
            playerState = PlayerState.IS_RIGHT;
        }
        if (col.CompareTag("Unvalid"))
        {
            playerState = PlayerState.IS_WRONG;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("TrappeButton"))
        {
            col.GetComponentInParent<TrappeButtonComponent>().RegisterPlayerOnTrappe(false, this.gameObject);
        }
        if (col.CompareTag("Valid"))
        {
            playerState = PlayerState.DO_NOT_KNOW;
        }
        if (col.CompareTag("Unvalid"))
        {
            playerState = PlayerState.DO_NOT_KNOW;
        }
    }
}
