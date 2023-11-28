using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Loading;
using UnityEngine;
using UnityEngine.UI;

public class TestReLoad : MonoBehaviour
{
    public Button reload;
    bool pressed = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha4)&&!pressed)
        {
            AsyncLoadManager.Instance.LoadScene("Stage1 1");
            pressed = true;
        }
    }

    
}
