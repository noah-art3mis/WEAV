using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class SettingsManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Toggle randomRulesetToggle;
    [SerializeField] private Dropdown startDropdown;
    [SerializeField] private Dropdown modeDropdown;
    [SerializeField] private Button runButton;
    private CA ca;

    private void Start()
    {
        ca = GetComponent<CA>();
    }
    public void CheckUI()
    {
        if (startDropdown.value == 0) ca.randomStart = false;
        if (startDropdown.value == 1) ca.randomStart = true;
        if (modeDropdown.value == 0) ca.scrolling = false;
        if (modeDropdown.value == 1) ca.scrolling = true;
        if (randomRulesetToggle.isOn == true) ca.randomRuleset = true;
        if (randomRulesetToggle.isOn == false) ca.randomRuleset = false;
    }
}
