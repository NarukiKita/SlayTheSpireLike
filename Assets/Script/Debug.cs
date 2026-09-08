using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics;

public class InputDebug : MonoBehaviour
{
    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Mouse)
        {
            UnityEngine.Debug.Log(
                $"★★★ MOUSE CHANGE ★★★\n" +
                $"Change: {change}\n" +
                $"Enabled: {device.enabled}\n" +
                $"Device: {device}"
            );

            if (change == InputDeviceChange.Disabled ||
                change == InputDeviceChange.HardReset)
            {
                UnityEngine.Debug.Log(
                    "★★★ MOUSE STACK TRACE ★★★\n" +
                    StackTraceUtility.ExtractStackTrace()
                );
            }
        }
    }

    void Start()
    {
        UnityEngine.Debug.Log("===== INPUT DEBUG START =====");

        UnityEngine.Debug.Log(
            $"Mouse: {Mouse.current} / enabled={Mouse.current?.enabled}"
        );

        UnityEngine.Debug.Log(
            $"Keyboard: {Keyboard.current} / enabled={Keyboard.current?.enabled}"
        );
    }
}