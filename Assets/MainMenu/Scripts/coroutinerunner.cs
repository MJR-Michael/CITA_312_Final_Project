using UnityEngine;

public class GlobalCoroutineRunner : MonoBehaviour
{
    private static GlobalCoroutineRunner instance;

    public static GlobalCoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("GlobalCoroutineRunner");
                instance = obj.AddComponent<GlobalCoroutineRunner>();
                DontDestroyOnLoad(obj);
            }
            return instance;
        }
    }
}
