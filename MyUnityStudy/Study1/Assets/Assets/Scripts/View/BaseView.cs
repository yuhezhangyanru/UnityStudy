///author:张燕茹
///function: 视图基类
///date:2015-6-2 22:52:11
///
using UnityEngine;
namespace Assets.Assets.Scripts.View
{ 
    public class BaseView:MonoBehaviour//:BaseView<T> :where T : Component , viewInt
    {
        public GameObject gameObject;
        private static GameObject _instance = null;
        public static GameObject Instance
        {
            get {
                if(_instance ==null)
                {
                    _instance = new GameObject();
                }
                return _instance;
            }
            set {
                _instance = value;
            }
        }
        
        public virtual string url { get { return null; } }
 
        public void OpenView()
        {
            InitView();
            Init();
            HandleAfterOpenView();
        }
        private void InitView()
        {
            Log("要初始化的界面=" + url+", class="+this.name);
           gameObject = GameObject.Find("uiRoot/" + url) as GameObject;
        }

        protected virtual void Init()
        {
        }

        protected virtual void HandleAfterOpenView()
        {
        }

        protected virtual void HandleBeforeCloseView()
        {
        }

        protected virtual void Log(string str)
        {
            Debug.Log(Time.time + ":" + str);
        }
    }
}