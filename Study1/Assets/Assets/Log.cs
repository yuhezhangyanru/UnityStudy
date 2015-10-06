
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Assets
{
    class Log:MonoBehaviour
    {
        public static void MyDebug(Object obj, string name)
        {
            Debug.Log(Time.time + "," + obj.GetType().Name + ":" + name);
        }


        public static void MyDebug(string name)
        {
            Debug.Log(Time.time + ":" + name);
        }
    }
}
