using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns
{ 

    public abstract class Jigsaw_Singleton<T> : MonoBehaviour where T : Component
    {
        private static T s_instance;
    

        public static T Instance
        {
            get
            {
                // 인스턴스가 null이면, 새로운 인스턴스를 생성하도록 변경
                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<T>();
                    if (s_instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        s_instance = obj.AddComponent<T>();
                    }
                }
                return s_instance;
            }
        }

        protected void Awake()
        {
            if(s_instance == null)
            {
                s_instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}