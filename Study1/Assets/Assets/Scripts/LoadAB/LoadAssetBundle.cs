using Assets.Assets;
using System.Collections;
using System.Collections.Generic;
///author:Stephanie
///function:加载AssetBundle资源并实例化对象出来，放缓存
///date:2015-10-7 16:08:55
using UnityEngine;

namespace Assets.Assets.Scripts.LoadAB
{
    //这里加载完对象携带obj信息外，
    //加上loadKey(加载时的唯一key):保留参数,暂时不用
    public delegate void LoadAssetCallback(GameObject obj = null, int loadKey = 0);

    //资源结构
    public class AssetStruct
    {
        public int loadKey;
        public string loadUrl;
        public LoadAssetCallback loadCallback;
        public AssetStruct(int koadKey, string loadurl, LoadAssetCallback loadCallback)
        {
            this.loadKey = koadKey;
            this.loadUrl = loadurl;
            this.loadCallback = loadCallback;
        }
    }

    public class LoadAssetBundle
    {
        private static LoadAssetBundle _instance = null;
        public static LoadAssetBundle Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LoadAssetBundle();
                return _instance;
            }
        }


        //维护加载时的索引
        private int _index = 0;

        //维护请求加载的队列
        private Dictionary<int, AssetStruct> _loadingDic = new Dictionary<int, AssetStruct>();

        //根目录的路径
        public static readonly string PathURL =
#if UNITY_ANDROID
		"jar:file:///" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
 "file:///" + Application.dataPath + "/";
#else
        string.Empty;
#endif
        //外部加载资源时调用的接口
        public void LoadAsset(string loadUrl, LoadAssetCallback callback)
        {
            if (loadUrl == string.Empty)
            {
                Log.MyDebug("加载地址为空！");
                return;
            }
            loadUrl = PathURL + loadUrl;


            Log.MyDebug("开始加载地址=" + loadUrl);
            _index++;
            _loadingDic.Add(_index, new AssetStruct(_index, loadUrl, callback));


            CoroutineMgr.Instance.StartTask(LoadAsset(loadUrl, _index));
        }

        private IEnumerator LoadAsset(string path, int key)
        {
            Log.MyDebug("加载的路径=" + path + ",key=" + key);
            WWW www = WWW.LoadFromCacheOrDownload(path, 1);
 
            yield return www;

            if (www.error != null)
            {
                Log.MyDebug("error=" + www.error);
            }
            else
            {
                AssetBundle bundle = www.assetBundle;
                GameObject obj = bundle.LoadAll()[0] as GameObject;
              
                if (_loadingDic.ContainsKey(key))
                {
                    AssetStruct item = _loadingDic[key]; 
                    Log.MyDebug(path + "加载完毕!key=" + key);
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one;

                    if (item.loadCallback != null)
                    {
                        item.loadCallback(obj, key);
                    }

                    _loadingDic.Remove(key);
                }
                www.assetBundle.Unload(false);
            }
        }
    }
}