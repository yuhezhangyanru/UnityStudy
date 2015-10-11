using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
///functtion:把资源打包成AssetBundle，选中游戏对象后菜单右键执行
/// 
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class BuildAssetBundle : EditorWindow
{ 
    //执行打包的菜单:菜单放到Assets层级下即可右键出现
    [MenuItem("Assets/BuildAssetBundle")]
    static void Init()
    {
        EditorWindow window = GetWindow(typeof(BuildAssetBundle));
        window.title = "打包资源";
        window.position = new Rect(200, 200, 200, 200); 
    }

    void OnGUI()
    {
        if (GUILayout.Button("打包到Windows平台"))
        {
            CheckBuild(BuildTarget.StandaloneWindows);
        }
        if(GUILayout.Button("打包到Android平台"))
        {
            CheckBuild(BuildTarget.Android);
        }
    }

    private void CheckBuild(BuildTarget targetPlatform)
    {
        List<Object> prefabList = new List<Object>();
        Object[] choosedList = Selection.GetFiltered(typeof(GameObject), SelectionMode.Editable | SelectionMode.TopLevel);
        if (choosedList.Length < 1)
        {
            Log("请先选中打包对象！");
            return;
        }
        for (int index = 0; index < choosedList.Length; index++)
        {
            prefabList.Add(choosedList
                [index]);
        }
        if (Selection.activeObject != null)
        {
            Log("开始准备打包，选中的=" + Selection.activeObject.name);
            BuildAB(this, prefabList, targetPlatform);
        }
        else
        {
            Log("打包前先选中某个预设！");
        }
    }

    private void BuildAB(BuildAssetBundle window, List<Object> prefabList, BuildTarget targetPlatform)
    {
        for (int index = 0; index < prefabList.Count; index++)
        {
            string curPath = AssetDatabase.GetAssetPath(prefabList[index]);

            curPath = curPath.Replace("Assets/", "");
            curPath = curPath.TrimEnd(prefabList[index].name.ToCharArray());
            curPath = curPath.TrimEnd(".prefab".ToCharArray());

            //把打包的结果放到StreamingAsset目录
            string dstDir = Application.dataPath + "/StreamingAssets/" + curPath;

            DirectoryInfo dicInfo = new DirectoryInfo(dstDir);
            if (!dicInfo.Exists)
            {
                dicInfo.Create();
            }
            string dstPath = dstDir + "/" + prefabList[index].name + ".assetbundle";
            Log("index=" + index + ",srcPath=" + curPath +
                "\n ,curpath=" + curPath +
                "\n ,dstpath=" + dstDir +
                "\n ,data=" + Application.dataPath +
                ",\ndstname=" + dstPath
                + ",name=" + prefabList[index].name);


            //执行打包
            if (BuildPipeline.BuildAssetBundle(prefabList[index], null, dstPath,
                UnityEditor.BuildAssetBundleOptions.CollectDependencies |
                UnityEditor.BuildAssetBundleOptions.CompleteAssets |
                UnityEditor.BuildAssetBundleOptions.DeterministicAssetBundle,
              targetPlatform))//打包的目标平台
            {
                Log(prefabList[index].name + " 打包成功！");
            }
            else
            {
                Log(prefabList[index].name + " 打包失败！");
            }
        }
    }
     
    static void Log(string str)
    {
        UnityEngine.Debug.Log(Time.time + ":" + str);
    }
}