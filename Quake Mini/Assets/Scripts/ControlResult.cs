using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlResult : MonoBehaviour {

    public Text allTurrets;
    public Text restTurrets;
    public Text Time;

    int _allturret;
    int _restturret;
    float _time;

    private void Awake()
    {
        _time = GameManager.spendTime;
        _restturret = GameManager.countTurret;
        _allturret = GameManager.totalTurret;
    }

    // Update is called once per frame
    void Update () {
        allTurrets.text = _allturret.ToString();
        restTurrets.text = _restturret.ToString();
        Time.text = string.Format("{0:0.00}", _time);
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
