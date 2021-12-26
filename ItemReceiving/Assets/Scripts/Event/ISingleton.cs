using UnityEngine;

public class ISingleton<T> : MonoBehaviour where T : Component {
    private static T _instance;
    public static T Instance {
        get {
            if (_instance == null) {
                T[] instances = FindObjectsOfType<T>();
                if (instances.Length > 1) {
                    Debug.LogErrorFormat("Multi component {0} detected", typeof(T).ToString());
                }

                if (instances.Length > 0) {
                    _instance = instances[0];
                }
            }

            if (_instance == null) {
                GameObject newGo = new GameObject();
                _instance = newGo.AddComponent<T>();
                newGo.name = typeof(T).ToString();
            }

            return _instance;
        }
    }
}
