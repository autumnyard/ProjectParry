using UnityEngine;

namespace AutumnYard.Core
{
    public abstract class SingletonAsset<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (!instance)
                {
                    instance = Resources.Load<T>(typeof(T).Name);
                }

                //if( !_instance )
                //{
                //  Debug.LogError( $"SingletonObject: Can't find asset {typeName} in Resources, gonna try addressables" );
                //  _instance = ChibigManager.Instance.GetManager( ChibigManager.Manager.Animal);
                //}

#if UNITY_EDITOR
                if (!instance)
                {
                    Debug.LogError($"SingletonAsset: Can't find asset {typeof(T).Name}");
                }
#endif

                instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                return instance;
            }
        }
    }
}
