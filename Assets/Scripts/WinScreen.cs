using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public Image youWin;

    void Start() {
        SetImageActive(false);
    }

    public void Display() {
        SetImageActive(true);
    }

    void SetImageActive(bool isActive) {
        youWin.gameObject.SetActive(isActive);
    }
}
