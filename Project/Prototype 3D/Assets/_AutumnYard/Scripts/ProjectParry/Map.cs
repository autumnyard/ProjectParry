using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public sealed class Map : MonoBehaviour
    {
        [SerializeField] private Transform player1Spawn;
        [SerializeField] private Transform player2Spawn;

        public void Enable(in PlayerMovement player)
        {
            gameObject.SetActive(true);
            player.Spawn(player1Spawn.position);
        }

        internal void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
