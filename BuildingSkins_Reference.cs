// ===============================================
// BUILDING SKINS REFERENCE - Extracted from BetterTC
// ===============================================
// This file contains the important skin-related code
// for use in other plugins.

using System.Collections.Generic;
using UnityEngine;

// ===============================================
// 1. TC SKIN DEFINITIONS
// ===============================================

public enum TCSkin
{
    Default,
    Retro,
    Shockbyte
}

public class TCSkinMeta
{
    public string ShortName;
    public string PrefabPath;
    public string EffectPath;
    public int ItemID;
    public int SkinID;
}

private static readonly Dictionary<TCSkin, TCSkinMeta> tcSkinMeta = new(){
    [TCSkin.Default] = new TCSkinMeta
    {
        ShortName = "cupboard.tool",
        PrefabPath = "assets/prefabs/deployable/tool cupboard/cupboard.tool.deployed.prefab",
        EffectPath = "assets/prefabs/deployable/tool cupboard/effects/tool-cupboard-deploy.prefab",
        ItemID = -97956382,
        SkinID = 0
    },
    [TCSkin.Retro] = new TCSkinMeta
    {
        ShortName = "cupboard.tool.retro",
        PrefabPath = "assets/prefabs/deployable/tool cupboard/retro/cupboard.tool.retro.deployed.prefab",
        EffectPath = "assets/prefabs/deployable/tool cupboard/retro/effects/tool-cupboard-retro-deploy.prefab",
        ItemID = 1488606552,
        SkinID = 10238
    },
    [TCSkin.Shockbyte] = new TCSkinMeta
    {
        ShortName = "cupboard.tool.shockbyte",
        PrefabPath = "assets/prefabs/deployable/tool cupboard/shockbyte/cupboard.tool.shockbyte.deployed.prefab",
        EffectPath = "assets/prefabs/deployable/tool cupboard/effects/tool-cupboard-deploy.prefab",
        ItemID = 1174957864,
        SkinID = 10239
    }
};

// ===============================================
// 2. EXTERNAL WALL PREFABS
// ===============================================

private Dictionary<int, string> WallPrefabs = new Dictionary<int, string>
{
    { 0, "assets/prefabs/building/wall.external.high.wood/wall.external.high.wood.prefab" },
    { 10302, "assets/prefabs/building/wall.external.high.legacy/wall.external.high.legacy.prefab" },
    { 1, "assets/prefabs/building/wall.external.high.stone/wall.external.high.stone.prefab" },
    { 10304, "assets/prefabs/building/wall.external.high.adobe/wall.external.high.adobe.prefab" },
    { 2, "assets/prefabs/misc/xmas/icewalls/wall.external.high.ice.prefab" },
};

// ===============================================
// 3. EXTERNAL GATE PREFABS
// ===============================================

private Dictionary<int, string> GatePrefabs = new Dictionary<int, string>
{
    { 0, "assets/prefabs/building/gates.external.high/gates.external.high.wood/gates.external.high.wood.prefab" },
    { 10302, "assets/prefabs/building/gates.external.high.legacy/gates.external.high.legacy.prefab" },
    { 1, "assets/prefabs/building/gates.external.high/gates.external.high.stone/gates.external.high.stone.prefab" },
    { 10304, "assets/prefabs/building/gates.external.high.adobe/gates.external.high.adobe.prefab" },
    { 2, "assets/prefabs/building/gates.external.high/gates.external.high.stone/gates.external.high.stone.prefab" },
};

// ===============================================
// 4. BUILDING BLOCK COLORS (for SetSkin)
// ===============================================

string[] colors = {
    "",                       // 0 - Default/None
    "0.25 0.56 0.75 1",      // 1 - Blue
    "0.25 0.72 0.31 1",      // 2 - Green
    "0.65 0.28 0.85 1",      // 3 - Purple
    "0.48 0.15 0.08 1",      // 4 - Brown/Red
    "0.92 0.46 0.06 1",      // 5 - Orange
    "0.87 0.87 0.87 1",      // 6 - White
    "0.18 0.18 0.16 1",      // 7 - Dark Gray
    "0.42 0.33 0.27 1",      // 8 - Tan
    "0.17 0.21 0.33 1",      // 9 - Navy
    "0.16 0.34 0.17 1",      // 10 - Dark Green
    "0.83 0.29 0.16 1",      // 11 - Red
    "0.85 0.53 0.38 1",      // 12 - Peach
    "0.90 0.67 0.15 1",      // 13 - Gold
    "0.34 0.32 0.31 1",      // 14 - Charcoal
    "0.08 0.33 0.37 1",      // 15 - Teal
    "0.68 0.61 0.56 1"       // 16 - Light Gray
};

