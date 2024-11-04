using AxGrid.Base;
using UnityEngine;

namespace AxGrid.Tools.Binders
{
    public class ObjectEnableStateBind : Binder
    {
        [Tooltip("Имя Bool-значения в модели для включения/выключения этого объекта.")]
        public string enableStateField = "";

        [Tooltip("Включен ли объект по умолчанию?")]
        public bool defaultEnable = true;


        [OnStart]
        public void Subscribe()
        {
            if (string.IsNullOrWhiteSpace(enableStateField)) throw new System.NullReferenceException($"Значение для модели переменной у объекта {gameObject.name} не задано!");
            Model.EventManager.AddAction($"On{enableStateField}Changed", OnStateChange);
            OnStateChange();
        }


        public void OnStateChange()
        {
            if (gameObject.activeSelf != Model.GetBool(enableStateField, defaultEnable))
            {
                gameObject.SetActive(Model.GetBool(enableStateField, defaultEnable));
                Log.Info($"Статус объекта {gameObject.name} изменён: {gameObject.activeSelf}");
            }
        }

        [OnDestroy]
        public void Unsubscribe()
        {
            Model.EventManager.RemoveAction($"On{enableStateField}Changed", OnStateChange);
        }

    }
}
