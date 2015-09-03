///author：Stephanie
///functiion：显示tip的接口
///date：2015-6-22 20:38:15

using Assets.Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class MessageShow : Singleton<MessageShow> {

    private List<MessageView> msgObjList = new List<MessageView>();

    //初始化信息
    private void InitList()
    {
        if(msgObjList.Count!=0)
        {
            return;
        }
        //GameObject go = GameObject.Instantiate.Load("Assets/Prefab/MessageView") as GameObject) as GameObject;
        GameObject go = GameObject.Find("uiRoot/MessageView").gameObject;
        MessageView msgView = go.AddMissingComponent<MessageView>();
        msgView.InitComponent(go);
        //msgView.
        msgObjList.Add(msgView);

        go.transform.localPosition = Vector3.zero;
        go.SetActive(false);
    }

    public void Show(string str)
    {
        InitList();
        //显示字符串
        for(int index=0;index < msgObjList.Count;index ++)
        {
            msgObjList[index].viewObj.SetActive(false);
        }
        msgObjList[0].Show(str);
    }
}
