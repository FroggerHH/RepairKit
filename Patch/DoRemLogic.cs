#pragma warning disable CS8509
namespace RepairKit.Patch;

[HarmonyPatch]
public static class DoRemLogic
{
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
            var isArmor = itemType == Helmet || itemType == Chest || itemType == Legs || itemType == Shoulder;
            // var isOther = itemType == Shield || itemType == Misc || itemType == Tool || itemType == Utility;
            var isOther = !isArmor;
            float value;
            if (isArmor && repairMode == RepairMode.Armor)
            {
                value = armorKit_percent.Value;
                value += itemData.m_quality * aditionalPercentsByKitQuality.Value;
                itemData.m_durability += itemData.GetMaxDurability() * value / 100;
            } else if (isOther && repairMode == RepairMode.Items)
            {
                value = itemKit_percent.Value;
                value += itemData.m_quality * aditionalPercentsByKitQuality.Value;
                itemData.m_durability += itemData.GetMaxDurability() * value / 100;
            }
        }
    }
}