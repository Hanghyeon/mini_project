using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    SetOthers = -1,
    CountDown,
    Working,
    End
}

public class GameManager : MonoBehaviour {

    public static GameManager Singleton;

    public AudioClip audiClip;
    AudioSource AS;

    public GameState GS = GameState.SetOthers;
    public bool playerIsDead = false;

    static public int countTurret = 0;
    static public int totalTurret = 0;
    static public float spendTime = 0f;
    public float countdown = 3f;

    float temp=0f;

    void Awake () {
        Singleton = this;
        Player_Data.whenGetDamaged += checkPlayerStillAlive;
        AS = this.gameObject.GetComponent<AudioSource>();
        AS.clip = audiClip;
        AS.loop = true;
        AS.volume = 1f;
        AS.Play();
    }

    private void Start()
    {
        totalTurret = countTurret;
    }

    private void Update()
    {
        if ((GS == GameState.Working && countTurret <= 0)||playerIsDead)
        {
            GS = GameState.End;
            GotoEnd();
        }

        if (temp > countdown+1f)
        {
            //Start Match!
            GS = GameState.Working;
            if (!playerIsDead)
                spendTime += Time.deltaTime;
        }
        else
        {
            //ready! set! 
            GS = GameState.CountDown;
            temp += Time.deltaTime;
            totalTurret = countTurret;
        }
    }


    public void checkPlayerStillAlive()     //Player_Data -> in "getDamage(int damageSize)" methord
    {
        if (Player_Data.Singleton.hp <= 0f)
        {
            playerIsDead = true;
            print("Player Died!!!");
        }
        else
        {
            print("Player get Damaged!!!");
        }
    }

    public void GotoEnd()
    {
        SceneManager.LoadScene("EndScene");
    }
}
