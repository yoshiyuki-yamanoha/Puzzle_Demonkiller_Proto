using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{

    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    //                    Debug.LogWarning (typeof(T) + "is nothing");
                    var singletonGameObject = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(singletonGameObject);
                    instance = singletonGameObject.AddComponent<T>();

                }
            }

            return instance;
        }
    }

    protected void Awake()
    {
        DontDestroyOnLoad(gameObject);
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = (T)this;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }

        Destroy(this.gameObject);
        return false;
    }

    protected void OnDestroy()
    {
        instance = null;
    }
}
