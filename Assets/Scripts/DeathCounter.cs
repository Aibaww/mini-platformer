using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    public static DeathCounter deathCounter;
     public int deaths = 0;
    private TMP_Text text;

    void Start() {
        deathCounter = this;
        text = GetComponent<TMP_Text>();
    }

    public void AddDeath() {
        deaths++;
        text.text = "Deaths: " + deaths;
    }
}
