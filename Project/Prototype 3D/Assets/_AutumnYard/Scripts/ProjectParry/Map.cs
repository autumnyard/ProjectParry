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

        public void Enable(in IPlayer player1, in IPlayer player2)
        {
            gameObject.SetActive(true);
            player1.Spawn(player1Spawn.position);
            player2.Spawn(player2Spawn.position);
        }

        internal void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
