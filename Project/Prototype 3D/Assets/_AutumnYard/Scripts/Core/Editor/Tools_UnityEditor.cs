using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Chibig
{
    public static class Tools_UnityEditor
    {
        private static EditorWindow _mouseOverWindow;

        [MenuItem("Chibig/Clear console %t", priority = 60)]
        private static void ClearConsole()
        {
            // This simply does "LogEntries.Clear()" the long way:
            //var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
            var logEntries = System.Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null, null);
        }

        //[MenuItem("Chibig/Select all objects with component MaterialSwapper %#m", priority = 60)]
        //private static void SelectObjectsComponentMaterialSwapper()
        //{
        //  UnityEngine.Object[] go = GameObject.FindObjectsOfType(typeof(Chibig.Graphics.MaterialSwapper));
        //  Selection.objects = go;
        //}

        [MenuItem("Chibig/Toggle window lock %q", priority = 60)]
        private static void ToggleInspectorLock()
        {
            if (_mouseOverWindow == null)
            {
                if (!EditorPrefs.HasKey("LockableInspectorIndex"))
                    EditorPrefs.SetInt("LockableInspectorIndex", 0);
                int i = EditorPrefs.GetInt("LockableInspectorIndex");

                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
                UnityEngine.Object[] findObjectsOfTypeAll = Resources.FindObjectsOfTypeAll(type);
                _mouseOverWindow = (EditorWindow)findObjectsOfTypeAll[i];
            }

            if (_mouseOverWindow != null && _mouseOverWindow.GetType().Name == "InspectorWindow")
            {
                Type type = Assembly.GetAssembly(typeof(UnityEditor.Editor)).GetType("UnityEditor.InspectorWindow");
                PropertyInfo propertyInfo = type.GetProperty("isLocked");
                bool value = (bool)propertyInfo.GetValue(_mouseOverWindow, null);
                propertyInfo.SetValue(_mouseOverWindow, !value, null);
                _mouseOverWindow.Repaint();
            }
        }

        [MenuItem("Chibig/Toggle selected GameObject %g", priority = 60)]
        private static void ToggleGameObject()
        {
            //var selected = Selection.transforms;
            if (Selection.transforms != null)
            {
                foreach (Transform t in Selection.transforms)
                    t.gameObject.SetActive(!t.gameObject.activeInHierarchy);
            }
        }

        [MenuItem("Chibig/Toggle selected GameObject: All false %#g", priority = 60)]
        private static void ToggleGameObjectFalse()
        {
            //var selected = Selection.transforms;
            if (Selection.transforms != null)
            {
                foreach (Transform t in Selection.transforms)
                    t.gameObject.SetActive(false);
            }
        }

        [MenuItem("Chibig/Toggle selected GameObject: All true %#&g", priority = 60)]
        private static void ToggleGameObjectTrue()
        {
            //var selected = Selection.transforms;
            if (Selection.transforms != null)
            {
                foreach (Transform t in Selection.transforms)
                    t.gameObject.SetActive(true);
            }
        }


        [MenuItem("Chibig/Reset scene camera", priority = 60)]
        private static void ResetSceneViewCamera()
        {
            SceneView.lastActiveSceneView.camera.transform.position = Vector3.zero;
            SceneView.lastActiveSceneView.camera.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

    }
}
