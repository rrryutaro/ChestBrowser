using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace ChestBrowser
{
    public static class ChestBrowserUtils
    {
        public const int tileSize = 16;
        public const int maxTilesX = 8400;
        public const int maxTilesY = 2400;


        public const int InfinityRange = int.MaxValue / 32 - 20;

        /// <summary>
        /// プレイヤーを指定の位置にテレポートさせる
        /// </summary>
        /// <remarks>
        /// HEROsMod の FlyCam.cs 99行目付近以降のコードを参考にした
        /// </remarks>
        /// <param name="x">タイル座標 X</param>
        /// <param name="y">タイル座標 Y</param>
        public static void Teleport(int x, int y)
        {
            Player player = Main.LocalPlayer;

            player.position.X = x * tileSize;
            player.position.Y = y * tileSize;
            player.fallStart = y;
        }

        public static Rectangle GetSearchRangeRectangle()
        {
            Rectangle result = new Rectangle();
            Point playerCenter = Main.LocalPlayer.Center.ToTileCoordinates();
            result.X = playerCenter.X - Config.searchRangeX / 2;
            result.Y = playerCenter.Y - Config.searchRangeY / 2;
            result.Width = Config.searchRangeX;
            result.Height = Config.searchRangeY;
            return result;
        }


        /// <summary>
        /// チェストを開く
        /// Player.tileRangeX、Player.tileRangeY の範囲までしか対象とならないので、操作範囲を拡張した状態にする
        ///  -> ChestBrowserPlayer.ResetEffects
        /// </summary>
        public static void OpenChest(Chest chest)
        {
            Main.LocalPlayer.chestX = chest.x;
            Main.LocalPlayer.chestY = chest.y;
            Main.LocalPlayer.tileInteractAttempted = true;
            Main.LocalPlayer.TileInteractionsCheck(chest.x, chest.y);
        }

        /// <summary>
        /// チェストをアイテム化した際のアイテムを取得する
        /// </summary>
        public static Item ToItem(this Chest chest)
        {
            Item result = new Item();
            int iconIndex = chest.getIconIndex();
            int itemType = chest.isDresser() ? Chest.dresserTypeToIcon[iconIndex] : Chest.chestTypeToIcon[iconIndex];
            result.SetDefaults(itemType);
            return result;
        }
        public static Tile getTile(this Chest chest)
        {
            Tile result = Main.tile[chest.x, chest.y];
            return result;
        }
        public static bool isDresser(this Chest chest)
        {
            bool result = chest.getTile().type == 88;
            return result;
        }
        public static int getIconIndex(this Chest chest)
        {
            int result = -1;
            short frameX = chest.getTile().frameX;
            result = chest.isDresser() ? frameX / 54 : frameX / 36;
            return result;
        }

        public static Vector2 getCenter(this Chest chest)
        {
            Vector2 result = new Vector2(chest.x * tileSize, chest.y * tileSize);
            if (chest.getTile().type == 88)
                result = result.Offset(tileSize + tileSize / 2, tileSize);
            else
                result = result.Offset(tileSize, tileSize);

            return result;
        }

        public static Vector2 Offset(this Vector2 position, float x, float y)
        {
            position.X += x;
            position.Y += y;
            return position;
        }

        public static Texture2D Resize(this Texture2D texture, int width, int height)
        {
            Texture2D result = texture;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                texture.SaveAsPng(ms, width, height);
                result = Texture2D.FromStream(texture.GraphicsDevice, ms);
            }
            return result;
        }

        /// <summary>
        /// 指定の座標のチェストがロックされている場合、強制的に解錠する。
        /// </summary>
        /// <remarks>
        /// Terraria.Chest.Unlock のコードをそのままコピーし、プランテラ撃破のチェックを外したのみ
        /// </remarks>
        public static bool ChestUnlock(int X, int Y)
        {
            int num = (int)(Main.tile[X, Y].frameX / 36);
            int num2 = num;
            short num3;
            int type;
            switch (num2)
            {
                case 2:
                    {
                        num3 = 36;
                        type = 11;
                        Terraria.GameContent.Achievements.AchievementsHelper.NotifyProgressionEvent(19);
                        goto IL_B7;
                    }
                case 3:
                    break;
                case 4:
                    {
                        num3 = 36;
                        type = 11;
                        goto IL_B7;
                    }
                default:
                    switch (num2)
                    {
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                            {
                                num3 = 180;
                                type = 11;
                                Terraria.GameContent.Achievements.AchievementsHelper.NotifyProgressionEvent(20);
                                goto IL_B7;
                            }
                        default:
                            switch (num2)
                            {
                                case 36:
                                case 38:
                                case 40:
                                    {
                                        num3 = 36;
                                        type = 11;
                                        goto IL_B7;
                                    }
                            }
                            break;
                    }
                    break;
            }
            return false;
            IL_B7:
            Main.PlaySound(22);
            for (int i = X; i <= X + 1; i++)
            {
                for (int j = Y; j <= Y + 1; j++)
                {
                    Tile expr_E8 = Main.tile[i, j];
                    expr_E8.frameX -= num3;
                    for (int k = 0; k < 4; k++)
                    {
                        Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, type, 0f, 0f, 0, default(Color), 1f);
                    }
                }
            }
            return true;
        }

    }
}
