///调用加载AB的测试入口

using UnityEngine;
namespace Assets.Assets.Scripts.LoadAB
{
    class LoadABTester : MonoBehaviour
    {
        void Start()
        {
            //StartCoroutine 
            //LoadTest();
        }

        void OnGUI()
        {
            if(GUILayout.Button("点击加载资源"))
            {
                LoadTest();
            }
        }

        private void LoadTest()
        {
            Log.MyDebug("开始测试加载！");
            string path = "StreamingAssets/Prefab/MessageView.assetbundle";
            LoadAssetBundle.Instance.LoadAsset(path, Loadcallback);
        }

        private void Loadcallback(GameObject obj, int keyIndex)
        {
            Log.MyDebug(obj.name + "回调执行!key="+keyIndex);
            obj.transform.localPosition = new Vector3(0,10,0);
        }
    }
}
