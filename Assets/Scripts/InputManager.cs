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
    [SerializeField] private Button runButton;

    CA ca;

    List<string> startOptions = new List<string> { "Normal", "Random" };
    List<string> modeOptions = new List<string> { "Static", "Scrolling" };

    private void Start()
    {
        ca = GetComponent<CA>();
        startDropdown.AddOptions(startOptions);
        modeDropdown.AddOptions(modeOptions);
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

    //public void ValidateInput()
    //{
    //    if (!ca.randomRuleset)
    //    {
    //        if (ca.ruleInput.text == "")
    //        {
    //            Debug.Log("oof");
    //            return;
    //        }
    //    }
    //}
}
