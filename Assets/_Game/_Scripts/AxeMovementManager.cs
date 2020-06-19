using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Game._Scripts
{
    public class AxeMovementManager : MonoBehaviour
    {
        /// <summary>
        /// Serializable class which contains all data to move vertical axes
        /// </summary>
        [field : SerializeField,
                 Tooltip("Serializable class which contains all data to move vertical axes")]
        private AxeSettings vertAxe;        
        
        /// <summary>
        /// Serializable class which contains all data to move horizontal axes
        /// </summary>
        [field : SerializeField,
                 Tooltip("Serializable class which contains all data to move horizontal axes")]
        private AxeSettings horAxe;

        private void Start()
        {
            StartCoroutine(AxeMovement());
        }

        private void UpdateAxePosition(AxeSettings axeSettings, bool isVert)
        {
            int vectorIndex = isVert ? 1 : 0;
            
            //Calculating vertical Sin movement 
            for (int i = 0; i < axeSettings.AxeList.Count; i++)
            {
                float currSpeed = i % 2 == 0 ? axeSettings.Speed : -axeSettings.Speed;
                float sin = Mathf.Sin(Time.time * currSpeed);

                if(Math.Abs(sin) >= axeSettings.HoldValue)
                    continue;
                    
                float newY = sin * axeSettings.Height;

                var newPos = axeSettings.AxeList[i].position;
                newPos[vectorIndex] = newY;
                
                axeSettings.AxeList[i].position = newPos;
            }
        }

        private IEnumerator AxeMovement()
        {
            while (true)
            {
                UpdateAxePosition(vertAxe, true);
                UpdateAxePosition(horAxe, false);
                yield return null;
            }
        }
    }

    [Serializable]
    public class AxeSettings
    {
        [field : SerializeField,
                 Tooltip("List of all vertical axes")]
        private List<Transform> axeList;

        /// <summary>
        /// List of all vertical axes
        /// </summary>
        public List<Transform> AxeList => axeList;

        [field : SerializeField,
                 Tooltip("Speed of moving from -1 to 1")]
        private float speed;

        /// <summary>
        /// Speed of moving from -1 to 1
        /// </summary>
        public float Speed => speed;

        [field : SerializeField,
                 Tooltip("The end point of position during 1 and -1")]
        private float height;

        /// <summary>
        /// The end point of position during 1 and -1
        /// </summary>
        public float Height => height;
        
        [field: SerializeField,
                Range(0.0f, 1.0f),
                Tooltip("Normalized value at which the axe will be held in the air")]
        private float holdValue = 0.9f;

        ///<summary>
        /// Normalized value at which the axe will be held in the air
        ///</summary>
        public float HoldValue => holdValue;
    }
}
