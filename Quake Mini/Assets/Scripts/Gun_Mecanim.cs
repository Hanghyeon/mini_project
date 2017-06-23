using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Gun_Kinds
{
    RailGun,
    Rocket,
    SolidGun
}

public enum FireFlow
{
    Lock=-2,
    UnLock,
    Shot,
    Ready,
    Rest
}

public class Gun_Mecanim : MonoBehaviour {

    public static Gun_Mecanim Singleton;
    Vector3 rayOrigin;
    RaycastHit rayHit;

    [SerializeField]
    LineRenderer LR;

    [SerializeField]
    private Camera fpsCam;
    protected Gun_Kinds GunKinds = Gun_Kinds.RailGun;
    [SerializeField]
    protected FireFlow FireState = FireFlow.Lock;

    public FireFlow p_GetFireState { get { return FireState; } }

    float timech = 0f;
    int layerMask = 1 << 9 | 1 << 10;

    public bool isChangeMag = false;

    public List<AudioClip> adClips;
    AudioSource adSour;
    //-------Rail Gun-------
    public float BulletSpeed = 10f;
    public float DemageSize = 100f;
    public float RepeatTime = 3f;
    public float MagTime = 5f;
    public ParticleSystem PS;
    public float weaponRange = 150f;

    List<float> timer = new List<float>();

    public GameObject Bullet;
    Rigidbody rb = null;
    public Transform FireSpot;
    //----------------------
    //------- Rocket -------

    //----------------------
    //-------Solid Gun-------

    //----------------------


    private void Awake()
    {
        LR = this.gameObject.GetComponent<LineRenderer>();
        fpsCam = this.gameObject.GetComponentInParent<Camera>();

        PS = this.gameObject.GetComponentInChildren<ParticleSystem>();
        Singleton = this;
        timer.Add(0f);
        timer.Add(0f);

        adSour = this.gameObject.GetComponent<AudioSource>();

        
    }

    private void Start()
    {
        //LR.SetWidth(0.1f, 0.1f);
        LR.widthMultiplier = 0.1f;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
            StartCoroutine(ShootEffect());

        if (GameManager.Singleton.GS==GameState.Working)
        {
            if(FireState==FireFlow.Lock)
            {
                FireState = FireFlow.Ready;
            }

            if (Input.GetKey(KeyCode.Mouse0) && FireState == FireFlow.Ready && !isChangeMag)
            {
                Shot();
            }
            Rest();

            ChangeMag();
        }
        else
        {
            //Lock gun trigger
            FireState = FireFlow.Lock;
        }
    }

    IEnumerator ShootEffect()
    {

        yield return new WaitForSeconds(RepeatTime);

        LR.enabled = false;
    }


    void Shot()
    {

        if (Player_Data.Singleton.LoadedAmmo > 0)
        {

            LR.enabled = true;

            LR.SetPosition(0, FireSpot.position);

            bool isFine = false;
            rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));


            if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out rayHit, weaponRange, layerMask, QueryTriggerInteraction.Ignore))
            {


                FireState = FireFlow.Shot;
                timer[0] = 0f;

                LR.SetPosition(1, rayOrigin + (fpsCam.transform.forward * rayHit.distance));

                if (rayHit.transform.tag == "Enemy")
                    rayHit.transform.gameObject.GetComponentInParent<Turret_Logic02>().shotAt = true;



                //GameObject temp = Instantiate(Bullet, FireSpot.position, FireSpot.rotation);   // 나중에 오브젝트 풀로 바꿔주자
                //rb = temp.gameObject.GetComponent<Rigidbody>();
                //rb.AddForce(fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, weaponRange)) * BulletSpeed, ForceMode.VelocityChange);




                FireState = FireFlow.Rest;
            }
            else
            {
                FireState = FireFlow.Shot;
                timer[0] = 0f;
                LR.SetPosition(1, rayOrigin + (fpsCam.transform.forward * weaponRange));
                FireState = FireFlow.Rest;
            }
            
            foreach (AudioClip ac in adClips)
            {
                if ("GunFire" == ac.name)
                {
                    isFine = true;
                    adSour.clip = ac;
                    adSour.volume = 0.75f;
                    if (Time.time >= timech)
                    {
                        timech = Time.time + RepeatTime;
                        adSour.Play();
                        Player_Data.Singleton.LoadedAmmo--;
                    }
                }
            }

            


            if (!isFine)
            {
                print("GunFire 파일의 재생을 실패하였습니다." + "코드의 위치는 Gun_Mecanim.cs >>> void Shot() 입니다.");
            }
        }
        else
        {
            foreach (AudioClip ac in adClips)
            {
                if ("GunEmpty" == ac.name)
                {
                    adSour.clip = ac;
                    adSour.volume = 0.75f;
                    if (!adSour.isPlaying)
                        adSour.Play();
                }
            }
            print("Loaded Magazine is empty");
        }
    }

    void Rest()
    {
        if (FireState == FireFlow.Rest)
        {
            timer[0] += Time.deltaTime;
            if (timer[0] >= RepeatTime)
            {
                FireState = FireFlow.Ready;
            }
        }
    }

    void ChangeMag()
    {
        if(isChangeMag)
        {
            

            timer[1] += Time.deltaTime;

            if (timer[1] >= MagTime)
            {
                int temp = 0;

                temp = Player_Data.Singleton.restAmmo;

                if (temp >= Player_Data.Singleton.MagSize)
                {
                    Player_Data.Singleton.restAmmo -= Player_Data.Singleton.MagSize;
                    Player_Data.Singleton.LoadedAmmo = Player_Data.Singleton.MagSize;
                }
                else
                {
                    Player_Data.Singleton.LoadedAmmo = temp;
                    Player_Data.Singleton.restAmmo = 0;
                }

                timer[1] = 0f;
                isChangeMag = false;
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            isChangeMag = true;



            if (Player_Data.Singleton.restAmmo > 0)
            {
                print("Start Change Mag");
                foreach (AudioClip ac in adClips)
                {
                    if ("GunReload" == ac.name)
                    {
                        adSour.clip = ac;
                        adSour.volume = 0.75f;
                        if (!adSour.isPlaying && isChangeMag)
                            adSour.Play();
                    }
                }
            }
            else
            {
                print("empty rest Magazine");
            }
        }
    }

}
