using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Game._Scripts
{
    public class PlayerController : MonoBehaviour
    {
        #region InspectorVisible
        
        [field: SerializeField,
                Tooltip("Movement speed of the character")]
        private float movementSpeed;

        [field : SerializeField,
                 Tooltip("Flying speed of item")]
        private float flySpeed;
        
        [field : SerializeField,
                 Tooltip("Jump height")]
        private float jumpHeight = 8f;
        
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
                 Tooltip("Player's rigidbody")]
        private Rigidbody rigid;
        
        [field : SerializeField,
                 Tooltip("Animator of the player")]
        private Animator animator;
        
        [field : SerializeField,
                 Tooltip("Animator of the player")]
        private Transform playerTransform;
        
        [field : SerializeField,
                 Tooltip("Boxcollider of the player")]
        private BoxCollider boxCollider;
        
        [field : SerializeField,
                 Tooltip("Target for item to fly")]
        private Transform handTransform;
        
        #endregion

        #region Private

        private Transform _transform;

        private Vector3 _movementVector = Vector3.zero;
        private bool _isGrounded;
        private int _deathCounter;

        private Transform _heldItem;
        private Vector3 _defaultPositionItem;
        private Vector3 _defaultScale;
        private bool _itemIsHeld;
        
        private Coroutine _moveCoroutine;

        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Jump = Animator.StringToHash("Jump");
        
        #endregion


        private void Start()
        {
            _transform = transform;

            InputManager.KeyActions.Player.Move.performed    += context => MovementHandler(context.ReadValue<Vector2>());
            InputManager.KeyActions.Player.Move.canceled     += context => MovementHandler(context.ReadValue<Vector2>());
            
            InputManager.KeyActions.Player.Jump.performed    += JumpHandler;
            
            InputManager.KeyActions.Player.Crouch.performed  += x => CrouchHandler(true);
            InputManager.KeyActions.Player.Crouch.canceled   += x =>  CrouchHandler(false);
        }
        
        private void FixedUpdate()
        {
            //Check if player is grounded
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
            
            if(rigid.velocity.y < 0)
                animator.SetBool(Jump, false);
        }

        #region Handlers
        
        /// <summary>
        /// Death teleportation and updating counter event 
        /// </summary>
        private void DeathHandler()
        {
            if (_itemIsHeld)
            {
                _heldItem.SetParent(null);
                _heldItem.position = _defaultPositionItem;
                ResetHeldItem();
            }

            if (_moveCoroutine != null)
                _movementVector = Vector3.zero;
                        
            _transform.position = Vector3.zero;
            
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
                animator.SetInteger(Speed, input.y > 0 ? 1 : -1);

            if (_moveCoroutine == null)
                _moveCoroutine = StartCoroutine(Move());
        }
        
        /// <summary>
        /// Adding force to the Rigidbody if player is on the groundss
        /// </summary>
        /// <param name="obj"></param>
        private void JumpHandler(InputAction.CallbackContext obj)
        {
            if (!_isGrounded)
                return;
            
            rigid.AddForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            animator.SetBool(Jump, _isGrounded);
        }

        /// <summary>
        /// Make the character crouch (without animation)
        /// </summary>
        /// <param name="isCrouch"></param>
        private void CrouchHandler(bool isCrouch)
        {
            if (isCrouch)
            {
                playerTransform.localScale *= 0.4f;
                movementSpeed /= 2;
                boxCollider.enabled = false;
            }
            else
            {
                playerTransform.localScale = Vector3.one;
                movementSpeed *= 2;
                boxCollider.enabled = true;

                if(_itemIsHeld)
                    _heldItem.localScale = _defaultScale;
            }
        }

        #endregion

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
            
            animator.SetInteger(Speed, 0);
            _moveCoroutine = null;
        }
        
        /// <summary>
        /// Make object fly to another and set it as parent
        /// </summary>
        /// <param name="whichItem"></param>
        /// <param name="destination"></param>
        /// <param name="toPlayer"></param>
        /// <returns></returns>
        private IEnumerator PickUpRoutine(Transform whichItem, Transform destination, bool toPlayer)
        {
            if (toPlayer)
            {
                _defaultPositionItem = whichItem.position;
                _defaultScale = whichItem.localScale;
            }

            whichItem.SetParent(destination);

            while (whichItem.localPosition != Vector3.zero)
            {
                whichItem.localPosition = Vector3.MoveTowards(whichItem.localPosition,Vector3.zero, 
                    flySpeed* Time.deltaTime);

                yield return null;
            }

            if (toPlayer)
            {
                _heldItem = whichItem;
                _itemIsHeld = true;
            }
            else
            {
                ResetHeldItem();
                InputManager.EndGameEventInvoker();
            }
        }

        /// <summary>
        /// Remove held item from player
        /// </summary>
        private void ResetHeldItem()
        {
            _heldItem.rotation = Quaternion.identity;

            _heldItem = null;
            _itemIsHeld = false;
        }
        
        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("DeathTrigger"))
                DeathHandler();
            
            else if (col.CompareTag("Item"))
                StartCoroutine(PickUpRoutine(col.transform, handTransform, true));
            
            else if (col.CompareTag("Table"))
                if (_itemIsHeld)
                {
                    _heldItem.GetComponent<BoxCollider>().enabled = false;
                    StartCoroutine(PickUpRoutine(_heldItem, col.transform, false));
                }
        }


        #endregion
    }
}