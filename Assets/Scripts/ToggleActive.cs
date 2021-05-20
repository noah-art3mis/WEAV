using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    public void Toggle() => gameObject.SetActive(!gameObject.activeSelf);
}
