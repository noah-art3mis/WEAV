using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private Text ruleText;
    [SerializeField] private Text startText;
    [SerializeField] private BinaryConverter converter;
    
    private void OnEnable() => CA.settingsDone += RunInfo;
    private void OnDisable() => CA.settingsDone -= RunInfo;
    private void RunInfo(int[] ruleset, string startInfo)
    {
        ruleText.text = "Rule " + BinaryConverter.RulesetBinarytoDecimal(ruleset);
        startText.text = startInfo;
    }
}
