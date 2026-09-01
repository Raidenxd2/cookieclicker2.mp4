using System.Collections.Generic;
using UnityEngine;
using TMPro;
 
public class LocalizeDropdown : MonoBehaviour
{
    [SerializeField] private string[] dropdownOptions;
    private TMP_Dropdown tmpDropdown;
 
    private void Awake()
    {
        List<TMP_Dropdown.OptionData> tmpDropdownOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < dropdownOptions.Length; i++)
        {
            tmpDropdownOptions.Add(new TMP_Dropdown.OptionData(BeanLocalization.GetString(dropdownOptions[i])));
        }
        if (!tmpDropdown) tmpDropdown = GetComponent<TMP_Dropdown>();
        tmpDropdown.options = tmpDropdownOptions;
    }
}