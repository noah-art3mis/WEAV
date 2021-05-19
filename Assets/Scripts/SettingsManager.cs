
using UnityEngine.UI;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InputField inputFieldRuleset;
    [SerializeField] private Toggle randomRulesetToggle;
    [SerializeField] private Dropdown startDropdown;
    [SerializeField] private Dropdown modeDropdown;
    [SerializeField] private InputField scrollSpeed;
    [SerializeField] private InputField inputFieldSize;
    [SerializeField] private Button runButton;
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

    public float GetScrollSpeed()
    {
        if (!scrolling)
            return 0;

        if (scrollSpeed.text == "")
            scrollSpeed.text = "20";
            
        float speed = 1 / float.Parse(scrollSpeed.text);
        return speed;
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
            if (inputFieldRuleset.text == "")
            {
                ShowError("Must input a number between 0 and 255");
                return ruleset;
            }

            int rulesetDecimal = int.Parse(inputFieldRuleset.text);

            if (rulesetDecimal > 255 || rulesetDecimal < 0)
            {
                ShowError("Must input a number between 0 and 255");
                return ruleset;
            }

            if (parameter == "up")
            {
                rulesetDecimal++;
                inputFieldRuleset.text = rulesetDecimal.ToString();
            }
            if (parameter == "down")
            {
                rulesetDecimal--;
                inputFieldRuleset.text = rulesetDecimal.ToString();
            }

            ruleset = BinaryConverter.RulesetDecimaltoBinary(rulesetDecimal);
        }
        return ruleset;
    }

    public int GetSize()
    {
        if (inputFieldSize.text == "")
            inputFieldSize.text = "100";

        int size = int.Parse(inputFieldSize.text);

        if (size > 500)
            size = 500;

        return size;
    }

    public int[] SetFirstGeneration(int arraySize)
    {
        int[] firstGen = new int[arraySize];

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
        ca.ResetGrid();
        errorPanelText.text = text;
        errorPanel.SetActive(true);
    }
}
