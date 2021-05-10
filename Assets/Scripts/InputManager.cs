using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class InputManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Toggle randomRulesetToggle;
    [SerializeField] private Dropdown startDropdown;
    [SerializeField] private Dropdown modeDropdown;

    List<string> startOptions = new List<string> { "Normal", "Random" };
    List<string> modeOptions = new List<string> { "Static", "Scrolling" };

    private void Start()
    {
        startDropdown.AddOptions(startOptions);
        modeDropdown.AddOptions(modeOptions);
    }
    public void CheckUI()
    {
        if (startDropdown.value == 0) CA.randomStart = false;
        if (startDropdown.value == 1) CA.randomStart = true;
        if (modeDropdown.value == 0) CA.scrolling = false;
        if (modeDropdown.value == 1) CA.scrolling = true;
        if (randomRulesetToggle.isOn == true) CA.randomRuleset = true;
        if (randomRulesetToggle.isOn == false) CA.randomRuleset = false;
    }
}
