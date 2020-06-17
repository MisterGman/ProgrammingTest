using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game._Scripts
{
    public class PlayerController : MonoBehaviour, IDeathCounterText
    {
        #region InspectorVisible
        
        [field: SerializeField,
                Tooltip("Movement speed of the character")]
        private float movementSpeed;

        [field : SerializeField,
                 Tooltip("Transform which placed near legs")]
        private Transform groundCheck;
        
        [field : SerializeField,
                 Tooltip("Distance of sphere cast")]
        private float groundDist = 0.4f;
        
        [field : SerializeField,
                 Tooltip("Layer mask of the ground")]
        private LayerMask groundMask;
        
        [field : SerializeField,
                 Tooltip("Jump height")]
        private float jumpHeight = 8f;
        
        #endregion


        #region Private

        private Transform _transform;
        private Rigidbody _controller;
        private Animator _animator;
        
        private Vector3 _movementVector = Vector3.zero;
        private bool _isGrounded;
        private int _deathCounter;
        
        private Coroutine _moveCoroutine;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Jump = Animator.StringToHash("Jump");
        
        #endregion


        private void Start()
        {
            _transform = transform;

            _animator = GetComponent<Animator>();
            _controller = GetComponent<Rigidbody>();
             
            InputManager.KeyActions.Player.Move.performed    += context => MovementHandler(context.ReadValue<Vector2>());
            InputManager.KeyActions.Player.Move.canceled     += context => MovementHandler(context.ReadValue<Vector2>());
            
            InputManager.KeyActions.Player.Jump.performed    += JumpHundler;
            InputManager.KeyActions.Player.Crouch.performed  += x =>
                _transform.localScale *= 0.25f;
            InputManager.KeyActions.Player.Crouch.canceled   += x => transform.localScale = Vector3.one;
        }

        #region Handlers
        
        /// <summary>
        /// Death teleportation and updating counter event 
        /// </summary>
        public void DeathHandler()
        {
            _transform.position = Vector3.zero;
            _transform.rotation = Quaternion.Euler(Vector3.zero);

            _deathCounter++;
            InputManager.DeathCounterEventInvoker(_deathCounter);
        }
        
        /// <summary>
        /// Movement handler
        /// Starts coroutine where player's movement happens
        /// </summary>
        /// <param name="input"></param>
        private void MovementHandler(Vector2 input)
        {
            _movementVector.z = input.y;
            _movementVector.x = input.x;

            if (_isGrounded)
                _animator.SetInteger(Speed, input.y > 0 ? 1 : -1);

            if (_moveCoroutine == null)
                _moveCoroutine = StartCoroutine(Move());
        }
        
        /// <summary>
        /// Adding force to the Rigidbody if player is on the groundss
        /// </summary>
        /// <param name="obj"></param>
        private void JumpHundler(InputAction.CallbackContext obj)
        {
            if (!_isGrounded)
                return;
            
            _controller.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            _animator.SetBool(Jump, _isGrounded);
        }

        #endregion

        private void FixedUpdate()
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
            
            if(_controller.velocity.y < 0)
                _animator.SetBool(Jump, false);

        }


        #region Helpers
        
        /// <summary>
        /// "Update" for movement
        /// So update method does not running it constantly
        /// </summary>
        /// <returns></returns>
        private IEnumerator Move()
        {
            while (_movementVector != Vector3.zero)
            {
                var moveForward =  _movementVector.normalized * (Time.deltaTime * movementSpeed);
                _transform.Translate(moveForward, Space.Self);

                yield return null;
            } 
            
            _animator.SetInteger(Speed, 0);
            _moveCoroutine = null;
        }
        
        #endregion
    }
}