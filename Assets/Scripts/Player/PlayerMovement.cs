using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class  PlayerMovement : MonoBehaviour
{ 
	private AudioSource footsteps;
	private bool footstepsPlaying = false;
    [Header("Movement")] 
    [SerializeField] private float walkSpeed = 50f;
    [SerializeField] private float runSpeed = 100f;
    [SerializeField] private float airMovementMultiplier = 0.4f;

    [Header("Jumping")] 
    [SerializeField] private float jumpForce = 55f;
    private bool _jumping;
    private bool _canJump = true;
    private readonly float _jumpCoolDown = 0.25f;
    
    [Header("Ground Check")] 
    public LayerMask ground;
    [SerializeField] private float playerHeight = 2f;
    private bool _grounded;
    private readonly float _groundDistance = 0.2f;

    [Header("Key Binds")] 
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Physics Related")]
    [SerializeField] private float rbDrag = 3f;
    [SerializeField] private float airDrag = 1f;
    [SerializeField] private float maxSlopeAngle;
    
    private float _horizontalMovement;
    private float _verticalMovement;

    private Vector3 _moveDirection;

    private Rigidbody _rb;

    private RaycastHit _slopeHit;

    [SerializeField] private TMP_Text healthText;

    private int health;

    private Vector3 startPOS;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
		footsteps = GetComponent<AudioSource>();
        _rb.freezeRotation = true;
        health = 4;
        startPOS = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
		if ((_horizontalMovement != 0 | _verticalMovement != 0) && !_jumping && footstepsPlaying == false) {
			footsteps.Play();	
		} else if (_horizontalMovement == 0 && _verticalMovement == 0){
			footsteps.Stop();
			footstepsPlaying = !footstepsPlaying;
		}
        
        // check if grounded
        _grounded = Physics.CheckSphere(
            transform.position - new Vector3(0, playerHeight/2, 0), _groundDistance, ground
            );
    }

    private void FixedUpdate()
    {
        // add drag
        _rb.drag = _grounded ? rbDrag : airDrag;
        MovePlayer();
    }

    private void GetInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");
        _jumping = Input.GetButton("Jump");
    }
    
    private void MovePlayer()
    {
        float moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = runSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }
        _moveDirection = transform.forward * _verticalMovement + transform.right * _horizontalMovement;

        // if holding down jump key, jump
        if (_jumping)
        {
            Jump();
        }
        // if we're on slope, add movement force towards the slant of the slope
        if (OnSlope())
        {
            _rb.AddForce( GetSlopeMoveDirection() * moveSpeed, ForceMode.Acceleration);

        }
        // if on ground, add movement as normal
        if (_grounded)
        {
            _rb.AddForce(_moveDirection.normalized * moveSpeed, ForceMode.Acceleration);
        }
        // if not on ground, reduce movement scale by airMovementMultiplier
        else if (!_grounded)
        {
            _rb.AddForce(_moveDirection.normalized * moveSpeed * airMovementMultiplier, ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        if (_grounded && _canJump)
        {
            _canJump = false;
            _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            
            // fixes the bouncing effect when holding down jump
            //If jumping while falling, reset y velocity.
            Vector3 vel = _rb.velocity;
            if (_rb.velocity.y < 0.5f)
                _rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (_rb.velocity.y > 0) 
                _rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(JumpCoolDown), _jumpCoolDown);
        }
    }

    private void JumpCoolDown()
    {
        _canJump = true;
    }

    private bool OnSlope()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, out _slopeHit, playerHeight / 2 + 0.5f))
        {
            return false;
        }
        float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
        return angle < maxSlopeAngle && angle != 0;
    }
    
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;    
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Portal")
        {
            SceneManager.LoadScene("SampleScene");
        } else if (collision.gameObject.tag == "Enemy")
        {
            health--;
            if (health == 0)
            {
                SceneManager.LoadScene("MainMenu");
				Cursor.visible = true;
            	Cursor.lockState = CursorLockMode.None;
            }
            healthText.text = "Health: " + health;
            transform.position = startPOS;
        }
    }

}
