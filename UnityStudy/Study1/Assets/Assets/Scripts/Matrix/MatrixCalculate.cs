using UnityEngine;
using System.Collections.Generic;
using Assets.Assets;

public class MatrixCalculate : MonoBehaviour
{

    private Transform _viewTran;
    private UIButton _btnCalculate; //计算按键:暂时计算矩阵的乘法
    private UIInput _inpMatrix1;
    private UIInput _inpMatrix2;
    private UILabel _labResult;


    private List<Matrix> _matrixList;

    private List<UIInput> _inpAreaList;

    private string[] matrixName = { "A", "B" };
    private UIButton _btnChoose;//选择计算公式的按钮
    private GameObject _subViewObj;
    private UIButton _btnSubViewClose;
    private Dictionary<UIButton, string> _btnDic;

    private enum CalType
    {
        ABmulti = 1,         //A*B
        ABadd = 2,           //A+B
        MatNorm = 3,         //矩阵的模
        MultScalar = 4,      //矩阵和标量相乘
        Transpose = 5,       //矩阵的转置
        UnitMatrix = 6,      //单位矩阵
        StandardAdjoint = 7, //A的标准伴随矩阵
        InverseMat = 8,      //逆矩阵：A矩阵未必存在逆矩阵
        CheckEquqal = 9,     //检查A和B矩阵相等
        CheckOrthogonal = 10,//检查A矩阵正交吗
    }
    private CalType _curType = CalType.ABmulti;
    private const int BUTTON_COUNT = 10;  //按钮个数

    //初始化组件
    public void InitComponent(GameObject viewObj)
    {
        this._viewTran = viewObj.transform;

        _inpAreaList = new List<UIInput>();
        _btnDic = new Dictionary<UIButton, string>();

        _btnCalculate = _viewTran.FindChild("center/btn_calculate").GetComponent<UIButton>();
        _inpMatrix1 = _viewTran.FindChild("center/input1").GetComponent<UIInput>();
        _inpMatrix2 = _viewTran.FindChild("center/input2").GetComponent<UIInput>();
        _labResult = _viewTran.FindChild("center/lab_result").GetComponent<UILabel>();
        _btnChoose = _viewTran.FindChild("center/btn_choose").GetComponent<UIButton>();
        _btnSubViewClose = _viewTran.FindChild("center/btn_choose/sub_panel/spr_bg").GetComponent<UIButton>();
        _subViewObj = _viewTran.FindChild("center/btn_choose/sub_panel").gameObject;

        //初始化操作列表
        for (int index = 0; index < BUTTON_COUNT; index++)
        {
            UIButton btnSub = _viewTran.FindChild("center/btn_choose/sub_panel/btn_sub" + index).GetComponent<UIButton>();
            UIEventListener.Get(btnSub.gameObject).onClick = OnClickSubBtn;
            _btnDic.Add(btnSub, btnSub.transform.FindChild("label").GetComponent<UILabel>().text);
        }
        UIEventListener.Get(_btnCalculate.gameObject).onClick = OnClickCalMulti;
        UIEventListener.Get(_btnChoose.gameObject).onClick = OnClickBtnChoose;
        UIEventListener.Get(_btnSubViewClose.gameObject).onClick = OnClickSubViewClose;

        _inpMatrix1.gameObject.SetActive(false);
        _inpMatrix2.gameObject.SetActive(false);
        _inpAreaList.Add(_inpMatrix1);
        _inpAreaList.Add(_inpMatrix2);

        Debug.Log(Time.time + "组件初始化完毕,可以矩阵乘法!");
        //激活2个矩阵
        for (int index = 0; index < _inpAreaList.Count; index++)
        {
            _inpAreaList[index].gameObject.SetActive(true);
        }

        //测试数据啊啊啊
        //默认显示A*B
        UpdateChooseType("矩阵A的模");
    }

