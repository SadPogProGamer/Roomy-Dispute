using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private static int _nextPlayerIndex = 0;
    private static int[] _playerIndexes = new int[4];

    private void OnEnable()
    {
        // Use onDeviceChange instead of onDeviceAdd
        InputSystem.onDeviceChange += OnDeviceChanged;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChanged;
    }

    private void OnDeviceChanged(InputDevice device, InputDeviceChange change)
    {
        if (change == InputDeviceChange.Added && device is Gamepad)
        {
            int playerIndex = AssignPlayerIndex(device);
            Debug.Log($"Player {playerIndex} assigned to {device}");
        }
    }

    private int AssignPlayerIndex(InputDevice device)
    {
        for (int i = 0; i < _playerIndexes.Length; i++)
        {
            if (_playerIndexes[i] == 0)
            {
                _playerIndexes[i] = device.deviceId;
                return i;
            }
        }

        Debug.LogError("No available player slots!");
        return -1;
    }

    public static int GetPlayerIndexForInput(InputDevice inputDevice)
    {
        for (int i = 0; i < _playerIndexes.Length; i++)
        {
            if (_playerIndexes[i] == inputDevice.deviceId)
            {
                return i;
            }
        }
        return -1;
    }
}
