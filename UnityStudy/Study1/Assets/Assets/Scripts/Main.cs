using UnityEngine;
using System.Collections;

public class Main : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (GUILayout.Button("初始化 滑动组件"))
        {
            GameObject scrollObj = GameObject.Find("uiRoot/ScrollArea") as GameObject;
            MyScrollView scrollViewCom = scrollObj.AddMissingComponent<MyScrollView>();
            scrollViewCom.InitComponent(new Vector2(500, 500), ScrollFinishCallback);
        }
    }
    private void ScrollFinishCallback(MoveDirection moveDir)
    {
        Debug.LogError("time:" + Time.time + " move " + moveDir.ToString());
    }
}