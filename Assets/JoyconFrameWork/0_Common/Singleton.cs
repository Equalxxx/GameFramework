using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_Instance;
    
    public static T Instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = (T)FindObjectOfType(typeof(T));

                if (s_Instance == null)
                {
                    var singletonObject = new GameObject();
                    s_Instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString();
                }
            }

            return s_Instance;
        }
    }
}