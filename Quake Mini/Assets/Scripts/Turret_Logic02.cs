using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum TurretState
{
    None,
    Working,
    DefenceMode,
    FindedTarget,
    Died
}

public class Turret_Logic02 : MonoBehaviour {

    public List<AudioClip> audioClip;
    public AudioSource audioSource;
    Turret_Sound_system TSS = null;
    //오디오 소스 2개 추가해야됨
    AudioSource as03 = null;
    AudioSource as04 = null;

    public float BulletSpeed = 30f;
    public float fireRate = 0.1f;
    public GameObject Bullet;
    public GameObject FirePointR;
    public GameObject FirePointL;
    public GameObject eye;
    public float colorTime = 0.5f;
    public TurretState TS = TurretState.None;
    public float hp = 1f;
    public bool shotAt = false;
    public GameObject Head;
    public float searchingSpeed = 0.5f;
    public bool closeSomthing = false;
    public bool findTarget = false;
    public bool isDetectTarget = false;
    public Camera sight;
    public float maxDistance = 100f;
    public float releaseROTtime = 8f;
    public float defenceROTtime = 6f;
    public float lostTargetTime = 5f;
    List<float> timer = new List<float>();
    Vector3 firstRot = Vector3.zero;

    DetectRange dr;

    GameObject nearSomthing=null;        //DetectRange 안에 EnterThing이랑 같은걸 담아야함

    RaycastHit rayHit;

    bool ctrlCorutain = true;

    List<bool> playOnce = new List<bool>();  //코루틴 재사용을 막기위해 사용

    Light LightEye;

    Rigidbody rb;

    int layerMask = 1 << 8 | 1 << 10;
    float nextWork = 0f;

    private void Awake()
    {

        
        dr = this.gameObject.GetComponentInChildren<DetectRange>();

        timer.Add(0f);  //setTarget() 안에 플레이어 놓치고 몇초 지났는지 계산할때 사용중
        timer.Add(0f);  //setState() 안에 트리거에서 플레이어 나가고 몇초 지났는지 계산할때 사용중
        timer.Add(0f);  //FiredTarget() 안에 트리거에서 플레이어 나가고 몇초 지났는지 계산할때 사용중

        playOnce.Add(false);     //센드리모드에서 빠져나올때 재생될 사운드 위해 만듦           / 0
        playOnce.Add(false);     //총쏘고나서 멈출때 재생될 사운드 위해 만듦                  / 1
        playOnce.Add(false);     //플레이어의 총에 맞은 다음 재생될 사운드 위해 만듦           / 2
        playOnce.Add(false);     //죽을 때, 사운드 재생이 끝났는지 확인하기 위해서 만듦        / 3
        playOnce.Add(false);     //맞았을 때, 플레이어를 바라보기 위해서 사용                 / 4
        playOnce.Add(false);     //굿바이가 사격중에 안나오게 하려고 만듦                     / 5

        LightEye = eye.GetComponent<Light>();

        audioSource = this.gameObject.GetComponent<AudioSource>();
        TSS = this.gameObject.GetComponentInChildren<Turret_Sound_system>();
        as03 = GameObject.Find("Turret_Under").GetComponent<AudioSource>();
        as04 = this.gameObject.GetComponentInChildren<DetectRange>().GetComponent<AudioSource>();
    }
    
    private void Start()
    {
        
        PlaySound("turret_When_Start");
        TS = TurretState.Working;
        firstRot = Head.transform.localEulerAngles;
        GameManager.countTurret++;
    }

