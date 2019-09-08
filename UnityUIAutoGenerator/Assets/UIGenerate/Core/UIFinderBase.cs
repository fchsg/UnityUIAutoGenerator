using UnityEngine;

namespace UIGenerate.Core
{
    public class UIFinderBase
    {
        private GameObject Base { get; set; }

        protected virtual void Init(Transform trans)
        {
            Base = trans.gameObject;
        }
        
        public T GetComponent<T>(GameObject go) where T : Component
        {
            T t = go.GetComponent<T>();
            if (t == null)
                t = go.AddComponent<T>();
            return t;
        }
        
        public void RemoveComponent<T>(GameObject go) where T : Component
        {
            var t = go.GetComponent<T>();
            if (t == null) return;
            Object.Destroy(t);
        }
    }
}

