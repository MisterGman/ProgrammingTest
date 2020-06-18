using System;
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
                 Tooltip("")]
        private VerticalAxeSettings vertAxe;        
        
        /// <summary>
        /// Serializable class which contains all data to move horizontal axes
        /// </summary>
        [field : SerializeField,
                 Tooltip("")]
        private HorizontalAxeSettings horAxe;

        private void Update()
        {
            //Calculating vertical Sin movement 
            for (int i = 0; i < vertAxe.VerticalAxeList.Count; i++)
            {
                float currSpeed = vertAxe.Speed;
                if (i % 2 == 0)
                    currSpeed = -currSpeed;
                
                float newY = Mathf.Sin(Time.time * currSpeed) * vertAxe.Height;

                var newPos = vertAxe.VerticalAxeList[i].position;
                newPos = new Vector3(newPos.x, newY, newPos.z);
                
                vertAxe.VerticalAxeList[i].position = newPos;
            }
            
            //Calculating horizontal Sin movement 
            for (int i = 0; i < horAxe.HorizontalAxeList.Count; i++)
            {
                float currSpeed = horAxe.Speed;
                if (i % 2 == 0)
                    currSpeed = -currSpeed;
                
                float newX = Mathf.Sin(Time.time * currSpeed) * horAxe.Height;
                horAxe.HorizontalAxeList[i].position = 
                    new Vector3(newX, horAxe.HorizontalAxeList[i].position.y, horAxe.HorizontalAxeList[i].position.z);
            }
        }
    }

    [Serializable]
    public class VerticalAxeSettings
    {
        [field : SerializeField,
                 Tooltip("List of all vertical axes")]
        private List<Transform> verticalAxeList;

        /// <summary>
        /// List of all vertical axes
        /// </summary>
        public List<Transform> VerticalAxeList => verticalAxeList;

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
    }
    
    [Serializable]
    public class HorizontalAxeSettings
    {
        [field : SerializeField,
                 Tooltip("List of all horizontal axes")]
        private List<Transform> horizontalAxeList;

        /// <summary>
        /// List of all vertical axes
        /// </summary>
        public List<Transform> HorizontalAxeList => horizontalAxeList;
        
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
    }
}
