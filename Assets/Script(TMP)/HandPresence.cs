using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class HandPresence : MonoBehaviour
{
    private InputDevice targetDevice;
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();//입력장치 리스트 선언
        //InputDevices.GetDevices(devices); // 모든 컨트롤러 리스트에 넣기
        InputDeviceCharacteristics rightControllercharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllercharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics); // Log로 아이템 이름, 상세정보 표시
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue); //primaryButton(bool형식으로된)을 누르는 동안 로그 뜸
        if (primaryButtonValue)
        {
            Debug.Log("Pressing Priamry Button");
        }
    }
}
