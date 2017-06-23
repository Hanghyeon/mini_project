using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectRange : MonoBehaviour {
    //public static DetectRange Singleton;
    public bool PlayerGetInSide = false;
    public Transform viaRoateY;
    public GameObject EnterThing;

    void Awake()
    {
        EnterThing = null;
        //singleton = this;
    }

    // Update is called once per frame
    void Update () {
        this.transform.eulerAngles = new Vector3(0f, viaRoateY.rotation.eulerAngles.y, 0f);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            EnterThing = sendTriggerEnterThing(other);
            PlayerGetInSide = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            PlayerGetInSide = false;
    }

    public GameObject sendTriggerEnterThing(Collider other)
    {
        return other.gameObject;
    }
}
