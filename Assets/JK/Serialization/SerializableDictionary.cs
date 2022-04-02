using System;
using System.Collections.Generic;
using UnityEngine;

namespace JK.Serialization
{
    /// <summary>
    /// http://answers.unity.com/answers/809221/view.html
    /// </summary>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<TValue> values = new List<TValue>();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            bool areKeysUnityObjects = isTypeUnityObject(typeof(TKey));
            bool areValuesUnityObjects = isTypeUnityObject(typeof(TValue));

            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                if (areKeysUnityObjects && pair.Key == null)
                    continue;
                if (areValuesUnityObjects && pair.Value == null)
                    continue;

                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new ArgumentException(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

            for (int i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
        }

        private bool isTypeUnityObject(Type type)
        {
            return type.IsSubclassOf(typeof(UnityEngine.Object)) || type == typeof(UnityEngine.Object);
        }
    }
}