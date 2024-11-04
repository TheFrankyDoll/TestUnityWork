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

        [Tooltip("Имя выпадающего меню (если пустое берется из имени объекта)")]
        public string dropdownName = "";

        [Space(5)]

        [Tooltip("Имя Bool-значения в модели для включения/выключения взаимодействия с этим объектом.\n" +
            "> Влияет на 'dropdown.interactable'.")]
        public string enableField = "";

        [Tooltip("Активен ли объект по умолчанию?")]
        public bool defaultEnable = true;

        [Space(5)]

        [Tooltip("Имя TMP_Dropdown.OptionData-значения в модели для вариантов выбора в списке.\n" +
            "> Влияет на 'dropdown.options'.")]
        public string optionsField = "";

        [Tooltip("Список опций по умолчанию.")]
        public List<TMP_Dropdown.OptionData> defaultOptions = new List<TMP_Dropdown.OptionData>();

        [Space(5)]

        [Tooltip("Имя int-значения в модели для выбранного варианта.\n" +
            "> Влияет на 'dropdown.value'.")]
        public string selectedOptionField = "";

        [Tooltip("Выбранная опция по умолчанию.")]
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

        /// <summary> Обновить весь Dropdown целиком, перерисовав актуальные данные из модели. </summary>
        [OnStart]
        public virtual void Redraw()
        {
            DrawInteractableChange();
            DrawOptionsChange();
            DrawSelectedChange();
        }

        public void DrawInteractableChange() => dropdown.interactable = Model.GetBool(enableField, defaultEnable);

        public void DrawOptionsChange()
        {
            dropdown.options = Model.Get(optionsField, defaultOptions);
            dropdown.RefreshShownValue();
        }

        public void DrawSelectedChange() => dropdown.value = Model.GetInt(selectedOptionField, defaultSelectedOption);


        public void OnValueChanged(int newValueID)
        {
            if (!dropdown.interactable || !isActiveAndEnabled) return;

            Log.Info($"{dropdownName} Новое значение получено: {newValueID}");

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