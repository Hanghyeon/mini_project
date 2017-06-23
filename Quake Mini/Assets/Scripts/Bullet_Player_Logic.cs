using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Player_Logic : MonoBehaviour {

    //void Awake()
    //{

    //}
    //// Use this for initialization
    //void Start()
    //{

    //}
    int test = 0;


    private void Update()
    {
        test++;
    }


    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                other.gameObject.GetComponentInParent<Turret_Logic02>().shotAt = true;
                this.gameObject.SetActive(false);
                break;

            case "Ground":
                this.gameObject.SetActive(false);
                break;

            default:
                print("Hit: " + other.gameObject.name);
                //this.gameObject.SetActive(false);
                break;
        }
    }

 //   // Update is called once per frame
 //   void Update () {
		
	//}
}
