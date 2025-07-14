#if UNITY_ANDROID && !CC2_REMOVE_VR_SUPPORT
using System.Collections.Generic;
using UnityEngine;

public class OculusQuestRefreshRateDropdownData : MonoBehaviour
{
    public List<float> refreshRates = new();

    public void ChangeRefreshRate(int val)
    {
        Game.instance.ChangeRefreshRate(refreshRates[val]);
    }
}
#endif