using System;
using UnityEngine;
using UnityEngine.XR;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6;

    public float sprintSpeed = 8;
    public float jumpForce;
    public float airMovementMultiplier = 0.2f;
    private readonly float _movementMultiplier = 10f;

    [Header("Jumping")] 
    [SerializeField] private LayerMask groundMask;
    private bool _isGrounded;
    private readonly float _groundDistance = 0.75f;


    [Header("Drag")] 
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float airDrag = 1f;

    [Header("Sounds")]
    public AudioSource footsteps;
    public AudioSource jump;
    public AudioSource terraformingSound;
    
    
    private float _horizontalMovement;
    private float _verticalMovement;
    private Vector3 _moveDirection;
    private Vector3 _slopeMoveDirection;

    private Rigidbody _rb;
    private RaycastHit _slopeHit;

    
    // reference to transform since repeated property access of built in component is inefficient
    private Transform _transform;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        
        _transform = transform;
    }
    
    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_transform.position, 
            _groundDistance, groundMask);

        GetInput();
        ControlDrag();

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            Jump();
        }
    

        _slopeMoveDirection = Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal);

        if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
             Input.GetKey(KeyCode.D)) && _isGrounded)
        {
            footsteps.enabled = true;
        }
        else
        {
            footsteps.enabled = false;
        }

        if (Input.GetKey(KeyCode.Mouse1) || Input.GetKey(KeyCode.Mouse0))
        {
            terraformingSound.enabled = true;
        }
        else
        {
            terraformingSound.enabled = false;
        }
    }

    private void ControlDrag()
    {
        _rb.drag = _isGrounded ? groundDrag : airDrag;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, 
                3))
        {
            return _slopeHit.normal != Vector3.up;
        }

        return false;
    }

    private void Jump()
    {
        _rb.AddForce(_transform.up * jumpForce, ForceMode.Impulse);
    }
    
    private void GetInput()
    {
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
        _verticalMovement = Input.GetAxisRaw("Vertical");
        
        _moveDirection = _transform.forward * _verticalMovement + _transform.right * _horizontalMovement;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_isGrounded && !OnSlope())
            {
                _rb.AddForce(_moveDirection.normalized * sprintSpeed * _movementMultiplier, ForceMode.Acceleration);
            }
            else if (_isGrounded && OnSlope())
            {
                _rb.AddForce(_slopeMoveDirection.normalized * sprintSpeed * _movementMultiplier, ForceMode.Acceleration);
            }
            else if (!_isGrounded)
            {
                _rb.AddForce(_moveDirection.normalized * (sprintSpeed * _movementMultiplier * airMovementMultiplier), 
                    ForceMode.Acceleration);
            }
        }
        else
        {
            if (_isGrounded && !OnSlope())
            {
                _rb.AddForce(_moveDirection.normalized * moveSpeed * _movementMultiplier, ForceMode.Acceleration);
            }
            else if (_isGrounded && OnSlope())
            {
                _rb.AddForce(_slopeMoveDirection.normalized * moveSpeed * _movementMultiplier, ForceMode.Acceleration);
            }
            else if (!_isGrounded)
            {
                _rb.AddForce(_moveDirection.normalized * (moveSpeed * _movementMultiplier * airMovementMultiplier), 
                    ForceMode.Acceleration);
            }
        }
        
    }
}