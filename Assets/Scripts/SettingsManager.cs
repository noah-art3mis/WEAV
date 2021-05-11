using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Toggle randomRulesetToggle;
    [SerializeField] private Dropdown startDropdown;
    [SerializeField] private Dropdown modeDropdown;
    [SerializeField] private Button runButton;
    [SerializeField] private InputField inputField;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private Text errorPanelText;
    private CA ca;
    private BinaryConverter converter;

    [Header("Settings")]
    public bool randomRuleset;
    public bool randomStart;
    public bool scrolling;



    private void Start()
    {
        ca = GetComponent<CA>();
        converter = GetComponent<BinaryConverter>();

        randomRuleset = false;
        randomStart = false;
        scrolling = false;
    }

    public int[] ComputeSettings(string parameter)
    {
        if (startDropdown.value == 0) randomStart = false;
        if (startDropdown.value == 1) randomStart = true;
        if (modeDropdown.value == 0) scrolling = false;
        if (modeDropdown.value == 1) scrolling = true;
        if (randomRulesetToggle.isOn == true) randomRuleset = true;
        if (randomRulesetToggle.isOn == false) randomRuleset = false;
        return GetRuleset(parameter);
    }

    private int[] GetRuleset(string parameter)
    {
        int[] ruleset = new int[8];

        if (randomRuleset)
        {
            for (int i = 0; i < 8; i++)
            {
                ruleset[i] = UnityEngine.Random.Range(0, 2);
            }
        }
        else
        {
            if (inputField.text == "")
            {
                ShowError("Must input a number between 0 and 255");
                return ruleset;
            }

            int rulesetDecimal = int.Parse(inputField.text);

            if (rulesetDecimal > 255 || rulesetDecimal < 0)
            {
                ShowError("Must input a number between 0 and 255");
                return ruleset;
            }

            if (parameter == "up")
            {
                rulesetDecimal++;
                inputField.text = rulesetDecimal.ToString();
            }
            if (parameter == "down")
            {
                rulesetDecimal--;
                inputField.text = rulesetDecimal.ToString();
            }

            ruleset = converter.RulesetDecimaltoBinary(rulesetDecimal);
        }
        return ruleset;
    }

    public string SetFirstGeneration()
    {
        int[] cells = new int[ca.arraySize];

        if (randomStart)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = UnityEngine.Random.Range(0, 2);
            }
            return "Random Start";
        }
        else
        {
            cells[cells.Length / 2] = 1;
            return "Single Cell Start";
        }
    }

    private void ShowError(string text)
    {
        ca.Reset();
        errorPanelText.text = text;
        errorPanel.SetActive(true);
    }
}
