using UnityEngine;
using UnityEngine.UI;
using System;

public class CA : MonoBehaviour
{   
    //TODO botoes delegates etc

    public static int[] cells;
    public static int[] ruleset;

    int[] nextgen;
    int screenWidth;
    int screenHeight;
    int arraySize;
    
    GameObject parent = null;
    GameObject ui;

    //parameters
    public bool randomStart = true;
    public bool randomRuleset = true;
    //public bool scrolling = false;
    //public float repeatRate = 0.1f;

    private float pixelDistance = 0.01f;
    private int rulesetSize = 8;
    public int maxGenerations = 100;
    public int resolution = 10;
    
    
    [SerializeField] private GameObject image;
    [SerializeField] private BinaryConverter converter;
    [SerializeField] private InputField ruleInput;
    [SerializeField] private Text ruleOutput;
    [SerializeField] private Text startOutput;

    private void Start()
    {
        ui = GameObject.Find("Main Panel");   
        
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        arraySize = maxGenerations;

        Camera _camera = Camera.main;
        _camera.transform.position = new Vector2(arraySize / 2 * pixelDistance, -maxGenerations / 2 * pixelDistance);
        _camera.orthographicSize = maxGenerations * pixelDistance * 0.5f; //fits camera vertically

    }

    public void Run()
    {
        Reset();
        SetRules();
        SetFirstGeneration();
        UpdateCells();
    }

    private void Reset()
    {
        ruleset = new int[rulesetSize];
        cells = new int[arraySize];
        nextgen = new int[arraySize];

        Destroy(GameObject.Find("Cell Container"));
        parent = new GameObject("Cell Container");
        parent.transform.parent = transform;
    }

    private void SetRules()
    {
        if (randomRuleset) 
        {
            for (int i = 0; i < ruleset.Length; i++)
            {
                ruleset[i] = UnityEngine.Random.Range(0, 2);
            }
        }
        else
        {
            int rulesetText = 0;
            if (rulesetText > 255) ruleInput.text = "255";
            rulesetText = int.Parse(ruleInput.text);
            ruleset = converter.RulesetDecimaltoBinary(rulesetText);
        }
        ruleOutput.text = "Rule " + converter.RulesetBinarytoDecimal();
    }

    private void SetFirstGeneration()
    {
        if (randomStart)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = UnityEngine.Random.Range(0, 2);
            }
            startOutput.text = "Random Start";
        }
        else
        {
            cells[cells.Length / 2] = 1;
            startOutput.text = "Normal Start";
        }
    }

    private void UpdateCells()
    {
        for (int generation = 0; generation < maxGenerations; generation++)
        {
            Generate();
            DrawNewGeneration(generation);
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
        return 999;
    }

    private void DrawNewGeneration(int yPos)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] == 1)
            {
                Instantiate(image, new Vector2(i * pixelDistance, -yPos * pixelDistance), Quaternion.identity, parent.transform);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("space") || Input.GetKeyDown("enter"))
        {
            Run();
        }
        if (Input.GetKeyDown("tab"))
        {
            if (ui.activeSelf)
            {
                ui.SetActive(false);
            }
            else
            {
                ui.SetActive(true);
            }
        }
    }
}

