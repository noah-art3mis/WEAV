using UnityEngine;
using System;
using System.Collections.Generic;

public class CellUpdater : MonoBehaviour
{
    CA ca;

    private void Start()
    {
        ca = GetComponent<CA>();
    }

    public void UpdateCells(int[] ruleset, int[] cells, int[] nextgen, bool scrolling)
    {
        if (!scrolling)
        {
            for (int generation = 0; generation < ca.maxGenerations; generation++)
            {
                DrawRow(generation);
                GenerateRow(ruleset, cells);
                Array.Copy(nextgen, cells, cells.Length); // cells = nextgen; ===> pra versao anterior
            }
        }
        else
        {
            InvokeRepeating(nameof(UpdateScrolling), ca.repeatRate, ca.repeatRate);
        }
    }

    private void UpdateScrolling()
    {
        DrawRow(ca.maxGenerations);
        ScrollUp(ca.usedSprites);
        GenerateRow(ca.ruleset, ca.cells);
        Array.Copy(ca.nextgen, ca.cells, ca.arraySize);
    }

    private void ScrollUp(List<GameObject> usedSprites)
    {
        for (int i = usedSprites.Count - 1; i >= 0; i--)
        {
            usedSprites[i].transform.position += new Vector3(0, 1, 0);

            if (usedSprites[i].transform.position.y > 0)
            {
                BackToPool(usedSprites[i]);
            }
        }

        //test for performance
        //foreach (GameObject sprite in usedSprites.ToList())
        //{
        //    sprite.transform.position += new Vector3(0, 1, 0);

        //    if (sprite.transform.position.y > 0)
        //    {
        //        BackToPool(sprite);
        //    }
        //}
    }

    private void DrawRow(int yPos)
    {
        Vector2 cellPosition = Vector2.zero;

        for (int i = 0; i < ca.arraySize; i++)
        {
            if (ca.cells[i] == 1)
            {
                cellPosition.x = i;
                cellPosition.y = -yPos;

                if (ca.spritePool.Count > 0)
                {
                    GameObject sprite = ca.spritePool.Dequeue();
                    sprite.transform.position = cellPosition;
                    sprite.SetActive(true);
                    ca.usedSprites.Add(sprite);
                }
                else
                {
                    GameObject sprite = Instantiate(ca.image, cellPosition, Quaternion.identity, this.transform);
                    sprite.SetActive(true);
                    ca.usedSprites.Add(sprite);
                }
            }
        }
    }

    private void GenerateRow(int[] ruleset, int[] cells)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            //wrap around
            int left = cells[(i - 1 + cells.Length) % cells.Length];
            int me = cells[(i + 0 + cells.Length) % cells.Length];
            int right = cells[(i + 1 + cells.Length) % cells.Length];
            ca.nextgen[i] = ApplyRuleset(ruleset, left, me, right);
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
    public void BackToPool(GameObject sprite)
    {
        sprite.SetActive(false);
        ca.spritePool.Enqueue(sprite);
        ca.usedSprites.Remove(sprite);
    }
}
