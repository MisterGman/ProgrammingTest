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
        
        /// <summary>
        /// Rotation of the camera's holder and the player
        /// </summary>
        /// <param name="lookPos"></param>
        private void CameraMovement(Vector2 lookPos)
        {
            // _mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
            // _mouseY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            
            _mouseX += lookPos.x * rotationSpeed;
            _mouseY -= lookPos.y * rotationSpeed;
            
            _mouseY = Mathf.Clamp(_mouseY, -35, 60);
            
            _transform.LookAt(target);
        
            target.rotation = Quaternion.Euler(_mouseY, _mouseX, 0);
            player.rotation = Quaternion.Euler(0, _mouseX, 0);
        }
    
    }
}
