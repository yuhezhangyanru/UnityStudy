using UnityEngine;
using System.Collections;

public class UIUtils : MonoBehaviour {

    public static GameObject FindInChild(GameObject father, string path)
    {
       return  father.transform.FindChild(path).gameObject;
    }
}
