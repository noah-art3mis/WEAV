using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class CA : MonoBehaviour
{
    //https://mathworld.wolfram.com/ElementaryCellularAutomaton.html

    [Header("Dependencies")]
    public GameObject image;
    [SerializeField] public InputField ruleInput;
    [SerializeField] private Text ruleOutput;
    [SerializeField] private Text startOutput;

    private SettingsManager settings;
    private MyCamera myCamera;
    private CellUpdater updater;

    public static event Action<int[], string> settingsDone;
    public static string startInfo;

    [Header("Settings")]
    public float repeatRate = 0.1f;
    public Vector2 gridSize;
    public int arraySize;
    public int maxGenerations;

    public static int[] ruleset = new int[Defaults.RULESET_SIZE];
    public int[] cells;
    public int[] nextgen;

    public Queue<GameObject> spritePool = new Queue<GameObject>();
    public List<GameObject> usedSprites = new List<GameObject>();

    public static float pixelDistance;

    private void Start()
    {
        settings = GetComponent<SettingsManager>();
        myCamera = GetComponent<MyCamera>();
        updater = GetComponent<CellUpdater>();

        gridSize = new Vector2(Defaults.GRID_SIZE, Defaults.GRID_SIZE);

        arraySize = (int)gridSize.x;
        maxGenerations = (int)gridSize.y;

        cells = new int[arraySize];
        nextgen = new int[cells.Length];

        CreateSprites(Defaults.SPRITE_POOL_SIZE);
    }

    private void CreateSprites(int poolSize)
    {
        for (int i = 0; i < poolSize; i++)
        {
            var sprite = Instantiate(image, Vector2.zero, Quaternion.identity, this.transform);
            sprite.SetActive(false);
            spritePool.Enqueue(sprite);
        }
    }

    public void Run(string upOrDown)
    {
        ResetGrid();

        settings.ComputeSettings();
        gridSize = settings.GetSize();
        ComputeSize(gridSize);
        myCamera.SetCamera(gridSize);

        repeatRate = settings.GetScrollSpeed();
        ruleset = settings.GetRuleset(upOrDown);
        cells = settings.SetFirstGeneration(arraySize);

        settingsDone?.Invoke(ruleset, startInfo);

        updater.UpdateCells(ruleset, cells, settings.isScrolling);
    }

    public void ResetGrid()
    {
        Array.Clear(ruleset, 0, ruleset.Length);
        Array.Clear(cells, 0, cells.Length);
        Array.Clear(nextgen, 0, nextgen.Length);

        CancelInvoke();

        foreach (GameObject sprite in usedSprites.ToList()) //performance melhor que o loop ao contrario e o clear
            updater.BackToPool(sprite);
    }

    private void ComputeSize(Vector2 gridSize)
    {
        if (!(cells.Length == gridSize.x))
        {
            cells = new int[(int)gridSize.x];
            nextgen = new int[(int)gridSize.x];
        }
        arraySize = (int)gridSize.x;
        maxGenerations = (int)gridSize.y;
    }

}

