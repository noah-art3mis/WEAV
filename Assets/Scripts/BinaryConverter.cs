﻿using UnityEngine;

public class BinaryConverter : MonoBehaviour
{
    public static int RulesetBinarytoDecimal(int[] ruleset)
    {
        int rulesetDecimal = 0;
        for (int i = 8, j = 1; i > 0; i--)
        {
            rulesetDecimal = rulesetDecimal + (ruleset[i - 1] * j);
            j *= 2;
        }
        return rulesetDecimal;
    }

    public static int[] RulesetDecimaltoBinary(int decimalNumber)
    {
        int[] ruleset = new int[Defaults.RULESET_SIZE];
        int remainder;

        if (decimalNumber > 255)
        {
            Debug.Log("There are only 256 rules");
            return ruleset;
        }
        for (int i = 8; i > 0; i--)
        {        
            remainder = decimalNumber % 2;
            decimalNumber /= 2;
            ruleset[i - 1] = remainder;
        }
        return ruleset;
    }
}
