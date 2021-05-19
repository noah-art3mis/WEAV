using UnityEngine;

public class AdvancedSettings : MonoBehaviour
{

    private void Start()
    {
        GameObject[] array = GameObject.FindGameObjectsWithTag("Advanced Setting");
    }

    public static void ToggleActive(GameObject[] array)
    {
        foreach (GameObject item in array)
        {
            item.SetActive(!item.activeSelf);
        }
    }
}
