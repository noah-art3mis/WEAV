using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

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
    public float repeatRate = 0.1f;
    public Vector2 gridSize;
    public int arraySize;
    public int maxGenerations;

    public static int[] ruleset = new int[Defaults.RULESET_SIZE];
    private int[] cells;
    private int[] nextgen;

    Queue<GameObject> spritePool = new Queue<GameObject>();
    List<GameObject> usedSprites = new List<GameObject>();

    public static float pixelDistance;

    private void Start()
    {
        settings = GetComponent<SettingsManager>();
        myCamera = GetComponent<MyCamera>();

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

        UpdateCells(settings.isScrolling);
    }

    public void ResetGrid()
    {
        Array.Clear(ruleset, 0, ruleset.Length);
        Array.Clear(cells, 0, cells.Length);
        Array.Clear(nextgen, 0, nextgen.Length);

        CancelInvoke();

        foreach (GameObject sprite in usedSprites.ToList()) //performance melhor que o loop ao contrario e o clear
            BackToPool(sprite);
    }

    private void ComputeSize(Vector2 gridSize)
    {
        if (!(cells.Length == gridSize.x))
        {
            cells = new int[(int)gridSize.x]
            nextgen = new int[(int)gridSize.x];
        }
        arraySize = (int)gridSize.x;
        maxGenerations = (int)gridSize.y;
    }



    private void BackToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
        spritePool.Enqueue(gameObject);
        usedSprites.Remove(gameObject);
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
            InvokeRepeating(nameof(UpdateScrolling), repeatRate, repeatRate);
        }
    }

    private void UpdateScrolling()
    {
        int gen = 0;
        gen++;

        DrawRow(maxGenerations);
        ScrollUp();
        GenerateRow();
        Array.Copy(nextgen, cells, arraySize);
    }

    private void ScrollUp()
    {
        for (int i = usedSprites.Count - 1; i >= 0; i--)
        {
            usedSprites[i].transform.position += new Vector3(0, 1, 0);
            
            if (usedSprites[i].transform.position.y > 0)
                BackToPool(usedSprites[i]);
        }

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

        for (int i = 0; i < arraySize; i++)
        {
            if (cells[i] == 1)
            {
                cellPosition.x = i;
                cellPosition.y = -yPos;

                if (spritePool.Count > 0)
                {
                    GameObject sprite = spritePool.Dequeue();
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

