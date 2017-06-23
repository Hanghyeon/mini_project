using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour {

    Player_Data pData;

    public List<Text> texts;

    Text restAmmoText;
    Text loadedAmmoText;
    Text HPText;
    Text ArmorText;
    Text Timer;
    Text CountDown;

    private void Awake()
    {
        pData = GameObject.Find("GameManager").GetComponent<Player_Data>();
    }

    private void Start()
    {
        foreach(Text tx in texts)
        {
            if(tx.gameObject.name== "RestAmmo")
            {
                restAmmoText = tx;
            }

            if (tx.gameObject.name == "LoadedAmmo")
            {
                loadedAmmoText = tx;
            }

            if (tx.gameObject.name == "HP")
            {
                HPText = tx;
            }

            if (tx.gameObject.name == "Armor")
            {
                ArmorText = tx;
            }

            if (tx.gameObject.name == "Timer")
            {
                Timer = tx;
            }

            if (tx.gameObject.name == "CountDown")
            {
                CountDown = tx;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        restAmmoText.text = pData.restAmmo.ToString();
        loadedAmmoText.text = pData.LoadedAmmo.ToString();
        HPText.text = string.Format("{0:g3}", pData.hp);
        ArmorText.text = string.Format("{0:g3}", pData.Armor * 100);
        if(GameManager.Singleton.GS==GameState.CountDown)
        {
            CountDown.enabled = true;
            CountDown.text = string.Format("{0:0}",GameManager.Singleton.countdown -= Time.deltaTime);
        }
        else
        {
            CountDown.enabled = false;
        }

        Timer.text = string.Format("{0:0.00}", GameManager.spendTime);

    }
}
