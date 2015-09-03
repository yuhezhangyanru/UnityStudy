///author：Stephanie
///function：矩阵数据结构，用字典存放行列
///date：2015-6-23 22:53:20
using UnityEngine;
using System.Collections.Generic;
using Assets.Assets;
using System;

public class Matrix
{
    public const float MAX = 10000000f;
    public const float ZERO = 0.000001f; //极小值，认为是0
    //空对象
    private Matrix NULL = null;

    public Dictionary<int, List<float>> data = new Dictionary<int, List<float>>();
    public Matrix()
    {
    }

    //矩阵行数
    public int HangCount
    {
        get
        {
            return this.data.Count;
        }
    }

    //矩阵的列数
    public int LieCount
    {
        get
        {
            //空矩阵
            if (!this.data.ContainsKey(0))
            {
                return 0;
            }
            return this.data[0].Count;
        }
    }

    //矩阵的模
    public float Mode
    {
        get
        {
            bool overflow = false;
            return this.GetModule(out overflow);
        }
    }

    ////重载!=确认两个矩阵不相等:为什么实现了==必须成对重载!=
    //public static bool operator !=(Matrix lMat, Matrix rMat)
    //{
    //    if (lMat.HangCount != rMat.HangCount)
    //        return true;
    //    if (lMat.LieCount != rMat.LieCount)
    //        return true;

    //    List<int> keyList = new List<int>(lMat.data.Keys);
    //    for (int hangindex = 0; hangindex < keyList.Count; hangindex++)
    //    {
    //        List<float> hangA = lMat.GetHang(hangindex);
    //        List<float> hangB = rMat.GetHang(hangindex);
    //        for (int lieindex = 0; lieindex < hangA.Count; lieindex++)
    //        {
    //            if (hangA[lieindex] != hangB[lieindex])
    //                return true;
    //        }
    //    }
    //    return false;
    //}

