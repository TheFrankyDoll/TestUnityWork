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

        [Tooltip("Ссылка на значение нужного спрайта в модели.")]
        public string spriteLinkField = "";

        [OnAwake]
        protected void Init() => image = GetComponent<Image>();

        [OnStart]
        protected void Subscribe()
        {
            if (string.IsNullOrWhiteSpace(spriteLinkField)) throw new System.NullReferenceException($"Значение для модели переменной у объекта {gameObject.name} не задано!");
            Model.EventManager.AddAction($"On{spriteLinkField}Changed", DrawSpriteChange);

            DrawSpriteChange();
        }

        void DrawSpriteChange()
        {
            image.sprite = Model.Get(spriteLinkField, DefaultSprite);
            Log.Info($"{gameObject.name}: спрайт изменён.");
        }

        [OnDestroy]
        protected void Unsubscribe()
        {
            Model.EventManager.RemoveAction($"On{spriteLinkField}Changed", DrawSpriteChange);
        }
    }
}
