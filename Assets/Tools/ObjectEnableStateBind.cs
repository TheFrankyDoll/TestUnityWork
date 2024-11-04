using AxGrid.Base;
using UnityEngine;

namespace AxGrid.Tools.Binders
{
    public class ObjectEnableStateBind : Binder
    {
        [Tooltip("��� Bool-�������� � ������ ��� ���������/���������� ����� �������.")]
        public string enableStateField = "";

        [Tooltip("������� �� ������ �� ���������?")]
        public bool defaultEnable = true;


        [OnStart]
        public void Subscribe()
        {
            if (string.IsNullOrWhiteSpace(enableStateField)) throw new System.NullReferenceException($"�������� ��� ������ ���������� � ������� {gameObject.name} �� ������!");
            Model.EventManager.AddAction($"On{enableStateField}Changed", OnStateChange);
            OnStateChange();
        }


        public void OnStateChange()
        {
            if (gameObject.activeSelf != Model.GetBool(enableStateField, defaultEnable))
            {
                gameObject.SetActive(Model.GetBool(enableStateField, defaultEnable));
                Log.Info($"������ ������� {gameObject.name} ������: {gameObject.activeSelf}");
            }
        }

        [OnDestroy]
        public void Unsubscribe()
        {
            Model.EventManager.RemoveAction($"On{enableStateField}Changed", OnStateChange);
        }

    }
}
