using System.Collections;
using UnityEngine;

namespace Assets.Assets.Scripts.LoadAB
{ 
    class CoroutineMgr
    {
        private static CoroutineMgr _instance = null;
        public static CoroutineMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CoroutineMgr();
                    _instance._obj = new GameObject();
                    _instance._obj.AddMissingComponent<CoroutineDeal>();
                }
                return _instance;
            }
        }
        private GameObject _obj;

        public void StartTask(IEnumerator routine)
        {
            //StartCoroutine(
            //public Coroutine StartCoroutine(IEnumerator routine);
            //GameObject obj = new GameObject();
            CoroutineDeal com = _obj.GetComponent<CoroutineDeal>();
             com.StartTask(routine);
            //Coroutine task = new Coroutine();
        }
    }


    class CoroutineDeal: MonoBehaviour
    {

        public void StartTask(IEnumerator routine)
        {  
            StartCoroutine(routine); 
        }
    }
}
