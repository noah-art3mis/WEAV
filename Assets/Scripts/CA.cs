using UnityEngine;
using UnityEngine.UI;
using System;

public class CA : MonoBehaviour
{
    //https://mathworld.wolfram.com/ElementaryCellularAutomaton.html
    //https://plato.stanford.edu/entries/cellular-automata/supplement.html
    //http://atlas.wolfram.com/01/01/rulelist.html

    [Header("Dependencies")]
    [SerializeField] private GameObject image;
    [SerializeField] public InputField ruleInput;
    [SerializeField] private Text ruleOutput;
    [SerializeField] private Text startOutput;
    [SerializeField] private GameObject ui;

    private SettingsManager settings;
    private GameObject cellsParent;
    private Camera _camera;
    private UpdateCells cellUpdater;

    public static int[] cells;
    public static int[] ruleset;

    public static event Action<int[], string> settingsDone;

    [Header("Settings")]
    public static int rulesetSize = 8;

    private void Start()
    {
        settings = GetComponent<SettingsManager>();
        cellUpdater = GetComponent<UpdateCells>();

        int screenWidth = Screen.width;
        int screenHeight = Screen.height;
        arraySize = maxGenerations;

        _camera = Camera.main;

        Grid.Create();
    }

    public void Run(string parameter)
    {
        ResetCamera();
        Reset();

        ruleset = settings.ComputeSettings(parameter);
        int[] firstGen = settings.SetFirstGeneration();
        settingsDone?.Invoke(ruleset, startInfo);
        
        cellUpdater.GenerateGrid(ruleset, cells);
    }

    private void ResetCamera()
    {
        _camera.transform.position = new Vector2(arraySize / 2 * pixelDistance, -maxGenerations / 2 * pixelDistance + pixelDistance);
        _camera.orthographicSize = maxGenerations * pixelDistance * 0.5f; //fits camera vertically
    }

    public void Reset()
    {
        Array.Clear(ruleset, 0, ruleset.Length);
        Array.Clear(cells, 0, cells.Length);
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

        if (!settings.randomRuleset)
        {
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
}

