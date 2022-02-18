using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapBtnManger : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera CameraOne;
    bool onOffToggle = false;
    public void MapBtn()
    {
        SwitchMapSize(onOffToggle);
    }


    private void SwitchMapSize(bool toggle)
    {
        if(toggle)
        {
            CameraOne.Priority = 20;
            onOffToggle = false;
        }
        else
        {
            CameraOne.Priority = 0;
            onOffToggle = true;
        }
    }
}
