using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AutumnYard.ProjectParry
{
    public sealed class GameDirector : Core.SingleInstance<GameDirector>
    {
        public enum Map { None, Test1, Test2 }

        [SerializeField] private GameObject[] maps;
        private GameObject _currentMapGO;
        private Map _currentMap;

        protected override void Awake()
        {
            base.Awake();

            SetMap(Map.Test1);
        }

        public void SetMap(Map to)
        {
            if (to == _currentMap) return;

            _currentMap = to;

            _currentMapGO = Instantiate(maps[(int)_currentMap]);
        }
    }
}