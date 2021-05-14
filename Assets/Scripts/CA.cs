using UnityEngine;
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
    [SerializeField] private GameObject ui;
    [SerializeField] private MyCamera myCamera;

    private SettingsManager settings;
    private GameObject cellsParent;

    public static event Action<int[], string> settingsDone;
    public static string startInfo;

    [Header("Settings")]
    public static int maxGenerations = 100;
    public static int arraySize = maxGenerations; //maybe change

    public static int[] cells = new int[arraySize];
    public static int[] ruleset = new int[8];

    Queue<GameObject> sprites = new Queue<GameObject>();
    List<GameObject> usedSprites = new List<GameObject>();

    public static float pixelDistance = 0.01f;

    private void Start()
    {
        settings = GetComponent<SettingsManager>();
        myCamera = GetComponent<MyCamera>();

        CreateSprites();
    }

    public void Run(string upOrDown)
    {
        myCamera.ResetCamera();
        ResetGrid();

        settings.ComputeSettings();
        ruleset = settings.GetRuleset(upOrDown);
        cells = settings.SetFirstGeneration();

        settingsDone?.Invoke(ruleset, startInfo);

        UpdateCells();
    }

    Vector2 spriteStartPosition = new Vector2(arraySize / 2 * pixelDistance, maxGenerations / 2 * pixelDistance + 2 * pixelDistance);
    private void CreateSprites()
    {
        for (int i = 0; i < arraySize * maxGenerations; i++)
        {
            var sprite = Instantiate(image, spriteStartPosition, Quaternion.identity, this.transform);
            sprite.SetActive(false); 
            sprites.Enqueue(sprite);
        }
    }

    [UnityEditor.MenuItem("Tools/ResetGrid")]
    public void ResetGrid()
    {
        Array.Clear(ruleset, 0, ruleset.Length);
        Array.Clear(cells, 0, cells.Length);

        foreach (GameObject sprite in usedSprites)
        {
            sprite.SetActive(false);
            sprites.Enqueue(sprite);

        }
        usedSprites.Clear();
    }

    private void UpdateCells()
    {
        for (int generation = 0; generation < maxGenerations; generation++)
        {
            DrawNewRow(generation);
            GenerateRow();
        }
    }

    private int[] nextgen = new int[arraySize];
    private void GenerateRow()
    {
        for (int i = 1; i < arraySize - 1; i++) // ignores borders
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

 
    private void DrawNewRow(int yPos)
    {
        Vector2 cellPosition = Vector2.zero;

        for (int i = 0; i < arraySize; i++)
        {
            if (cells[i] == 1)
            {
                cellPosition.x = i * pixelDistance;
                cellPosition.y = -yPos * pixelDistance;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            Run("");
        }
        
        if (!settings.randomRuleset)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) 
                Run("up");
            if (Input.GetKeyDown(KeyCode.DownArrow))
                Run("down");
        }

        if (Input.GetKeyDown("tab"))
        {
            if (ui.activeSelf)
                ui.SetActive(false);
            else
                ui.SetActive(true);
        }
    }
}

