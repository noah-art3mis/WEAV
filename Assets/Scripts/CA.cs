using UnityEngine;
using UnityEngine.UI;
using System;

public class CA : MonoBehaviour
{
    //https://mathworld.wolfram.com/ElementaryCellularAutomaton.html

    [Header("Dependencies")]
    [SerializeField] private GameObject image;
    [SerializeField] public InputField ruleInput;
    [SerializeField] private Text ruleOutput;
    [SerializeField] private Text startOutput;
    [SerializeField] private GameObject ui;

    private SettingsManager settings;
    private GameObject cellsParent;
    private Camera _camera;

    public static event Action<int[], string> settingsDone;
    public static string startInfo;

    [Header("Settings")]
    public static int maxGenerations = 100;
    public static int arraySize = 100;

    public static int[] cells = new int[arraySize];
    public static int[] ruleset = new int[8];

    private float pixelDistance = 0.01f;

    private void Start()
    {
        settings = GetComponent<SettingsManager>();

        _camera = Camera.main;

        arraySize = maxGenerations; //now is a square. maybe rectangle in future
    }

    public void Run(string upOrDown)
    {
        ResetCamera();
        Reset();

        CreateSprites();

        settings.ComputeSettings();
        ruleset = settings.GetRuleset(upOrDown);
        cells = settings.SetFirstGeneration();

        settingsDone?.Invoke(ruleset, startInfo);

        UpdateCells();
    }

    private void CreateSprites()
    {
        throw new NotImplementedException();
    }

    private void ResetCamera()
    {
        Vector2 cameraPosition = Vector2.zero;
        cameraPosition.x = arraySize / 2 * pixelDistance;
        cameraPosition.y = maxGenerations / 2 * pixelDistance - pixelDistance;
        _camera.transform.position = new Vector2(cameraPosition.x, -cameraPosition.y);
        _camera.orthographicSize = maxGenerations * pixelDistance * 0.5f; //fits camera vertically
    }

    public void Reset()
    {
        Array.Clear(ruleset, 0, ruleset.Length);
        Array.Clear(cells, 0, cells.Length);

        Destroy(GameObject.Find("Cell Container")); //memory leak
        cellsParent = new GameObject("Cell Container");
        cellsParent.transform.parent = transform;
    }

    private void UpdateCells()
    {
        for (int generation = 0; generation < maxGenerations; generation++)
        {
            Generate();
            DrawNewGeneration(generation); 
        }
    }

    private int[] nextgen = new int[arraySize];
    private void Generate()
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

    Vector2 cellPosition = Vector2.zero;
    private void DrawNewGeneration(int yPos)
    {
        for (int i = 0; i < arraySize; i++)
        {
            if (cells[i] == 1)
            {
                cellPosition.x = i * pixelDistance;
                cellPosition.y = -yPos * pixelDistance;
                Instantiate(image, cellPosition, Quaternion.identity, cellsParent.transform); //memory
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

