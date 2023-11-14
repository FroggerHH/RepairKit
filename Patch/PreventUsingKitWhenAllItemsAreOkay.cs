#pragma warning disable CS8509
namespace RepairKit.Patch;

[HarmonyPatch]
public static class PreventUsingKitWhenAllItemsAreOkay
{
    [HarmonyPatch(typeof(Player), nameof(Player.ConsumeItem))] [HarmonyPrefix] [HarmonyWrapSafe]
    private static bool Patch(Player __instance, ref bool __result, ItemData item)
    {
        var repairMode = item.m_dropPrefab.GetPrefabName() switch
        {
            "JF_ItemsRepairKit" => RepairMode.Items,
            "JF_ArmorRepairKit" => RepairMode.Armor,
            _ => RepairMode.None
        };
        if (repairMode == RepairMode.None) return true;

        foreach (var itemData in __instance.GetInventory().GetAllItems())
        {
            var itemType = itemData.m_shared.m_itemType;
            var isArmor = itemType == Helmet || itemType == Chest || itemType == Legs || itemType == Shoulder;
            if (repairMode == RepairMode.Armor && !isArmor) continue;
            if (repairMode == RepairMode.Items && isArmor) continue;
            if (itemData.m_shared.m_useDurability && itemData.m_durability < itemData.GetMaxDurability()) return true;
        }

        __instance.Message(MessageHud.MessageType.Center, "$noItemsToRepair");
        __result = false;
        return false;
    }
}