using UnityEngine;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] private Text ruleText;
    [SerializeField] private Text startText;
    

    void Start()
    {
        gameObject.SetActive(false);
    }

    private void RunInfo()
    {
        ruleText.text = "Rule ";//outro event converter.RulesetBinarytoDecimal(); 
        startText.text = ""; //listen to event
    }
    

}
