using UnityEngine;

public class CameraTargetSingelton : MonoBehaviour
{
    public static CameraTargetSingelton Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Warning: Multiple instances of CameraTargetSingleton detected. Destroying new instance.", Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

}