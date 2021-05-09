using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA : MonoBehaviour
{
    
    Camera _camera;
    int[] cells;
    int[] nextgen;
    int screenWidth;
    int screenHeight;
    int generation = 0;

    //parameters
    public Sprite image;
    //public float repeatRate = 0.1f;
    public bool randomStart = true;
    public bool randomRuleset = true;
    //public bool scrolling = false;
    public int resolution = 1;
    public int[] ruleset;
    
    void Start()
    {
        _camera = Camera.main;
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        _camera.transform.position = new Vector2(screenWidth / 2, -screenHeight / 2);
        cells = new int[screenWidth / resolution];
        nextgen = new int[screenWidth / resolution];
        SetRules();
        SetFirstGeneration();
        UpdateCells();
    }

    private void Update()
    {
        //if event run then run
    }

    private void SetRules()
    {
        if (randomRuleset) 
        {
            for (int i = 0; i < ruleset.Length; i++)
            {
                ruleset[i] = UnityEngine.Random.Range(0, 2);
            }
            //convert to decimal
            //draw
        }
        else
        {
            //ruleset = { 0, 0, 0, 1, 0, 0, 1, 0 }; //18
        }


    }

    private void SetFirstGeneration()
    {
        if (randomStart)
        {
            for (int i = 0; i < screenWidth; i++)
            {
                cells[i] = UnityEngine.Random.Range(0, 2);
            }
        }
        else
        {
            for (int i = 0; i < screenWidth; i++)
            {
                cells[i] = 0;
            }
            cells[cells.Length / 2] = 1;
        }
    }

    void UpdateCells()
    {
        for (int i = 0; i < screenHeight; i++)
        {
            Generate();
            DrawNewGeneration();
        }
    }


    private void Generate()
    {
        for (int i = 1; i < cells.Length - 1; i++) // ignores borders
        {
            int left = cells[i - 1];
            int me = cells[i];
            int right = cells[i + 1];
            nextgen[i] = ApplyRuleset(left, me, right);
        }
        cells = nextgen;
    }

    private int ApplyRuleset(int a, int b, int c)
    {
        if (a == 1 && b == 1 && c == 1) return ruleset[7];
        if (a == 1 && b == 1 && c == 0) return ruleset[6];
        if (a == 1 && b == 0 && c == 1) return ruleset[5];
        if (a == 1 && b == 0 && c == 0) return ruleset[4];
        if (a == 0 && b == 1 && c == 1) return ruleset[3];
        if (a == 0 && b == 1 && c == 0) return ruleset[2];
        if (a == 0 && b == 0 && c == 1) return ruleset[1];
        if (a == 0 && b == 0 && c == 0) return ruleset[0];
        return 2;
    }

    private void DrawNewGeneration()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] == 1)
            {
                Instantiate<Sprite>(image, new Vector2(i, -generation), Quaternion.identity);
            }
        }
        generation++;
    }
}

