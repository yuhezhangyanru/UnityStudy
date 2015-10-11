
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Assets
{
    class Log:MonoBehaviour
    {
        public static void MyDebug(Object obj, string name)
        {
            Debug.Log(GetTime() + "," + obj.GetType().Name + "," + name);
        }


        public static void MyDebug(string name)
        {
            Debug.Log(GetTime() + "," + name);
        }

        public static  string GetTime()
        {
            int time = (int) Time.time;
            string minute = time / 60 < 10 ? "0"+time/60:(time/60).ToString();
            string second = time % 60 < 10 ? "0" + time % 60 : (time % 60).ToString();

            return minute +":"+ second;
        }
    }
}
