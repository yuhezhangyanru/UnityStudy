using UnityEngine;

public enum MoveDirection
{
    toRight= 1,
    toLeft = 2,
    toDown = 3,
    toUp = 4,
    none = 5
}

public class MyScrollView: MonoBehaviour
{
    private UIScrollView _scrollView;
    public delegate void OnDragFnishDelegate(MoveDirection dirction);
    public OnDragFnishDelegate onDragFinishCallback;

    private const float MOVE_MIN = 0.01f;
    private Vector2 _areaSize;

    public void InitComponent( Vector2 areaSize,OnDragFnishDelegate onDragFinishcallback = null)
    {
        _areaSize = areaSize;
        _scrollView = this.GetComponent<UIScrollView>();
        _scrollView.onDragFinished = OnDragFnish;
        onDragFinishCallback = onDragFinishcallback;

        this.GetComponent<UIPanel>().SetRect(0, 0, areaSize.x, areaSize.y);
        this.transform.FindChild("grid").GetComponent<UIGrid>().cellWidth = areaSize.x;
        this.transform.FindChild("grid").GetComponent<UIGrid>().cellHeight = areaSize.y;

        Transform sprTran = this.transform.FindChild("grid/item").transform;
        sprTran.GetComponent<BoxCollider>().size = new Vector3(areaSize.x, areaSize.y, 0f);
        sprTran.GetComponent<UISprite>().width = (int)areaSize.x;
        sprTran.GetComponent<UISprite>().height = (int)areaSize.y;
        sprTran.GetComponent<UISprite>().alpha = 0.1f; // *= new Color(1, 1, 1, 0.1f);
    }

    //拖拽的回调
    private void OnDragFnish()
    {
        Vector3 moveRelative = Vector3.zero; //咱四NGUI更新了UIScrollView的脚本被覆盖了，想要恢复要找到scrollview资源包从里面获取 _scrollView.MLastPos - _scrollView.MStartPos; //移动的相对坐标
        MoveDirection curDir = MoveDirection.none;
        //决定识别水平还是垂直的
        if(Mathf.Abs(moveRelative.x) > Mathf.Abs(moveRelative.y))
        {
            if(Mathf.Abs(moveRelative.x) < MOVE_MIN)
            {
                return ;
            }
            if(moveRelative.x>= MOVE_MIN)
            {
                curDir = MoveDirection.toRight;
            }
            if(moveRelative.x<=-MOVE_MIN)
            {
                curDir = MoveDirection.toLeft;
            }
        }
        else
        {
            if(Mathf.Abs(moveRelative.y) < MOVE_MIN)
            {
                return;
            }
            if(moveRelative.y>=MOVE_MIN)
            {
                curDir = MoveDirection.toUp;
            }
            if(moveRelative.y <= -MOVE_MIN)
            {
                curDir = MoveDirection.toDown;
            }
        } 
        //Log("拖拽完毕=" + moveRelative+",dir="+curDir.ToString());
        //拖拽完毕的回调
        if (onDragFinishCallback != null)
        {
            onDragFinishCallback(curDir);
        }
    }

    protected void Log(string str)
    {
        Debug.Log(Time.time + ":" + str);
    }
}
