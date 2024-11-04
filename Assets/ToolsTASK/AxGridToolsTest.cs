using AxGrid.Base;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Collections.ObjectModel;
using AxGrid;
using UnityEngine.UIElements;

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


    //void modelTest()
    //{
    //    var a = new List<string>();
        
    //    Model.Set("A", a);
    //    Model.EventManager.AddAction($"OnAChanged", ping);

    //    a.Add("TEXT");
    //    Model.Set("A", a); //ping НЕ вызовется


    //    var b = Model.Get<List<string>>("A");

    //    b.Add("TEXT2");
    //    b.RemoveAt(0);
    //    Model.Set("A", b); //ping НЕ вызовется

    //    Model.Set("A", new List<string>(b)); //ping ВЫЗОВИТСЯ, т.к. передан другой объект (в данном случае копия).
    //}
    //void ping() => Log.Info("!");


    public void SendChanges()
    {
        // 1.
        Model.Set(objectToEnableFieldValue, activeSelfValue);

        // 2.
        Model.Set($"{DropdownNameField}Interactable", DropdownActive);

        //ЛИБО отправить копию списка.
        //Model.Set($"{DropdownNameField}Options", new List<TMPro.TMP_Dropdown.OptionData>(DropdownOptions));
        //ЛИБО указать что данные были обновлены через Model.Refresh.
        Model.Set($"{DropdownNameField}Options", DropdownOptions);
        Model.Refresh($"{DropdownNameField}Options");

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