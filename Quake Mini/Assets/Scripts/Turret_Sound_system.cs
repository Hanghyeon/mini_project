using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret_Sound_system : MonoBehaviour {
    
    public AudioSource subAudiSou;

    private void Awake()
    {
        subAudiSou = this.gameObject.GetComponent<AudioSource>();
    }

}
