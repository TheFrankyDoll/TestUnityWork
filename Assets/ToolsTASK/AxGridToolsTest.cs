using AxGrid.Base;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AxGridToolsTest : MonoBehaviourExt
{
    [Header("1. ObjectEnableStateBinder")]
    public string objectToEnableFieldValue = "ObjectActiveTest";
    public bool activeSelfValue = false;

    [Header("2. DropdownDataBinder")]

    public string DropdownNameField = "DropdownTest";
    public bool DropdownActive;
    public List<TMPro.TMP_Dropdown.OptionData> DropdownOptions;
    public int DropdownCurrentOption;

    [Header("3. ImageDataBinder")]

    public string TargetSpriteField = "TestSprite";
    public Sprite sprite;


    public void SendChanges()
    {
        // 1.
        Model.Set(objectToEnableFieldValue, activeSelfValue);

        // 2.
        Model.Set($"{DropdownNameField}Interactable", DropdownActive);
        Model.Set($"{DropdownNameField}Options", DropdownOptions);
        Model.Set($"{DropdownNameField}SelectedOption", DropdownCurrentOption);

        // 3.
        Model.Set(TargetSpriteField, sprite);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(AxGridToolsTest))]
public class AxGridToolsTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Отправить данные в модель"))
        {
            if (!EditorApplication.isPlaying) Debug.LogError("Модель не будет получать значения пока игра не запущенна. Запустите игру.");
            else (target as AxGridToolsTest).SendChanges();
        }
    }
}

#endif