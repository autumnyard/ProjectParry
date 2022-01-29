using System;
using UnityEngine;
using System.Linq;

namespace AutumnYard.Core
{
    public abstract class SingleInstance<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        private static bool hasInstance;
        private static int instanceId;

        public static bool HasInstance => hasInstance;
        public static bool HasInstance2 => instance != null || !instance.Equals(null);
        public static bool HasInstance3 => instance != null || !instance.Equals(null) || Instance != null;


        public static T Instance
        {
            get
            {
                if (hasInstance)
                {
                    return instance;
                }

                instance = FindFirstInstance();

                if (instance == null)
                {
                    throw new Exception($"The instance of singleton component {typeof(T)} was requested, but it doesn't appear to exist in the scene.");
                }

                hasInstance = true;
                instanceId = instance.GetInstanceID();
                return instance;
            }
        }


        protected virtual void Awake()
        {
            if (Application.isPlaying && Instance != null)
            {
                if (GetInstanceID() != instanceId)
                {
#if UNITY_EDITOR
                    Debug.LogError($"A redundant instance ({name}) of singleton {typeof(T)} is present in the scene.", this);
                    UnityEditor.EditorGUIUtility.PingObject(this);
                    //throw new Exception($"A redundant instance ({name}) of singleton {typeof(T)} is present in the scene.");
#endif
                    enabled = false;
                }

                // This might be unnecessary, but just to be safe we do it anyway.
                foreach (var redundantInstance in FindInstances().Where(o => o.GetInstanceID() != instanceId))
                {
                    redundantInstance.enabled = false;
                }
            }
        }

        protected virtual void OnDestroy()
        {
            if (GetInstanceID() == instanceId)
            {
                hasInstance = false;
            }
        }

        private static T[] FindInstances()
        {
            var objects = FindObjectsOfType<T>();
            Array.Sort(objects, (a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
            return objects;
        }

        private static T FindFirstInstance()
        {
            var objects = FindInstances();
            return objects.Length > 0 ? objects[0] : null;
        }


        protected void Log(string text) => Debug.Log(text, this);
        protected void LogError(string text) => Debug.LogError(text, this);

    }
}
