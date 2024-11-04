using AxGrid.Base;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace AxGrid.Tools.Binders
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class UIDropdownDataBind : Binder
    {
        protected TMP_Dropdown dropdown;

        [Tooltip("��� ����������� ���� (���� ������ ������� �� ����� �������)")]
        public string dropdownName = "";

        [Space(5)]

        [Tooltip("��� Bool-�������� � ������ ��� ���������/���������� �������������� � ���� ��������.\n" +
            "> ������ �� 'dropdown.interactable'.")]
        public string enableField = "";

        [Tooltip("������� �� ������ �� ���������?")]
        public bool defaultEnable = true;

        [Space(5)]

        [Tooltip("��� TMP_Dropdown.OptionData-�������� � ������ ��� ��������� ������ � ������.\n" +
            "> ������ �� 'dropdown.options'.")]
        public string optionsField = "";

        [Tooltip("������ ����� �� ���������.")]
        public List<TMP_Dropdown.OptionData> defaultOptions = new List<TMP_Dropdown.OptionData>();

        [Space(5)]

        [Tooltip("��� int-�������� � ������ ��� ���������� ��������.\n" +
            "> ������ �� 'dropdown.value'.")]
        public string selectedOptionField = "";

        [Tooltip("��������� ����� �� ���������.")]
        public int defaultSelectedOption = 0;


        [OnAwake]
        protected virtual void Init()
        {
            dropdown = GetComponent<TMP_Dropdown>();
            if (string.IsNullOrEmpty(dropdownName)) dropdownName = name;

            if (string.IsNullOrEmpty(enableField)) enableField = $"{dropdownName}Interactable";
            if (string.IsNullOrEmpty(optionsField)) optionsField = $"{dropdownName}Options";
            if (string.IsNullOrEmpty(selectedOptionField)) selectedOptionField = $"{dropdownName}SelectedOption";

            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        [OnStart]
        public virtual void Subscribe()
        {
            Model.EventManager.AddAction($"On{enableField}Changed", DrawInteractableChange);
            Model.EventManager.AddAction($"On{optionsField}Changed", DrawOptionsChange);
            Model.EventManager.AddAction($"On{selectedOptionField}Changed", DrawSelectedChange);
        }

        /// <summary> �������� ���� Dropdown �������, ����������� ���������� ������ �� ������. </summary>
        [OnStart]
        public virtual void Redraw()
        {
            DrawInteractableChange();
            DrawOptionsChange();
            DrawSelectedChange();
        }

        public void DrawInteractableChange() => dropdown.interactable = Model.GetBool(enableField, defaultEnable);

        public void DrawOptionsChange() => dropdown.options = Model.Get(optionsField, defaultOptions);

        public void DrawSelectedChange() => dropdown.value = Model.GetInt(selectedOptionField, defaultSelectedOption);


        public void OnValueChanged(int newValueID)
        {
            if (!dropdown.interactable || !isActiveAndEnabled) return;

            Log.Info($"{dropdownName} ����� �������� ��������: {newValueID}");

            Settings.Fsm?.Invoke($"On{dropdownName}ValueSet", newValueID, dropdown.options[newValueID]);
            Model.EventManager.Invoke($"On{dropdownName}ValueSet", newValueID, dropdown.options[newValueID]);

            Model.EventManager.Invoke("SoundPlay", "Click");
        }


        [OnDestroy]
        public virtual void Unsubscribe()
        {
            Model.EventManager.RemoveAction($"On{enableField}Changed", DrawInteractableChange);
            Model.EventManager.RemoveAction($"On{optionsField}Changed", DrawOptionsChange);
            Model.EventManager.RemoveAction($"On{selectedOptionField}Changed", DrawSelectedChange);
        }
    }
}