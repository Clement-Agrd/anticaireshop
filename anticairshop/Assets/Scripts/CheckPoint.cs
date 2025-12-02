using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class CheckPoint : MonoBehaviour
    {
        [SerializeField] private DeathZone deathZone;
        [SerializeField] public bool baseMap;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                deathZone.SetCheckPoint(this);
            }
        }
    }
}