    private List<Matrix> GetStrtoMatrix()
    {
        //先解析输入内容
        _matrixList = new List<Matrix>();
        _matrixList.Add(new Matrix());
        _matrixList.Add(new Matrix());

        for (int outIndex = 0; outIndex < _inpAreaList.Count; outIndex++)
        {
            //将字符串解析成矩阵
            string strTemp = _inpAreaList[outIndex].value;
            strTemp = strTemp.Replace("\n\n", "\n");
            
            //测试的输入数据

            string[] lines = strTemp.Split('\n');
            int rowCount = 0;
            //解析每一行
            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                if (lines[lineIndex] == " " || lines[lineIndex] == "\n")
                {
                    continue;
                }
                //每一行还是 1，1，1
                string[] linedata = lines[lineIndex].Split(' ');
                if (rowCount == 0)
                {
                    rowCount = linedata.Length;
                }
                if (rowCount != linedata.Length)
                {
                    Log.MyDebug(this, "输入矩阵A行不等于B列,row=" + rowCount + ",line=" + linedata.Length + "，return");
                    return null;
                }

                List<float> lineItem = new List<float>();
                for (int dataIndex = 0; dataIndex < linedata.Length; dataIndex++)
                {
                    float result = 0;
                    linedata[dataIndex] = linedata[dataIndex].Replace(" ", "");
                    if (float.TryParse(linedata[dataIndex], out result) == false &&
                        strTemp != string.Empty)
                    {
                        Log.MyDebug(this, "输入矩阵异常！，return" + linedata[dataIndex] + ",src=" + strTemp + ".");
                        return null;
                    }
                    lineItem.Add(result);
                }
                //Log.MyDebug(this, "out int=" + outIndex + " 解析矩阵，line=" + lineIndex);
                _matrixList[outIndex].data.Add(lineIndex, lineItem);
            }
        }
        Log.MyDebug(this, "输入矩阵=\n" + _matrixList[0].ToString());

