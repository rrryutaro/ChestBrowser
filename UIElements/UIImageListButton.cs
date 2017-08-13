using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.UI;

namespace ChestBrowser.UIElements
{
    internal class UIImageListButton : UIElement
    {
        private List<Texture2D> _textures;
        private List<string> _hoverTexts;
        private float _visibilityActive = 1f;
        private float _visibilityInactive = 0.4f;
        internal int Index { get; set; }

        public UIImageListButton(List<Texture2D> textures, List<string> hoverTexts)
        {
            this._textures = textures;
            this._hoverTexts = hoverTexts;
            this.Width.Set((float)this._textures[0].Width, 0f);
            this.Height.Set((float)this._textures[0].Height, 0f);
            Index = 0;
        }

        public void AddImage(Texture2D texture)
        {
            this._textures.Add(texture);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = base.GetDimensions();
            spriteBatch.Draw(this._textures[Index], dimensions.Position(), Color.White * (base.IsMouseHovering ? this._visibilityActive : this._visibilityInactive));
            if (IsMouseHovering)
            {
                ChestBrowser.instance.chestBrowserTool.tooltip = _hoverTexts[Index];
            }
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
            Main.PlaySound(12, -1, -1, 1, 1f, 0f);
        }

        public void SetVisibility(float whenActive, float whenInactive)
        {
            this._visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
            this._visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
        }
    }
}
