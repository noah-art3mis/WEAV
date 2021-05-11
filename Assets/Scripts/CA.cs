using UnityEngine;
using UnityEngine.UI;

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
    private BinaryConverter converter;
    private GameObject cellsParent;

    public static int[] cells;
    public static int[] ruleset;
    private int[] nextgen;


    [Header("Settings")]
    public int maxGenerations = 100;
    public int arraySize = 100; 
    public int resolution = 10;
    
    private float pixelDistance = 0.01f;
    private int rulesetSize = 8;

    private void Start()
    {
        settings = GetComponent<SettingsManager>();
        converter = GetComponent<BinaryConverter>();

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        arraySize = maxGenerations;

        Camera _camera = Camera.main;
        _camera.transform.position = new Vector2(arraySize / 2 * pixelDistance, -maxGenerations / 2 * pixelDistance);
        _camera.orthographicSize = maxGenerations * pixelDistance * 0.5f; //fits camera vertically
    }

    public void Run(string parameter)
    {
        Reset();
        ruleset = settings.ComputeSettings(parameter);
        settings.SetFirstGeneration();
        UpdateCells();
    }

    public void Reset()
    {
        ruleset = new int[rulesetSize];
        cells = new int[arraySize];
        nextgen = new int[arraySize];

        Destroy(GameObject.Find("Cell Container"));
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
                Instantiate(image, new Vector2(i * pixelDistance, -yPos * pixelDistance), Quaternion.identity, cellsParent.transform);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            Run("");
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

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Run("up");
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Run("down");
        }
    }
}

