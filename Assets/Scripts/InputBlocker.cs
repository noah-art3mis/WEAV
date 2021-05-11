using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputBlocker : MonoBehaviour
{
    private InputField field;

    private void Start()
    {
        field = GetComponent<InputField>();
    }

    public void ToggleInteractable()
    {
        field.interactable = !field.interactable;
        field.text = "";
    }
}
