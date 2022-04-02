using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JK.Utils
{
    /// <summary>
    /// A <see cref="UnityEditor.Undo"/> wrapper that can be used without #if guards
    /// </summary>
    public static class Undo
    {
        public static GameObject CreateGameObject(string name, Transform parent = null, params System.Type[] components)
        {
            GameObject go = new GameObject(name, components);

#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Create {name}");
            if (parent != null)
                UnityEditor.Undo.SetTransformParent(go.transform, parent, $"Create {name}");
#else
            if (parent != null)
                go.transform.SetParent(parent);
#endif

            return go;
        }

        public static T CreateComponentOnNewGameObject<T>(string name, Transform parent = null) where T : MonoBehaviour
        {
            GameObject go = CreateGameObject(name, parent, typeof(T));
            return go.GetComponent<T>();
        }

        public static T AddComponent<T>(GameObject go) where T : Component
        {
#if UNITY_EDITOR
            return UnityEditor.Undo.AddComponent<T>(go);
#else
            return go.AddComponent<T>();
#endif
        }

//        public static GameObject InstantiatePrefab(GameObject prefab, Transform parent)
//        {
//            GameObject go = PrefabUtils.InstantiatePrefab(prefab, parent);

//#if UNITY_EDITOR
//            UnityEditor.Undo.RegisterCreatedObjectUndo(go, $"Instantiate {prefab.name}");
//#endif

//            return go;
//        }

//        public static T InstantiatePrefab<T>(T prefab, Transform parent) where T : MonoBehaviour
//        {
//            T mb = PrefabUtils.InstantiatePrefab(prefab, parent);

//#if UNITY_EDITOR
//            UnityEditor.Undo.RegisterCreatedObjectUndo(mb.gameObject, $"Instantiate {prefab.name}");
//#endif

//            return mb;
//        }

        public static void RecordObjectForUndo(UnityEngine.Object go, string undoName)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.RecordObject(go, undoName);
#endif
        }

        public static void DestroyObjectImmediate(UnityEngine.Object obj)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.DestroyObjectImmediate(obj);
#else
            GameObject.DestroyImmediate(obj);
#endif
        }

        public static void DestroyChildrenImmediate(Transform transform)
        {
            List<Transform> children = new List<Transform>(transform.childCount);

            foreach (Transform child in transform)
                children.Add(child);

            foreach (Transform child in children)
                DestroyObjectImmediate(child.gameObject);
        }

        public static void SetCurrentUndoGroupName(string name)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.SetCurrentGroupName(name);
#endif
        }

        public static int GetCurrentUndoGroup()
        {
#if UNITY_EDITOR
            return UnityEditor.Undo.GetCurrentGroup();
#else
            return 0;
#endif
        }

        public static void CollapseUndoOperations(int group)
        {
#if UNITY_EDITOR
            UnityEditor.Undo.CollapseUndoOperations(group);
#endif
        }
    }
}