using System;
using UnityEngine;

namespace _Game._Scripts
{
    public class DeathTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            if (col.CompareTag("Player"))
                col.GetComponent<PlayerController>().DeathHandler();
        }
    }
}
