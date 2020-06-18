using System;
using UnityEngine;

namespace _Game._Scripts
{
    public class PlayerCameraController : MonoBehaviour
    {
        #region InspectorVisible

        [field: SerializeField,
                Tooltip("Rotation speed of the camera")]
        private float rotationSpeed;
        
        [field: SerializeField,
                Tooltip("Min camera Y rotation")]
        private float minYRot = -35f;
        
        [field: SerializeField,
                Tooltip("Max camera Y rotation")]
        private float maxYRot = 60f;

        [field : SerializeField,
                 Tooltip("Player transform")]
        private Transform player;
    
        [field : SerializeField,
                 Tooltip("Camera holder transform transform")]
        private Transform target;

        #endregion

        #region MyRegion

        private float _mouseX;
        private float _mouseY;

        private Vector2 _cameraPos;
        private Transform _transform;
        
        #endregion

        private void Start()
        {
            _transform = transform;
            InputManager.KeyActions.Player.MouseLook.performed += context => CameraMovement(context.ReadValue<Vector2>());
        }

        #region Enable/Disable

        private void OnEnable() =>
            InputManager.DeathCounterEvent += ResetMousePosition;

        private void OnDisable() =>
            InputManager.DeathCounterEvent -= ResetMousePosition;

        #endregion


        /// <summary>
        /// Reset camera and rotation
        /// </summary>
        /// <param name="x"></param>
        private void ResetMousePosition(int x)
        {
            _mouseX = 0;
            _mouseY = 0;
            
            target.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
            player.rotation = Quaternion.Euler(0, _mouseX, 0);
        }

        /// <summary>
        /// Rotation of the camera's holder and the player
        /// </summary>
        /// <param name="lookPos"></param>
        private void CameraMovement(Vector2 lookPos)
        {
            _mouseX += lookPos.x * rotationSpeed * Time.deltaTime;
            _mouseY -= lookPos.y * rotationSpeed * Time.deltaTime;
            
            _mouseY = Mathf.Clamp(_mouseY, minYRot, maxYRot);
            
            _transform.LookAt(target);
        
            target.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
            player.rotation = Quaternion.Euler(0, _mouseX, 0);
        }
    
    }
}
