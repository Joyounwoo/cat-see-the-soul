using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    public Button button;
    public void Awake()
    {
        button.onClick.AddListener(() =>
        AudioManager.PlayUI());
    }
}
