using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    CA ca;
    SettingsManager settings;
    [SerializeField] private GameObject ui;
    [SerializeField] private Text infoPanelRule;
    [SerializeField] private Text infoPanelStart;
    [SerializeField] private GameObject infoPanel;

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

        if (Input.GetKeyDown(KeyCode.P))
        {
            string directory = System.IO.Directory.GetCurrentDirectory() + "/Screenshots/";

            if (!System.IO.Directory.Exists(directory))
            {
                System.IO.Directory.CreateDirectory(directory);
                Debug.Log("created" + directory + "folder");
            }

            string date = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
            string filename = infoPanelRule.text.ToString() + "_" + infoPanelStart.text.ToString() + "_" + date + ".png";

            string path = directory + filename;

            infoPanelRule.transform.parent.gameObject.SetActive(false);
            ScreenCapture.CaptureScreenshot(path);
            infoPanelRule.transform.parent.gameObject.SetActive(true);
            Debug.Log("saved ss to " + path); //make this show in UI
        }
    }
}
