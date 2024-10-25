using System;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this as T)
            Destroy(this);
        else
            Instance = GetComponent<T>();
    }
}