using UnityEngine;
using System.Collections;

public class RoleMoveController : MonoBehaviour {

    GameObject obj;
	// Use this for initialization
	void Start () {
        obj = this.transform.FindChild("quangu1").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