    private void Update()
    {
        if(GameManager.Singleton.playerIsDead)
        {

        }

        if (GameManager.Singleton.GS == GameState.Working)
        {
            if (playOnce[3] == true)
            {
                this.gameObject.SetActive(false);
            }

            setState();

            runPhase();

            if (shotAt)
            {
                shotAt = ShootAt();
                Died();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nearSomthing = other.gameObject;
            closeSomthing = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            closeSomthing = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (isDetectTarget)
        {
            Gizmos.DrawRay(Head.transform.position, Head.transform.forward * rayHit.distance);
        }
        else
        {
            Gizmos.DrawRay(Head.transform.position, Head.transform.forward * maxDistance);
        }

        
    }

    IEnumerator turret_When_Start(AudioClip ac)
    {
        audioSource.clip = ac;

        if (!audioSource.isPlaying)
        {
            audioSource.volume = 0.65f;
            audioSource.Play();
            yield return new WaitForSeconds(ac.length);
        }
    }

    IEnumerator turret_fire_1x(AudioClip ac)
    {
        TSS.subAudiSou.clip = ac;
        TSS.subAudiSou.volume = 0.15f;
        TSS.subAudiSou.Play();
        yield return new WaitForSeconds(ac.length);
        playOnce[1] = true;
    }

    IEnumerator turret_CentreMode_Activated(AudioClip ac)
    {

        if (!audioSource.isPlaying)
        {
            if (ctrlCorutain)
            {
                audioSource.clip = ac;
                ctrlCorutain = false;
                playOnce[0] = true;
                audioSource.volume = 0.65f;

                audioSource.Play();
                yield return new WaitForSeconds(ac.length);
            }
        }
    }
    
    IEnumerator turret_retire_1(AudioClip ac)
    {

        if (!audioSource.isPlaying)
        {
            if (playOnce[0])
            {
                audioSource.clip = ac;
                playOnce[0] = false;
                audioSource.volume = 0.65f;
                audioSource.Play();
                yield return new WaitForSeconds(ac.length);
                ctrlCorutain = true;
            }
        }
    }

    IEnumerator turret_search_1(AudioClip ac)
    {

        audioSource.clip = ac;

        if (!audioSource.isPlaying)
        {
            if (playOnce[1])
            {
                playOnce[1] = false;
                audioSource.volume = 0.65f;
                audioSource.Play();
                yield return new WaitForSeconds(ac.length);
            }
        }
    }

    IEnumerator sentry_damage1(AudioClip ac)
    {

        as03.clip = ac;

        if (!as03.isPlaying)
        {
            //if (playOnce[2])
            //{
            //    playOnce[2] = false;
                as03.volume = 0.75f;
                as03.Play();
                yield return new WaitForSeconds(ac.length);
            //}
        }
    }

    IEnumerator turret_shotat_1(AudioClip ac)
    {

        as04.clip = ac;

        if (!as04.isPlaying)
        {
            //if (playOnce[2])
            //{
            //    playOnce[2] = false;
                as04.volume = 0.75f;
                as04.Play();
                yield return new WaitForSeconds(ac.length);
            //}
        }
    }

    IEnumerator turret_disabled_1(AudioClip ac)
    {
        as03.clip = ac;

        foreach (AudioClip clip in audioClip)
        {
            if (clip.name == "turret_disabled_2")
            {
                as04.clip = clip;
            }
        }

        as03.volume = 1f;
        as03.Play();
        yield return new WaitForSeconds(ac.length);
        as04.volume = 0.75f;
        as04.Play();
        yield return new WaitForSeconds(as04.clip.length);
        playOnce[3] = true;
    }

    void PlaySound(string efName)           //파일 이름으로 찾아야함, 랜덤 불가능
    {
        if (!playOnce[3])
        {

            bool isFine = false;

            foreach (AudioClip ac in audioClip)
            {
                if (efName == ac.name)
                {
                    isFine = true;
                    StartCoroutine(efName, ac);
                }
            }

            if (!isFine)
            {
                print("\"" + efName + "\"이란 오디오 클립을 못찾았습니다. -> \"코루틴 함수명\"과 \"오디오 클립의 파일명\"을 살펴주세요. 둘이 같아야 됩니다.");
            }
        }
    }

    void setState()
    {
        if (closeSomthing || dr.PlayerGetInSide || playOnce[4])     //  플레이어가 트리거 안으로 들어오면 무조건 디펜스 모드임 수색함
        {
            if (nearSomthing == null)
            {
                nearSomthing = dr.EnterThing;
            }

            TS = TurretState.DefenceMode;

            if (ctrlCorutain)
                PlaySound("turret_CentreMode_Activated");
        }
        else
        {
            timer[1] += Time.deltaTime;
            if (timer[1] > 10f)
            {
                timer[1] = 0f;
            }
            if (!closeSomthing && !dr.PlayerGetInSide)
            {
                TS = TurretState.Working;
                if (!isDetectTarget)
                    PlaySound("turret_retire_1");
                
            }
        }


        if(isDetectTarget)
        {
            Head.transform.DOKill();
            TS = TurretState.FindedTarget;   
        }
    }


    void runPhase()
    {
        if (TS == TurretState.DefenceMode)
        {
            if (!isDetectTarget && Physics.Raycast(Head.transform.position, Head.transform.forward, out rayHit, maxDistance, layerMask) && rayHit.transform.gameObject.tag == "Player")
            {
                isDetectTarget = true;
            }

            timer[0] += Time.deltaTime;
            if (timer[0] > lostTargetTime)
            {
                timer[0] = 0f;
                playOnce[4] = false;
            }
        }



        switch (TS)
        {
            case TurretState.Working:   //아직 아무것도 발견 못함 = 전방&근처에 아무것도 없음
                ReleaseHead();
                break;
            case TurretState.DefenceMode:   //  무언가 발견함 = 플레이어 좌표를 받아 서서히 돌아봄
                workPhaseDefenceMode();
                break;
            case TurretState.FindedTarget:  // 무언가를 찾고 lookat으로 고정함 & 타겟이 trigger 범위를 벗어나지 않으면 lookat이 풀리지 않음
                setTarget();
                FiredTarget();
                break;
            case TurretState.Died:
                print(this.gameObject.name + "is Died!!");
                break;
        }
    }

    

    void setTarget()
    {
        LightEye.DOColor(Color.red, colorTime);
        Head.transform.LookAt(nearSomthing.transform);

        if (isDetectTarget && Physics.Raycast(Head.transform.position, Head.transform.forward, out rayHit, maxDistance, layerMask) && rayHit.transform.gameObject.tag != "Player")
        {
            timer[0] += Time.deltaTime;
            if (timer[0] > lostTargetTime)
            {
                timer[0] = 0f;
                isDetectTarget = false;
                PlaySound("turret_search_1");
            }
        }
    }

    void FiredTarget()
    {
        timer[2] += Time.deltaTime;
        if (timer[2] > fireRate)
        {
            timer[2] = 0f;
            GameObject temp = Instantiate(Bullet, FirePointR.transform.position, FirePointR.transform.rotation);   // 나중에 오브젝트 풀로 바꿔주자
            temp.transform.LookAt(nearSomthing.transform);

            rb = temp.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(rb.transform.forward * BulletSpeed, ForceMode.VelocityChange);

            PlaySound("turret_fire_1x");

            temp = Instantiate(Bullet, FirePointL.transform.position, FirePointL.transform.rotation);   // 나중에 오브젝트 풀로 바꿔주자
            temp.transform.LookAt(nearSomthing.transform);

            rb = temp.gameObject.GetComponent<Rigidbody>();
            rb.AddForce(rb.transform.forward * BulletSpeed, ForceMode.VelocityChange);
            
        }
    }


    void workPhaseDefenceMode()
    {
        if (TS == TurretState.DefenceMode|| playOnce[4])
        {
            LightEye.DOColor(Color.yellow, colorTime);
            Head.transform.DOLookAt(nearSomthing.transform.position, defenceROTtime);
        }
    }

    void ReleaseHead()
    {
        if (TS == TurretState.Working)      //워킹상태이면 처음에 바라보던 곳으로 되돌아옴
        {
            LightEye.DOColor(Color.green, colorTime);
            Head.transform.DOLocalRotate(firstRot, releaseROTtime);
        }
    }

    bool ShootAt()
    {
        nearSomthing = GameObject.Find("Player");
        playOnce[4] = true;
        hp -= Gun_Mecanim.Singleton.DemageSize;
        PlaySound("turret_shotat_1");
        PlaySound("sentry_damage1");
        return false;
    }

    void Died()
    {
        if(hp<=0f)
        {
            TS = TurretState.Died;
            GameManager.countTurret--;
            print(this.gameObject.name + "is Died");
            PlaySound("turret_disabled_1");
        }
    }


}
