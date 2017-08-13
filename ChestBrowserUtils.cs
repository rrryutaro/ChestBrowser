using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;

namespace ChestBrowser
{
    public static class ChestBrowserUtils
    {
        public const int tileSize = 16;

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
            Tile tile = chest.getTile();
            int itemType = tile.type == 88 ? Chest.dresserTypeToIcon[(int)(tile.frameX / 54)] : Chest.chestTypeToIcon[(int)(tile.frameX / 36)];

            result.SetDefaults(itemType);

            return result;
        }
        public static Tile getTile(this Chest chest)
        {
            Tile result = Main.tile[chest.x, chest.y];
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
    }
}