    /// <summary>
    /// 重载== 检查两个矩阵是否相等
    /// </summary>
    /// <param name="lMat"></param>
    /// <param name="rMat"></param>
    /// <returns></returns>
    public bool Equal(Matrix rMat)
    {
        Matrix lMat = this;
        if (lMat.HangCount != rMat.HangCount)
            return false;
        if (lMat.LieCount != rMat.LieCount)
            return false;

        List<int> keyList = new List<int>(lMat.data.Keys);
        for (int hangindex = 0; hangindex < keyList.Count; hangindex++)
        {
            List<float> hangA = lMat.GetHang(hangindex);
            List<float> hangB = rMat.GetHang(hangindex);
            for (int lieindex = 0; lieindex < hangA.Count; lieindex++)
            {
                //Log.MyDebug("检查元素hangind=" + hangindex + ",lieindex=" + lieindex + ",a=" + hangA[lieindex] + ",b=" + hangB[lieindex]);
                if (hangA[lieindex] != hangB[lieindex])
                    return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 重载矩阵乘法*，
    /// </summary>
    /// <param name="matrixA"></param>
    /// <param name="matrixB"></param>
    /// <returns></returns>
    public static Matrix operator *(Matrix matrixA, Matrix matrixB)
    {
        Matrix resMatrix = new Matrix();
        List<int> keyList = new List<int>(matrixA.data.Keys);
        Dictionary<int, List<float>> dic = matrixA.data;//[keyList[inIndex]];
        //现在是A矩阵的inIndex行，要分别乘以B的inIndex列
        //遍历没一列
        for (int lineIndex = 0; lineIndex < matrixA.LieCount; lineIndex++)
        {
            List<float> resList = new List<float>();
            //即将遍历每一行
            for (int rowIndex = 0; rowIndex < matrixA.HangCount; rowIndex++)
            {
                float rowNum = 0;
                //将A行rowIndex行subIndex列的数字和B中rowIndex列subIndex行的数字相乘
                List<float> aRow = matrixA.GetHang(lineIndex);  //A行的数据
                List<float> bLine = matrixB.GetLie(rowIndex);//B列的数据
                for (int subIndex = 0; subIndex < aRow.Count; subIndex++)
                {
                    rowNum += aRow[subIndex] * bLine[subIndex];
                }
                resList.Add(rowNum);
            }
            resMatrix.data.Add(lineIndex, resList);
        }
        return resMatrix;
    }

    /// <summary>
    /// 计算矩阵A*B，A为this矩阵
    /// </summary>
    /// <param name="matrixB"></param>
    /// <returns></returns>
    //public Matrix GetMultiMtrix(Matrix matrixB)
    //{
    //    Matrix matrixA = this;
    //    Matrix resMatrix = new Matrix();
    //    List<int> keyList = new List<int>(matrixA.data.Keys);
    //    Dictionary<int, List<float>> dic = matrixA.data;//[keyList[inIndex]];
    //    //现在是A矩阵的inIndex行，要分别乘以B的inIndex列
    //    //遍历没一列
    //    for (int lineIndex = 0; lineIndex < GetLineCount(matrixA); lineIndex++)
    //    {
    //        List<float> resList = new List<float>();
    //        //即将遍历每一行
    //        for (int rowIndex = 0; rowIndex < GetRowCount(matrixA); rowIndex++)
    //        {
    //            float rowNum = 0;
    //            //将A行rowIndex行subIndex列的数字和B中rowIndex列subIndex行的数字相乘
    //            List<float> aRow = matrixA.GetHang(lineIndex);  //A行的数据
    //            List<float> bLine = matrixB.GetLie(rowIndex);//B列的数据

    //            for (int subIndex = 0; subIndex < aRow.Count; subIndex++)
    //            {
    //                rowNum += aRow[subIndex] * bLine[subIndex];
    //            }
    //            resList.Add(rowNum);
    //        }
    //        resMatrix.data.Add(lineIndex, resList);
    //    }
    //    return resMatrix;
    //}


    ///这个暂时还不行啊。注释掉
    /// <summary>
    /// 重载+号，计算矩阵相加
    /// </summary>
    /// <param name="lMat"></param>
    /// <param name="rmat"></param>
    /// <returns></returns>
    public static Matrix operator +(Matrix lMat, Matrix rMat)
    {
        if (lMat.HangCount != rMat.HangCount)
        {
            return null;
        }
        if (lMat.LieCount != rMat.LieCount)
        {
            return null;
        }
        //计算 矩阵相加
        Matrix resMat = new Matrix();
        List<int> keyList = new List<int>(lMat.data.Keys);
        for (int index = 0; index < keyList.Count; index++)
        {
            List<float> lines = lMat.data[keyList[index]];
            List<float> linesB = rMat.data[keyList[index]];
            List<float> resList = new List<float>();
            for (int subIndex = 0; subIndex < lines.Count; subIndex++)
            {
                resList.Add(lines[subIndex] + linesB[subIndex]);
            }
            resMat.data.Add(index, resList);
        }
        return resMat;
    }

    /// <summary>
    /// 计算矩阵相加的结果，矩阵不能相加时，返回空矩阵:已经直接可以this+matrixB
    /// </summary>
    /// <returns></returns>
    //public Matrix GetAddMatrix(Matrix matrixB)
    //{
    //    //如果A和B的行列不符合，不能相加
    //    if (this.HangCount != matrixB.HangCount)
    //    {
    //        return null;
    //    }
    //    if (this.LieCount != matrixB.LieCount)
    //    {
    //        return null;
    //    }

    //    //计算 矩阵相加
    //    Matrix resMat = new Matrix();
    //    List<int> keyList = new List<int>(this.data.Keys);
    
    //    for (int index = 0; index < keyList.Count; index++)
    //    {
    //        List<float> lines = this.data[keyList[index]];
    //        List<float> linesB = matrixB.data[keyList[index]];
    //        List<float> resList = new List<float>();
    //        for (int subIndex = 0; subIndex < lines.Count; subIndex++)
    //        {
    //            resList.Add(lines[subIndex] + linesB[subIndex]);
    //        }
    //        resMat.data.Add(index, resList);
    //    }
    //    return resMat;
    //}


    /// <summary>
    /// 获得矩阵的字符串表示：每行以\n分割
    /// </summary>
    /// <returns></returns>
    public string ToString()
    {
        if (data.Count == null)
            return "";

        List<int> keyList = new List<int>(this.data.Keys);
        string str = "";
        for (int index = 0; index < keyList.Count; index++)
        {
            for (int subIndex = 0; subIndex < this.data[keyList[index]].Count; subIndex++)
            {
                str += this.data[keyList[index]][subIndex] + " ";
            }
            str = str.TrimEnd(' ');
            str += "\n";
        }
        return str;
    }


    /// <summary>
    /// 获取矩阵的行数
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    private int GetHangCount()
    {
        return this.data.Count;
    }

    /// <summary>
    /// 获取矩阵的列数
    /// </summary>
    /// <returns></returns>
    private int GetLieCount()
    {
        //空矩阵
        if (!this.data.ContainsKey(0))
        {
            return 0;
        }
        return this.data[0].Count;
    }

    ///***********************************************
    //矩阵内部的工具函数

    //获取矩阵的某行
    protected List<float> GetHang(int rowIndex)
    {
        return data[rowIndex];  //字典的value就是这一行数据的list
    }

    //获取矩阵的某列
    protected List<float> GetLie(int lineIndex)
    {
        List<float> lineList = new List<float>();
        List<int> keyList = new List<int>(data.Keys);
        for (int index = 0; index < keyList.Count; index++)
        {
            lineList.Add(data[keyList[index]][lineIndex]);
        }
        return lineList;
    }

    //获取矩阵的行数
    //private int GetRowCount(Matrix matrix)
    //{
    //    return matrix.data.Count;
    //}

    //获取矩阵的列数
    //private int GetLineCount(Matrix matrix)
    //{
    //    //空矩阵
    //    if (!matrix.data.ContainsKey(0))
    //    {
    //        return 0;
    //    }
    //    return matrix.data[0].Count;
    //}

    /// <summary>
    /// 计算矩阵的模
    /// TODO:模的计算方式是错的！
    /// </summary>
    /// <returns></returns>
    public float GetModule(out bool overflow)
    {
        overflow = false;
        //行数不等于列数，
        if (LieCount != HangCount)
        {
            overflow = true;
            Debug.Log(Time.time + " 行数不等于列数！lieCount=" + LieCount + ",hangcount=" + HangCount);
            return 0;
        }

        //2阶矩阵的模为0 啊
        ///a b
        ///c d
        ///模= a*d - c*b
        if (LieCount == 2)
        {
            return getMultioflist(getRightDownList(0)) - getMultioflist(getLeftUpList(0));
        }

        //测试通过
        List<float> rightDown = new List<float>();//右下各个列的积

        for (int index = 0; index < LieCount; index++)
        {
            rightDown.Add(getMultioflist(getRightDownList(index)));
        }

        List<float> leftUp = new List<float>();
        for (int index = 0; index < LieCount; index++)
        {
            leftUp.Add(getMultioflist(getLeftUpList(index)));
        }

        float result = getSumofList(rightDown) - getSumofList(leftUp);
        Debug.Log("计算结果=" + result.ToString());
        return result;
    }

    /// <summary>
    /// 计算矩阵乘以浮点数:注：会改变原矩阵的值
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Matrix MultiScalar(float value)
    {
        foreach (KeyValuePair<int, List<float>> item in this.data)
        {
            for (int index = 0; index < item.Value.Count; index++)
            {
                item.Value[index] *= value;
            }
        }
        return this;
    }

    /// <summary>
    /// 获得转置矩阵，不改变原矩阵
    /// </summary>
    /// <returns></returns>
    public Matrix GetTranspose()
    {
        //非方阵
        if (HangCount != LieCount)
        {
            return null;
        }
        Matrix transpose = new Matrix();
        List<int> keyList = new List<int>(data.Keys);
        for (int index = 0; index < keyList.Count; index++)
        {
            List<float> hang = new List<float>(data[keyList[index]]);
            for (int subIndex = 0; subIndex < hang.Count; subIndex++)
            {
                if (transpose.data.ContainsKey(subIndex))
                {
                    transpose.data[subIndex].Add(hang[subIndex]);
                }
                else
                {
                    transpose.data.Add(subIndex, new List<float> { hang[subIndex] });
                }
            }
        }
        return transpose;
    }

    /// <summary>
    /// 获取N阶的单位矩阵
    /// </summary>
    /// <param name="order"></param>
    /// <returns></returns>
    public static Matrix GetUnitMatrixN(int N)
    {
        Matrix resMat = new Matrix();
        for (int index = 0; index < N; index++)
        {
            List<float> line = new List<float>();

            for (int subIndex = 0; subIndex < N; subIndex++)
            {
                if (index == subIndex)
                {
                    line.Add(1);
                }
                else
                {
                    line.Add(0);
                }
            }
            resMat.data.Add(index, line);
        }
        return resMat;
    }

    /// <summary>
    /// 获取矩阵的数字形式（只有一个元素的！）
    /// </summary>
    /// <returns></returns>
    public float GetMatrixToInt()
    {
        foreach (KeyValuePair<int, List<float>> pair in data)
        {
            return pair.Value[0];
        }
        return MAX;
    }

    //检查一个矩阵是否正交？
    public bool CheckOrthogonal()
    {
        Matrix lMat = this;
        if (lMat == NULL)
            return false;

        if (lMat.HangCount == 1 || lMat.LieCount == 1)
            return false;
        Matrix rMat = this.GetTranspose();
        if (rMat == NULL)
            return false;
        Matrix res = lMat * rMat;

        if (res == NULL)
            return false;

        Matrix resOther = GetUnitMatrixN(res.HangCount);
        return (res == resOther);
    }

    /// <summary>
    /// 获得一个矩阵的逆矩阵:如果是，返回的逆矩阵不为空
    /// 注：如果一个矩阵有逆矩阵，那矩阵和逆矩阵相乘结果等于单位矩阵
    /// </summary>
    /// <returns></returns>
    public Matrix GetInverseMatrix()
    {
        //step1:先筛选：某行或某列全为0的矩阵没有逆矩阵（因为乘自己得到的结果是0矩阵）
        if (CheckZeroList() == false)
            return null;

        //几个简化原则：
        //1.单位矩阵的逆矩阵是本身
        if (GetUnitMatrixN(this.HangCount) == this)
            return this;
        ///其他性质
        ///2.A是奇异矩阵的话，他的逆矩阵的逆等于自己
        ///3.矩阵转置的逆等于矩阵的逆的转置
        ///4.（AB）-1 = (B-1)*(A-1)  -1表示-1次方，即矩阵乘积的逆等于反顺序的矩阵逆的乘积

        //step2:先得到：代数余子式的矩阵
        //GetConfactorMat();

        //step3:余子式的矩阵要转置:得到A的标准伴随矩阵
        Matrix resMat = GetConfactorMat().GetTranspose();

        //step3:矩阵除以原矩阵的模
        float moderate = 1f / this.Mode;
        //Log.MyDebug("原矩阵的1/模=" + moderate);

        return resMat.MultiScalar(moderate);
    }

    //A的标准伴随矩阵
    public Matrix GetStanderedAdjoint()
    {
        return GetConfactorMat().GetTranspose();
    }

    //获取代数余子式组成的矩阵
    public Matrix GetConfactorMat()
    {
        Matrix resMat = new Matrix();
        for (int hangInd = 0; hangInd < HangCount; hangInd++)
        {
            List<float> list = GetHang(hangInd);
            List<float> resList = new List<float>();
            for (int lieInd = 0; lieInd < list.Count; lieInd++)
            {
                //获取index行，subindex列的代数余子式的模啊
                float sign = (hangInd + lieInd) % 2 == 0 ? 1f : -1f;//结果的符号
                float mode = GetConfactor(hangInd, lieInd).Mode;
                resList.Add(sign * mode);
            }
            resMat.data.Add(hangInd, resList);
        }
        return resMat;
    }


    /// <summary>
    /// 获得矩阵某行某列的代数余子式(去掉nHang行nLie列的最终子矩阵)
    /// </summary>
    /// <param name="nHang"></param>
    /// <param name="nLie"></param>
    /// <returns></returns>
    private Matrix GetConfactor(int nHang, int nLie)
    {
        Matrix resMat = new Matrix();
        int key = 0;
        for (int hangInd = 0; hangInd < HangCount; hangInd++)
        {
            if (hangInd == nHang)
            {
                continue;
            }
            List<float> list = GetHang(hangInd);
            List<float> resultList = new List<float>();
            for (int lieIndex = 0; lieIndex < list.Count; lieIndex++)
            {
                if (lieIndex == nLie)
                {
                    continue;
                }

                resultList.Add(list[lieIndex]);
            }
            resMat.data.Add(key, resultList);
            key++;
        }
        return resMat;
    }
    /// <summary>
    /// 
    /// 注意：检查某行或某列全为0
    /// </summary>
    /// <returns></returns>
    public bool CheckZeroList()
    {
        for (int index = 0; index < HangCount; index++)
        {
            int count = 0;
            List<float> list = GetHang(index);
            for (int subIndex = 0; subIndex < list.Count; subIndex++)
            {
                if (list[subIndex] >= -ZERO && list[subIndex] <= ZERO)
                {
                    count++;
                }
            }
            if (count == list.Count)
                return false;
        }
        for (int index = 0; index < LieCount; index++)
        {

            int count = 0;
            List<float> list = GetLie(index);
            for (int subIndex = 0; subIndex < list.Count; subIndex++)
            {
                if (list[subIndex] >= -ZERO && list[subIndex] <= ZERO)
                {
                    count++;
                }
            }
            if (count == list.Count)
                return false;
        }
        return true;
    }

    //TODO测试函数可删除！
    //private string getListtoStr(List<float> list)
    //{
    //    if(list ==null)
    //    {
    //        return "空";
    //    }
    //    string temp = "";
    //    for(int index=0;index <list.Count;index ++)
    //    {
    //        temp += list[index].ToString() + ",";
    //    }
    //    return temp;
    //}

    private int addIndex(int num1, int num2)
    {
        return num1 + num2 >= LieCount ? ((num1 + num2) % LieCount) : (num1 + num2);
    }


    //获得矩阵的第i右下列
    private List<float> getRightDownList(int needLine)
    {
        //遍历矩阵的所有列
        List<float> tempList = new List<float>();
        for (int index = 0; index < GetLieCount(); index++)
        {
            List<float> line = this.GetHang(index);
            for (int subIndex = 0; subIndex < line.Count; subIndex++)
            {
                if (subIndex == addIndex(index, needLine))
                {
                    tempList.Add(line[subIndex]);
                }
            }
        }
        return tempList;
    }

    //获得矩阵的第i右上列
    private List<float> getLeftUpList(int needLine)
    {
        //遍历矩阵的所有列
        List<float> tempList = new List<float>();
        for (int index = GetLieCount() - 1; index >= 0; index--)
        {
            List<float> line = this.GetHang(index);
            for (int subIndex = 0; subIndex < line.Count; subIndex++)
            {
                if (index + addIndex(subIndex, needLine) == LieCount - 1)
                {
                    tempList.Add(line[subIndex]);
                }
            }
        }
        return tempList;
    }

    //获得一个list元素的积
    private float getMultioflist(List<float> dataList)
    {
        float result = 1;
        for (int index = 0; index < dataList.Count; index++)
        {
            result *= dataList[index];
        }
        return result;
    }

    //获得一个列表元素的和
    private float getSumofList(List<float> dataList)
    {
        float sum = 0;
        for (int index = 0; index < dataList.Count; index++)
        {
            sum += dataList[index];
        }
        return sum;
    }

    //获得下一个右下索引，溢出则获取最大索引
    private int getRightInd(int curInd, int add)
    {
        return (curInd + add) >= LieCount ? 0 : (curInd + add);
    }

    //获得keylist
    private List<int> GetKeyList()
    {
        return new List<int>(data.Keys);
    }
}