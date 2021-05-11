using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class BinaryConverter : MonoBehaviour
{

    public int RulesetBinarytoDecimal(int[] ruleset)
    {
        int rulesetDecimal = 0;
        for (int i = 0, j = 128; i < 8; i++)
        {
            rulesetDecimal = rulesetDecimal + (ruleset[i] * j);
            j /= 2;
        }
        return rulesetDecimal;
    }

    public int[] RulesetDecimaltoBinary(int decimalNumber)
    {
        int[] ruleset = new int[CA.ruleset.Length];
        int remainder;
        int i = 0;

        if (decimalNumber > 255)
        {
            Debug.Log("There are only 256 rules");
            return ruleset;
        }
        while (decimalNumber > 0)
        {        
            remainder = decimalNumber % 2;
            decimalNumber /= 2;
            ruleset[i] = remainder;
            i++;
        }
        System.Array.Reverse(ruleset);
        return ruleset;
    }


}
