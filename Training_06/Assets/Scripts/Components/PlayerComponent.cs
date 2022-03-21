using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerComponent : MonoBehaviour
{
    public enum PlayerState {IS_RIGHT, IS_WRONG, DO_NOT_KNOW, IS_DEAD };
    public PlayerState playerState;

    [Header("3C Settings")]
    public Rigidbody rb;
    public bool canControlItsCharacter = true; 
    public float moveSpeed;
    public float rotationSpeed; 
    public Vector2 input;
    public GameObject charArmature; 
    public float armatureOffset = 3.5f;

    [Header("Game state")]
    public int vies;
    public Animator playerAnimator;
    public Transform playerSpot; 


    private void Awake() 
    {
        GameManager.Instance.RegisterPlayer(this.gameObject);
        playerSpot = GameManager.Instance.GetPlayerSpot();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //Since animations are badly done, there's an offset in the skinedMesh, we correct the offset here, this is a hack
        charArmature.transform.position = new Vector3(transform.position.x, transform.position.y - armatureOffset, transform.position.z);

        Vector3 desiredVelocity = new Vector3(input.x, 0.0f, input.y);

        if(canControlItsCharacter == false) 
        {
            playerAnimator.SetBool("isRunning", false);
            input = Vector2.zero;
            desiredVelocity = Vector3.zero;
            return; 
        } 

        rb.velocity = desiredVelocity * moveSpeed;

        if(desiredVelocity.magnitude >= 0.1f)
            playerAnimator.SetBool("isRunning", true);
        else
            playerAnimator.SetBool("isRunning", false);

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

    public void RepositionPlayerOnSpot ()
    {
        transform.position = playerSpot.position; 
    }
}
