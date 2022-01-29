using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public sealed class GameDirector : Core.SingleInstance<GameDirector>
    {
        public enum Map { Test1, Test2 }

        [SerializeField] private PlayerMovement player;
        [SerializeField] private ProjectParry.Map[] maps;
        private Map _currentMap;

        protected override void Awake()
        {
            base.Awake();

            foreach (var item in maps)
            {
                item.Disable();
            }

            SetMap(Map.Test1, true);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetMap(Map.Test1, false);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetMap(Map.Test2, false);
            }
        }

        public void SetMap(Map to, bool force)
        {
            if (!force && to == _currentMap) return;

            maps[(int)_currentMap].Disable();
            _currentMap = to;
            maps[(int)_currentMap].Enable(in player);
        }
    }
}