// ===============================================
// 5. DLC SKIN ID RANGES
// ===============================================

// Format: (startSkinId, endSkinId, dlcSteamItemId)
private static readonly List<(int start, int end, int dlcSteamItemId)> skinIdRanges = new(){
    (10244, 10268, 10265), // DLC WP (Wallpaper Pack)
    (10272, 10279, 10280), // DLC LUNAR YEAR
    (10311, 10313, 10273), // DLC JUNGLE
    (10360, 10409, 10387), // DLC FLOOR (Flooring Pack)
};

// ===============================================
// 6. FREE/WHITELISTED SKINS (no DLC required)
// ===============================================

private static readonly List<ulong> whitelistedSkins = new List<ulong> {
    2,      // Ice
    10242,  // Free skin
    10243,  // Free skin
    10246,  // Free skin
    10372,  // Free skin
    10386,  // Free skin
    10384,  // Free skin
    10388,  // Free skin
    10401,  // Free skin
    10406   // Free skin
};

// ===============================================
// 7. WALLPAPER/FLOORING ITEM IDs
// ===============================================

// Wall Wallpaper ItemID: 553967074
// Floor Wallpaper ItemID: -551431036
// Ceiling Wallpaper ItemID: 1730664641

// To get all skins for each category:
// Wall skins: WallpaperSettings.WallpaperItemDef?.skins?.ToList()
// Floor skins: WallpaperSettings.FlooringItemDef?.skins?.ToList()
// Ceiling skins: WallpaperSettings.CeilingItemDef?.skins?.ToList()

// ===============================================
// 8. HOW TO SPAWN A TC WITH SKIN
// ===============================================

private void TCSkinReplace(BuildingPrivlidge tc, BasePlayer player, TCSkin skin)
{
    if (!tcSkinMeta.TryGetValue(skin, out var meta)) return;

    var pos = tc.transform.position;
    var rot = tc.transform.rotation;

    // Create the new TC entity
    var tcskin = GameManager.server.CreateEntity(meta.PrefabPath, pos, rot, true);
    if (tcskin == null) return;

    tcskin.OwnerID = tc.OwnerID;
    tcskin.Spawn();
    
    // Transfer inventory, auth list, etc. in NextTick
    NextTick(() => {
        var Building = tcskin as BuildingPrivlidge;
        if (Building == null) return;

        // Handle parenting (for barges, etc.)
        if (tc.HasParent()){
            var parent = tc.GetParentEntity();
            if (parent != null && !parent.IsDestroyed){
                tcskin.SetParent(parent, true);
            }
        }

        // Transfer inventory
        foreach (var item in tc.inventory.itemList.ToList()){
            item.MoveToContainer(Building.inventory);
        }

        // Transfer auth list
        foreach (var entry in tc.authorizedPlayers){
            Building.authorizedPlayers.Add(entry);
        }

        // Kill old TC
        tc.Kill();
    });
}

// ===============================================
// 9. HOW TO APPLY WALLPAPER/FLOORING SKIN
// ===============================================

// Use BuildingBlock.ChangeGradeAndSkin() method:
// block.ChangeGradeAndSkin(block.grade, skinId, true, true);

// Or use BuildingBlock.SetSkin() with a color:
// block.SetSkin(skinId, true);

// ===============================================
// 10. CHECK IF PLAYER OWNS DLC
// ===============================================

private bool IsSkinAllowed(BasePlayer player, int skinId)
{
    // Check whitelisted (free) skins
    if (whitelistedSkins.Contains((ulong)skinId)) return true;
    
    // Check DLC ownership
    foreach (var range in skinIdRanges)
    {
        if (skinId >= range.start && skinId <= range.end)
        {
            // Check if player owns DLC (range.dlcSteamItemId)
            return player.blueprints.steamInventory.HasItem(range.dlcSteamItemId);
        }
    }
    
    return true; // Default allow if not in any range
}
