using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CotrolTitle : MonoBehaviour {

    public RectTransform followMouse;
    public Vector3 offset;
    public RectTransform basisObject;
    public Camera came;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        followObject();
	}

    void followObject()
    {
        Vector3 pos = Vector3.Scale(basisObject.position.normalized, (Input.mousePosition + offset));
        pos.z = came.transform.position.z;
        followMouse.position = basisObject.position + came.ScreenToWorldPoint(pos);

    }
}
