using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class CreateLabelFormUGUI : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_Dropdown classDropdown;
    [SerializeField] Button confirmButton;
    [SerializeField] Button cancelButton;


    public event Action<string, BaguaZone> OnConfirm;

    void Awake()
    {
        classDropdown.ClearOptions();
        classDropdown.AddOptions(Enum.GetNames(typeof(BaguaZone)).ToList());


        confirmButton.onClick.AddListener(Confirm);
        cancelButton.onClick.AddListener(Close);

        nameInput.onValueChanged.AddListener(_ => Refresh());
    }
    void OnEnable()
    {
        nameInput.text = "";
        classDropdown.value = 0;
        Refresh();
        nameInput.ActivateInputField();
    }

    void Refresh()
    {
        confirmButton.interactable = !string.IsNullOrWhiteSpace(nameInput.text);
    }

    void Confirm()
    {
        var labelName = nameInput.text.Trim();
        var selected = (BaguaZone)classDropdown.value;

        Debug.Log($"[CreateLabelForm] Nome={labelName}, Classe={selected}");
        OnConfirm?.Invoke(labelName, selected);
        Close();
    }

    public void Open() => gameObject.SetActive(true);
    public void Close() => gameObject.SetActive(false);

}