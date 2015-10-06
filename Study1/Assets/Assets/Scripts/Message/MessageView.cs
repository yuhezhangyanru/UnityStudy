using UnityEngine;
using System.Collections;

public class MessageView : MonoBehaviour {

    public GameObject viewObj
    {
        get
        {
            return _viewObj;
        }
    }
    private GameObject _viewObj;
    private UILabel labMsg;

    public void InitComponent(GameObject viewObj)
    {
        this._viewObj = viewObj;
        this.labMsg = viewObj.transform.FindChild("label").GetComponent<UILabel>();
    }


    public void Show(string name)
    {
        _viewObj.SetActive(true);
        this.labMsg.text = name;
        StartCoroutine(delayClose());
    }

    private IEnumerator delayClose()
    {
        yield return new WaitForSeconds(1f);
        _viewObj.SetActive(false);
    }
}
