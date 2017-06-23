using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_defualt_Logic : MonoBehaviour {

    public float BulletDamage = 20f;


    //void Awake()
    //{

    //}
    //// Use this for initialization
    //void Start()
    //{

    //}

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                Player_Data.Singleton.getDamage(20);
                this.gameObject.SetActive(false);
                break;

            case "Enemy":
                this.gameObject.SetActive(false);
                
                break;

            case "Ground":
                this.gameObject.SetActive(false);
                break;

            default:
                print("Hit: " + other.gameObject.name);
                this.gameObject.SetActive(false);
                break;
        }
    }

 //   // Update is called once per frame
 //   void Update () {
		
	//}
}
