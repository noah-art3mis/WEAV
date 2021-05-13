using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCells : MonoBehaviour
{
    int[] currentGen = new int[Grid.maxX];
    int[] nextGen = new int[Grid.maxX];

    private int left, me, right;
    private int lineSize = Grid.maxX;
    private int maxGenerations = Grid.maxY;

    public void UpdateGrid(int[] ruleset, int[] cells)
    {
        for (int generation = 0; generation < maxGenerations; generation++)
        {
            UpdateLine(generation);
            ComputeNextLine(ruleset, cells, generation);
        }
    }

    private void UpdateLine(int j)
    {
        for (int i = 1; i < lineSize - 1; i++)
        {
            if (currentGen[i] == 1)
            {
                Grid.spriteArray[j][i].SetActive(true);
            }
            else
            {
                Grid.spriteArray[j][i].SetActive(false);
            }
        }
    }

    private void ComputeNextLine(int[] ruleset, int[] cells, int generation)
    {
        for (int i = 1; i < lineSize - 1; i++) // ignores borders
        {
            left = cells[i - 1];
            me = cells[i];
            right = cells[i + 1];

            nextGen[i] = ApplyRuleset(ruleset, left, me, right);

            if (nextGen[i] == 1)
                Grid.spriteArray[generation][i].SetActive(true);
            else
                Grid.spriteArray[generation][i].SetActive(false);
        }
    }

    private int ApplyRuleset(int[] ruleset, int a, int b, int c)
    {
        if (a == 1 && b == 1 && c == 1) return ruleset[0];
        if (a == 1 && b == 1 && c == 0) return ruleset[1];
        if (a == 1 && b == 0 && c == 1) return ruleset[2];
        if (a == 1 && b == 0 && c == 0) return ruleset[3];
        if (a == 0 && b == 1 && c == 1) return ruleset[4];
        if (a == 0 && b == 1 && c == 0) return ruleset[5];
        if (a == 0 && b == 0 && c == 1) return ruleset[6];
        if (a == 0 && b == 0 && c == 0) return ruleset[7];
        return 999;
    }
}
