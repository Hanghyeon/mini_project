//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

////public enum TurretState
////{
////    None,
////    Working,
////    Searching,
////    FindedTarget,
////    Died
////}

//public class Turret_Logic : MonoBehaviour {

//    public TurretState TS=TurretState.None;
//    public GameObject Target;
//    public float searchingSpeed = 0.2f;
//    public bool findSomthing = false;
//    public bool findTarget = false;
//    public Camera sight;
//    public float maxDistance = 100f;

//    float timer = 0f;

//    Ray ray;
//    Vector3 rayOrigin;
//    RaycastHit rayHit;

//    private void Start()
//    {
//        TS = TurretState.Working;
//    }

//    private void OnTriggerStay(Collider other)
//    {
//        if(other.gameObject.tag=="Player")
//        {
//            findSomthing = true;
//        }
//    }

//    // Update is called once per frame
//    void Update () {

//        setFindMode(findSomthing);
//        setAttackMode();

//        workingTurret(TS);
//	}


//    void workingTurret(TurretState ts)
//    {
//        switch (ts)
//        {
//            case TurretState.None:
//                break;
//            case TurretState.Working:
//                break;
//            //case TurretState.Searching:
//                this.transform.Rotate(Vector3.up * searchingSpeed);
//                break;
//            case TurretState.FindedTarget:
//                break;
//            case TurretState.Died:
//                break;
//        }
//    }

//    void setFindMode(bool nearThing)
//    {
//        if(nearThing==true)
//        {
//            print("find Mode on");
//            //TS = TurretState.Searching;
//        }
//    }

//    void setAttackMode()
//    {
//        rayOrigin = sight.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

//        if (Physics.Raycast(rayOrigin, sight.transform.forward, out rayHit, maxDistance) && rayHit.transform.gameObject.tag == "Player")
//        {
//            findTarget = true;
//            if (timer <= 100)
//            {
//                timer += Time.deltaTime;
//            }
//        }
//        else
//        {
//            findTarget = false;
//            if (timer > 0f)
//            {
//                timer -= Time.deltaTime;
//            }
//        }

//        if (findTarget || timer > 0f)
//        {
//            this.transform.LookAt(rayHit.transform);
//            print("Detacted Target");
//            TS = TurretState.FindedTarget;
//        }
//        else
//        {
//            print("lost Target");
//            findTarget = false;
//        }

//    }
//}
