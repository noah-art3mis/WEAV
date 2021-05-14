using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    CA ca;
    SettingsManager settings;
    [SerializeField] private GameObject ui;

    private void Start()
    {
        ca = GetComponent<CA>();
        settings = GetComponent<SettingsManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            ca.Run("");
        }

        if (!settings.randomRuleset)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                ca.Run("up");
            if (Input.GetKeyDown(KeyCode.DownArrow))
                ca.Run("down");
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
