namespace RepairKit.Patch;

[HarmonyPatch]
public static class DoRemLogic
{
    public const ItemType COSMETIC_CHEST = (ItemType)30;
    public const ItemType COSMETIC_HELMET = (ItemType)31;
    public const ItemType COSMETIC_LEGS = (ItemType)32;
    public const ItemType COSMETIC_CAPE = (ItemType)33;

    [HarmonyPatch(typeof(Player), nameof(Player.ConsumeItem))] [HarmonyPostfix] [HarmonyWrapSafe]
    private static void Patch(Player __instance, bool __result, ItemData item)
    {
        if (!__result) return;
        var repairMode = item.m_dropPrefab.GetPrefabName() switch
        {
            "JF_ItemsRepairKit" => RepairMode.Items,
            "JF_ArmorRepairKit" => RepairMode.Armor,
            _ => RepairMode.None
        };

        if (repairMode == RepairMode.None) return;
        foreach (var itemData in __instance.GetInventory().GetAllItems())
        {
            var shared = itemData.m_shared;
            var itemType = shared.m_itemType;

            if (shared.m_useDurability == false) continue;
            var isArmor = itemType == Helmet || itemType == Chest || itemType == Legs || itemType == Shoulder ||
                          itemType == COSMETIC_CHEST || itemType == COSMETIC_HELMET ||
                          itemType == COSMETIC_LEGS || itemType == COSMETIC_CAPE;
            var isOther = !isArmor;
            float value = 0;
            if (isArmor && repairMode == RepairMode.Armor)
                value = armorKit_percent.Value;
            else if (isOther && repairMode == RepairMode.Items)
                value = itemKit_percent.Value;

            itemData.m_durability += itemData.GetMaxDurability() * value / 100;
        }
    }
}