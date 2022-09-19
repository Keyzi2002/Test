using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerticSingleton <T> : MonoBehaviour
    where T : Component
{ 
    private static T instance;
    public static T Instance 
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = nameof(T).ToString();
                    instance = obj.AddComponent<T>();
                    
                }
                DontDestroyOnLoad(instance);
            }
           
            return instance;
        }
            
        //private set;
    }
    public virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
