using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ReLogic.Content;

namespace MoreSentries.Common;

public class TavernkeepSystem : ModSystem
{
    private const string assetPath = "MoreSentries/Textures";
    private Asset<Texture2D> ArrowTexture;
    public static Point TavernkeepTilePos = new Point(-1, -1);

    public override void Load()
    {
        ArrowTexture = ModContent.Request<Texture2D>($"{assetPath}/Arrow");
    }

    public override void ClearWorld()
    {
        TavernkeepTilePos = new Point(-1, -1);
    }

    public override void PostWorldLoad()
    {
        TavernkeepTilePos = GetSafePosition();
    }

    public override void PreUpdateWorld() {
        if (!IsTileVisibleToAnyPlayer(TavernkeepTilePos.X, TavernkeepTilePos.Y) && (Condition.DownedEaterOfWorlds.IsMet() || Condition.DownedBrainOfCthulhu.IsMet()) && !NPC.AnyNPCs(NPCID.BartenderUnconscious) && !NPC.AnyNPCs(NPCID.DD2Bartender))
        {
            NPC.NewNPC(null, TavernkeepTilePos.X * 16, TavernkeepTilePos.Y * 16, NPCID.BartenderUnconscious);
        }
    }

    public override void PostDrawTiles()
    {
        if (!Main.LocalPlayer.GetModPlayer<MoreSentriesPlayer>().TavernkeepRadar) return;

        Vector2 targetPos = Vector2.Zero;
        foreach (var npc in Main.ActiveNPCs)
        {
            if (npc.type == NPCID.BartenderUnconscious) targetPos = npc.Center;
        }
        if (targetPos == Vector2.Zero) return;

        SpriteBatch spriteBatch = Main.spriteBatch;

        Vector2 worldPos = Main.LocalPlayer.Center;
        Vector2 screenPos = Main.ScreenSize.ToVector2() / 2;

        Vector2 direction = targetPos - worldPos;
        float rotation = direction.ToRotation();

        spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullNone,
            null,
            Main.GameViewMatrix.TransformationMatrix
        );

        spriteBatch.Draw(
            ArrowTexture.Value,
            screenPos,
            null,
            Color.White,
            rotation,
            ArrowTexture.Size() / 2f - Vector2.UnitX * 80,
            1f,
            SpriteEffects.None,
            0f
        );

        spriteBatch.End();
    }

    public static bool IsTileOnScreen(Player player, int tileX, int tileY) {
        int worldX = tileX * 16;
        int worldY = tileY * 16;

        Rectangle screenRect = new Rectangle(
            (int)player.position.X - Main.screenWidth / 2,
            (int)player.position.Y - Main.screenHeight / 2,
            Main.screenWidth,
            Main.screenHeight
        );

        return screenRect.Contains(worldX, worldY);
    }

    public bool IsTileVisibleToAnyPlayer(int tileX, int tileY) {
        foreach (var player in Main.player) {
            if (!player.active) continue;
            if (IsTileOnScreen(player, tileX, tileY)) return true;
        }
        return false;
    }

    public static Point GetSafePosition()
    {
        int x = Main.rand.Next(0, Main.maxTilesX);
        int y = 0;

        while (!Main.tile[x, y].HasTile || !Main.tileSolid[Main.tile[x, y].TileType])
        {
            y++;
            if (y > Main.maxTilesY)
            {
                x = Main.rand.Next(0, Main.maxTilesX);
                y = 0;
                continue;
            }
        }

        if (y < Main.worldSurface * 0.6f) return GetSafePosition();

        return new Point(x, y - 2);
    }
}
