
using UnityEngine.UI;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Dropdown rulesetDropdown;
    [SerializeField] private InputField rulesetInputField;
    [SerializeField] private Dropdown modeDropdown;
    [SerializeField] private InputField scrollSpeed;
    [SerializeField] private Dropdown sizeDropdown;
    [SerializeField] private InputField sizeInputField;
    [SerializeField] private InputField resolutionField;
    [SerializeField] private Dropdown startDropdown;
    [SerializeField] private Button runButton;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private Text errorPanelText;
    
    private CA ca;

    [Header("Settings")]
    public bool isRandomRuleset;
    public bool isScrolling;
    public bool isFullscreen;
    public bool isRandomStart;
    public int resolution;

    private void Start()
    {
        ca = GetComponent<CA>();
        resolution = Defaults.RESOLUTION;
    }

    public void ComputeSettings()
    {
        if (rulesetDropdown.value == 0) isRandomRuleset = true;
        if (rulesetDropdown.value == 1) isRandomRuleset = false;
        if (modeDropdown.value == 0) isScrolling = true;
        if (modeDropdown.value == 1) isScrolling = false;
        if (sizeDropdown.value == 0) isFullscreen = true;
        if (sizeDropdown.value == 1) isFullscreen = false;
        if (startDropdown.value == 0) isRandomStart = false;
        if (startDropdown.value == 1) isRandomStart = true;
    }

    public Vector2 GetSize()
    {
        if (isFullscreen) //fit horizontal
        {

            if (resolutionField.text == "")
                resolutionField.text = Defaults.RESOLUTION.ToString();
            else
                resolution = int.Parse(resolutionField.text);
            
            float factor = Defaults.RESOLUTION_FACTOR;
            float h = Screen.height * factor * resolution;
            float w = Screen.width * factor * resolution;
            return new Vector2(w, h);
        }
        else //fit vertical
        {
            if (sizeInputField.text == "")
                sizeInputField.text = Defaults.GRID_SIZE.ToString();

            int size = int.Parse(sizeInputField.text);

            if (size > 400)
            {
                ShowError("Size too big. Try less than 400.");
                return new Vector2(50, 50);
            }

            return new Vector2(size, size);
        }
    }

    public float GetScrollSpeed()
    {
        if (!isScrolling)
            return 0;

        if (scrollSpeed.text == "") //see string.IsNullOrWhiteSpace
            scrollSpeed.text = Defaults.SCROLL_SPEED.ToString();

        float input = float.Parse(scrollSpeed.text) / 10;
        float speed = Defaults.SPEED_MODIFIER / input;
        return speed;
    }

    public int[] GetRuleset(string parameter)
    {
        int[] ruleset = new int[Defaults.RULESET_SIZE];

        if (isRandomRuleset)
        {
            for (int i = 0; i < ruleset.Length; i++)
            {
                ruleset[i] = Random.Range(0, 2);
            }
            return ruleset;
        }
        else
        {
            if (rulesetInputField.text == "")
            {
                ShowError("Must input a number between 0 and 255");
                return ruleset;
            }

            int rulesetDecimal = int.Parse(rulesetInputField.text);

            if (rulesetDecimal > 255 || rulesetDecimal < 0)
            {
                ShowError("Must input a number between 0 and 255");
                return ruleset;
            }

            if (parameter == "up")
            {
                rulesetDecimal++;
                rulesetInputField.text = rulesetDecimal.ToString();
            }
            if (parameter == "down")
            {
                rulesetDecimal--;
                rulesetInputField.text = rulesetDecimal.ToString();
            }

            ruleset = BinaryConverter.RulesetDecimaltoBinary(rulesetDecimal);
            return ruleset;
        }
    }

    public int[] SetFirstGeneration(int arraySize)
    {
        int[] firstGen = new int[arraySize];

        if (isRandomStart)
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
        ca.errorFlag = true;
        ca.ResetGrid();
        errorPanelText.text = text;
        errorPanel.SetActive(true);
    }


}
