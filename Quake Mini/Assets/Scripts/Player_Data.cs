using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Data : MonoBehaviour {

    public static Player_Data Singleton;

    public float hp = 100f;                // 잃을 체력 = 받은 데미지 * (1f-armor)
    public float Armor = 1f;           // 피해 감쇄율       armor=armor`-(1f-armor`) ... 0.6f | 0f 이하로 안내려감
    public float lessArmor = 0.1f;
    public int MagSize = 30;            // 탄창 크기
    public int LoadedAmmo;              // 장전된 탄약
    public int restAmmo;  // 휴행탄약
    float lessHP;
    public static System.Action whenGetDamaged;


    public static object temp;

    
    private void Awake()
    {
        LoadedAmmo = MagSize;
        restAmmo = MagSize * 3;
        Singleton = this;
    }


    public void getDamage(int damageSize)
    {
        lessHP = damageSize * (1f - Armor);
        if (Armor <= lessArmor)
        {
            Armor = 0f;
        }
        else
        {
            Armor -= lessArmor;
        }
        hp -= lessHP;                            

        if (hp <= 0f)
        {
            hp = 0f;
        }

        

        whenGetDamaged();
    }
}
