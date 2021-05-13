
using UnityEngine.UI;
using UnityEngine;

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

    [Header("Settings")]
    public bool randomRuleset;
    public bool randomStart;
    public bool scrolling;

    private void Start()
    {
        ca = GetComponent<CA>();

        randomRuleset = false;
        randomStart = false;
        scrolling = false;
    }

    public void ComputeSettings()
    {
        if (startDropdown.value == 0) randomStart = false;
        if (startDropdown.value == 1) randomStart = true;
        if (modeDropdown.value == 0) scrolling = false;
        if (modeDropdown.value == 1) scrolling = true;
        if (randomRulesetToggle.isOn == true) randomRuleset = true;
        if (randomRulesetToggle.isOn == false) randomRuleset = false;
    }

    public int[] GetRuleset(string parameter)
    {
        int[] ruleset = new int[CA.ruleset.Length];

        if (randomRuleset)
        {
            for (int i = 0; i < ruleset.Length; i++)
            {
                ruleset[i] = Random.Range(0, 2);
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

            ruleset = BinaryConverter.RulesetDecimaltoBinary(rulesetDecimal);
        }
        return ruleset;
    }

    public int[] SetFirstGeneration()
    {
        int[] firstGen = new int[CA.arraySize];

        if (randomStart)
        {
            for (int i = 0; i < firstGen.Length; i++)
            {
                firstGen[i] = Random.Range(0, 2);
            }
            CA.startInfo = "Random Start";
            return firstGen;
        }
        else
        {
            firstGen[firstGen.Length / 2] = 1;
            CA.startInfo = "Single Cell Start";
            return firstGen;
        }
    }

    private void ShowError(string text)
    {
        ca.Reset();
        errorPanelText.text = text;
        errorPanel.SetActive(true);
    }
}