        return _matrixList;
    }

    //点击了计算乘法
    private void OnClickCalMulti(GameObject go)
    {
        GetStrtoMatrix();

        CalTypeSwitch();

        //暂时不清理数据！

        //for (int index = 0; index < inpAreaList.Count; index++)
        //{
        //    inpAreaList[index].value = "输入矩阵 " + matrixName[index] + "...";
        //}
    }

    //根据当前操作选项决定计算什么
    private void CalTypeSwitch()
    {
        switch (_curType)
        {
            case CalType.ABmulti:
                {
                    CalABMultiplication();
                } break;
            case CalType.ABadd:
                {
                    CalABAdd();
                } break;
            case CalType.MatNorm:
                {
                    bool resultInvalid = false;
                    UpdateLabRes("矩阵A的模=" + _matrixList[0].GetModule(out resultInvalid));
                    if (resultInvalid == true)//表示结果不可靠，矩阵不规范或结果异常
                    {
                        UpdateLabRes("A矩阵非方阵");
                    }
                } break;
            case CalType.MultScalar: //矩阵乘以标量
                {
                    float scalar = 0;
                    string str = _matrixList[1].ToString();
                    str = str.Replace(",", "");
                    if (float.TryParse(str, out scalar) == false)
                    {
                        MessageShow.Instance.Show("输入B参数非标量！=" + str);
                        return;
                    }
                    Matrix result = _matrixList[0].MultiScalar(scalar);
                    UpdateLabRes(result.ToString());
                } break;
            case CalType.Transpose:
                {
                    Matrix result = _matrixList[0].GetTranspose();
                    if (result == null)
                    {
                        UpdateLabRes("矩阵A非方阵");
                        return;
                    }
                    UpdateLabRes(result.ToString());
                } break;
            case CalType.UnitMatrix:
                {
                    int jie = (int)_matrixList[0].GetMatrixToInt();
                    if (jie == Matrix.MAX)
                    {
                        UpdateLabRes("输入原阶数异常！");
                        return;
                    }
                    UpdateLabRes(Matrix.GetUnitMatrixN(jie).ToString());
                } break;
            case CalType.StandardAdjoint:
                {
                    UpdateLabRes(_matrixList[0].GetStanderedAdjoint().ToString());
                } break;
            case CalType.InverseMat: {//A矩阵的逆
                UpdateLabRes(_matrixList[0].GetInverseMatrix().ToString());
            } break;
            case CalType.CheckEquqal: {
                bool equal = (_matrixList[0] == _matrixList[1]);
                UpdateLabRes(equal.ToString());
            } break;
            case CalType.CheckOrthogonal: {
                bool isOrthogonal = _matrixList[0].CheckOrthogonal();
                UpdateLabRes(isOrthogonal.ToString());
            } break;
            default:
                {
                    Log.MyDebug(this, "未定义的计算！return");
                    return;
                }
        }
    }

    private void UpdateLabRes(string text)
    {
        _labResult.text = text;
        Log.MyDebug("结果区=" + _labResult.text);
    }

    //计算A+B矩阵
    private void CalABAdd()
    {
        Matrix matrixA = _matrixList[0];
        Matrix matrixB = _matrixList[1];
        Matrix resMat = matrixA + matrixB;//matrixA.GetAddMatrix(matrixB);  //获得AB相加结果，
        if (resMat == null)
        {
            MessageShow.Instance.Show("输入数据不合法！");
            return;
        }
        UpdateLabRes(resMat.ToString());
    }

    //计算A*B矩阵
    private void CalABMultiplication()
    {
        //解析完输入串
        Matrix matrixA = _matrixList[0];
        Matrix matrixB = _matrixList[1];
        Matrix resMatrix = matrixA * matrixB;
        UpdateLabRes(resMatrix.ToString());
    }

    //打开关闭公式选项
    private void OnClickBtnChoose(GameObject go)
    {
        SetSubViewActive(!_subViewObj.activeInHierarchy);
    }

    //关闭子面板
    private void OnClickSubViewClose(GameObject go)
    {
        SetSubViewActive(false);
    }

    //是否显示子面板?
    private void SetSubViewActive(bool active)
    {
        _subViewObj.SetActive(active);
        if (active)
        {
            _subViewObj.transform.localPosition = new Vector3(0, -160, 0);
        }
    }

    //点击了某个子选项
    private void OnClickSubBtn(GameObject go)
    {
        Log.MyDebug(this, "点击了按钮：" + go.name + ",公式=" + _btnDic[go.GetComponent<UIButton>()]);
        MessageShow.Instance.Show("选择矩阵计算：" + _btnDic[go.GetComponent<UIButton>()]);
        UpdateChooseType(_btnDic[go.GetComponent<UIButton>()]);
        SetSubViewActive(false);
    }

    //指定当前的公式
    private void UpdateChooseType(string str)
    {
        switch (str)
        {
            case "A*B":
                {
                    _curType = CalType.ABmulti;
                } break;
            case "A+B":
                {
                    _curType = CalType.ABadd;
                } break;
            case "矩阵A的模":
                {
                    _curType = CalType.MatNorm;
                } break;
            case "矩阵A乘标量(B框)":
                {
                    _curType = CalType.MultScalar;
                } break;
            case "矩阵A的转置":
                {
                    _curType = CalType.Transpose;
                } break;
            case "求N阶段单位矩阵":
                {
                    _curType = CalType.UnitMatrix;
                } break;
            case "A的标准伴随矩阵":
                {
                    _curType = CalType.StandardAdjoint;
                } break;
            case "求A的逆矩阵":
                {
                    _curType = CalType.InverseMat;
                } break;
            case "检查A和B矩阵相等":
                {
                    _curType = CalType.CheckEquqal;
                } break;
            case "检查A矩阵正交吗": {
                _curType = CalType.CheckOrthogonal;
            } break;
        }
        _btnChoose.transform.FindChild("label").GetComponent<UILabel>().text = str;
    }
}