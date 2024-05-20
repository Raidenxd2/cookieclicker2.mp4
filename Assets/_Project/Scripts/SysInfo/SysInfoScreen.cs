using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Cookie2.Runtime.SysInfo
{
    public class SysInfoScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text SysInfoText;
        [SerializeField] private GameObject SysInfoScreenGO;

        [SerializeField] private PlayerInput playerInput;

        private void Start()
        {
            SysInfoText.text = "Game version: " + Application.version + "\n"
            + "Unity version: " + Application.unityVersion + "\n"
            + "Platform: " + Application.platform + "\n"
            + "dataPath: " + Application.dataPath + "\n"
            + "temporaryCachePath: " + Application.temporaryCachePath + "\n"
            + "persistentDataPath: " + Application.persistentDataPath + "\n"
            + "Graphics API: " + SystemInfo.graphicsDeviceType + "\n"
            + "Graphics Device Name: " + SystemInfo.graphicsDeviceName + "\n"
            + "Graphics Memory Size: " + SystemInfo.graphicsMemorySize + "\n"
            + "Device name: " + SystemInfo.deviceName + "\n"
            + "Device model: " + SystemInfo.deviceModel + "\n"
            + "Device type: " + SystemInfo.deviceType + "\n"
            + "Operating System: " + SystemInfo.operatingSystem + "\n"
            + "Operating System Family: " + SystemInfo.operatingSystemFamily + "\n"
            + "CPU Cores: " + SystemInfo.processorCount + "\n"
            + "RAM: " + SystemInfo.systemMemorySize;
        }

        private void Update()
        {
            if (playerInput.actions["SysInfo"].WasPressedThisFrame())
            {
                SysInfoScreenGO.SetActive(true);
            }
        }
    }
}