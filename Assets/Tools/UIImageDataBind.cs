using AxGrid.Base;
using UnityEngine;
using UnityEngine.UI;

namespace AxGrid.Tools.Binders
{
    [RequireComponent(typeof(Image))]
    public class UIImageDataBind : Binder
    {
        protected Image image;

        [SerializeField] Sprite DefaultSprite;

        [Tooltip("������ �� �������� ������� ������� � ������.")]
        public string spriteLinkField = "";

        [OnAwake]
        protected void Init() => image = GetComponent<Image>();

        [OnStart]
        protected void Subscribe()
        {
            if (string.IsNullOrWhiteSpace(spriteLinkField)) throw new System.NullReferenceException($"�������� ��� ������ ���������� � ������� {gameObject.name} �� ������!");
            Model.EventManager.AddAction($"On{spriteLinkField}Changed", DrawSpriteChange);

            DrawSpriteChange();
        }

        void DrawSpriteChange()
        {
            image.sprite = Model.Get(spriteLinkField, DefaultSprite);
            Log.Info($"{gameObject.name}: ������ ������.");
        }

        [OnDestroy]
        protected void Unsubscribe()
        {
            Model.EventManager.RemoveAction($"On{spriteLinkField}Changed", DrawSpriteChange);
        }
    }
}
