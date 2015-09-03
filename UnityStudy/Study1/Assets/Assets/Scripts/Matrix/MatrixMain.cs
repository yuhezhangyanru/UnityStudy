using UnityEngine;
using System.Collections;

public class MatrixMain : MonoBehaviour {

    GameObject viewObj;

    void Start () {
        viewObj = GameObject.Find("uiRoot/view").gameObject;

        MatrixCalculate matrix = viewObj.AddMissingComponent<MatrixCalculate>();
        matrix.InitComponent(viewObj);
    }
}
