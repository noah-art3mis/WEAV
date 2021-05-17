﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CA : MonoBehaviour
{
    //https://mathworld.wolfram.com/ElementaryCellularAutomaton.html

    [Header("Dependencies")]
    [SerializeField] private GameObject image;
    [SerializeField] public InputField ruleInput;
    [SerializeField] private Text ruleOutput;
    [SerializeField] private Text startOutput;

    private SettingsManager settings;
    private MyCamera myCamera;

    public static event Action<int[], string> settingsDone;
    public static string startInfo;

    [Header("Settings")]
    public int arraySize;
    public int maxGenerations;

    public static int[] ruleset = new int[8];
    private int[] cells;
    private int[] nextgen;

    Queue<GameObject> sprites = new Queue<GameObject>();
    List<GameObject> usedSprites = new List<GameObject>();

    public static float pixelDistance;

    private void Start()
    {
        settings = GetComponent<SettingsManager>();
        myCamera = GetComponent<MyCamera>();
        
        arraySize = 100;
        maxGenerations = arraySize;
        cells = new int[arraySize];
        nextgen = new int[arraySize];

        CreateSprites();
    }

    public void Run(string upOrDown)
    {
        myCamera.ResetCamera();
        ResetGrid();

        settings.ComputeSettings();
        ComputeSize(settings.GetSize());
        ruleset = settings.GetRuleset(upOrDown);
        cells = settings.SetFirstGeneration(arraySize);

        settingsDone?.Invoke(ruleset, startInfo);

        UpdateCells(settings.scrolling);
    }

    private void ComputeSize(int arraySizeInput)
    {
        if (arraySizeInput == arraySize) return;

        arraySize = arraySizeInput; 
        cells = new int[arraySize];
        nextgen = new int[arraySize];
        maxGenerations = arraySize;
    }

    private void CreateSprites()
    {
        Vector2 spriteStartPosition = new Vector2(arraySize / 2, maxGenerations / 2 + 2);
        for (int i = 0; i < arraySize * maxGenerations * 0.5f; i++)
        {
            var sprite = Instantiate(image, spriteStartPosition, Quaternion.identity, this.transform);
            sprite.SetActive(false); 
            sprites.Enqueue(sprite);
        }
    }

    public void ResetGrid()
    {
        Array.Clear(ruleset, 0, ruleset.Length);
        Array.Clear(cells, 0, cells.Length);
        Array.Clear(nextgen, 0, nextgen.Length);

        foreach (GameObject sprite in usedSprites) //while < arraysize * arraysize?
        {
            sprite.SetActive(false);
            sprites.Enqueue(sprite);
        }
        usedSprites.Clear();
    }

    private void UpdateCells(bool scrolling)
    {
        if (!scrolling)
        {
            for (int generation = 0; generation < maxGenerations; generation++)
            {
                DrawRow(generation);
                GenerateRow();
                Array.Copy(nextgen, cells, arraySize); // cells = nextgen; ===> pra versao anterior
            }
        }
        else
        {

        }
    }

    private void DrawRow(int yPos)
    {
        Vector2 cellPosition = Vector2.zero;

        for (int i = 0; i < arraySize; i++)
        {
            if (cells[i] == 1)
            {
                cellPosition.x = i;
                cellPosition.y = -yPos;

                if (sprites.Count <= 0)
                {
                    GameObject sprite = sprites.Dequeue();
                    sprite.transform.position = cellPosition;
                    sprite.SetActive(true);
                    usedSprites.Add(sprite);
                }
                else
                {
                    GameObject sprite = Instantiate(image, cellPosition, Quaternion.identity, this.transform);
                    sprite.SetActive(true);
                    usedSprites.Add(sprite);
                }
            }
        }
    }

    private void GenerateRow()
    {
        for (int i = 0; i < arraySize; i++)
        {
            //wrap around
            int left = cells[(i - 1 + cells.Length) % cells.Length];
            int me = cells[(i + 0 + cells.Length) % cells.Length];
            int right = cells[(i + 1 + cells.Length) % cells.Length];
            nextgen[i] = ApplyRuleset(left, me, right);
        }
    }

    private int ApplyRuleset(int a, int b, int c)
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

