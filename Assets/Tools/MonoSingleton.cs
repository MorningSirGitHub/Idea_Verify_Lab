/// <summary>
/// 通用单例,用于创建物体
/// </summary>
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{

    private static T m_Instance = null;

    public static T instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (m_Instance == null)
                {
                    m_Instance = new GameObject("Singleton of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    //m_Instance.Init();
                    m_Instance.gameObject.transform.SetSiblingIndex(0);
                }

            }
            return m_Instance;
        }
    }

    private void Awake()
    {

        if (m_Instance == null)
        {
            m_Instance = this as T;
        }
    }

    public virtual void Init() { }


    protected virtual void OnApplicationQuit()
    {
        m_Instance = null;
    }

    protected virtual void OnDestroy()
    {
        if (m_Instance == this)
        {
            m_Instance = null;
        }
    }
}