using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    CA ca;
    SettingsManager settings;
    [SerializeField] private GameObject mainPanel;
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

        if (!settings.isRandomRuleset)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                ca.Run("up");
            if (Input.GetKeyDown(KeyCode.DownArrow))
                ca.Run("down");
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (mainPanel.activeSelf)
                mainPanel.SetActive(false);
            else
                mainPanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            if (infoPanel.activeSelf)
                infoPanel.SetActive(false);
            else
                infoPanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(nameof(TakeScreenshot));
        }
    }

    IEnumerator TakeScreenshot()
    {
        infoPanel.SetActive(false);
        yield return new WaitForEndOfFrame(); //wait one frame for info to disappear

        string directory = System.IO.Directory.GetCurrentDirectory() + "/Screenshots/";

        if (!System.IO.Directory.Exists(directory))
            System.IO.Directory.CreateDirectory(directory);

        string date = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string filename = infoPanelRule.text.ToString() + "_" + infoPanelStart.text.ToString() + "_" + date + ".png";
        string path = directory + filename;
        ScreenCapture.CaptureScreenshot(path, 4);
        Debug.Log("saved ss to " + path); //make this show in UI

        yield return new WaitForEndOfFrame();
        infoPanel.SetActive(true);
    }
}
