using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingGround : MonoBehaviour {

    public float speed=30f;
    public GameObject withSomething;
    public List<GameObject> passPos=new List<GameObject>();
    List<Vector3> paths = new List<Vector3>();

    bool setStart = false;

    // Update is called once per frame
    void Start()
    {
        for (int count = 0; count < passPos.Count; count++)
        {
            paths.Add(passPos[count].transform.position);
        }
        setStart = true;
    }

    private void Update()
    {
        withSomething.transform.position = new Vector3(this.gameObject.transform.position.x, withSomething.transform.position.y, this.gameObject.transform.position.z);

        if(this.gameObject.transform.position==paths[0])
        {
            this.gameObject.transform.DOMove(paths[1], speed, false);
        }
        else if (this.gameObject.transform.position == paths[1])
        {
            this.gameObject.transform.DOMove(paths[2], speed, false);
        }
        else if (this.gameObject.transform.position == paths[2])
        {
            this.gameObject.transform.DOMove(paths[3], speed, false);
        }
        else if (this.gameObject.transform.position == paths[3] || setStart)
        {
            setStart = false;
            this.gameObject.transform.DOMove(paths[0], speed, false);
        }


    }
}
