using UnityEngine;

namespace TOM.Common.Utils {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        
        // Mutex for thread safety, will not be shared across different subtypes of Singleton
        private static readonly object lockObj = new object();

        void Awake() 
        {
            if(instance == null) {
                // Set the instance to this object and make it persistent across scenes
                instance = this as T;
                DontDestroyOnLoad(this);
                Debug.Log($"Singleton of {typeof(T)} has instance {this.GetInstanceID()}");
            }
            else if(instance != this) {
                // If there is already a Singleton, destroy this object
                Destroy(this.gameObject);
                Debug.LogWarning($"Detected >1 instance of {typeof(T)} Singletons");
            }
        }
        
        // In-case of access from multiple threads, we apply a mutex on the instance getter
        public static T Instance
        {
            get
            {
                lock (lockObj) 
                {
                    // If instance is null, try to find it in the scene
                    if (instance == null) {
                        instance = FindObjectOfType<T>();
                        if (instance == null) {
                            Debug.LogError($"Singleton of {typeof(T)} is absent!");
                        }
                    }
                }
                return instance;
            }
        }

    }
}