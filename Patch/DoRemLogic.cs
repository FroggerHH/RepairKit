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
        Debug($"DoRemLogic: repairMode: {repairMode}");

        if (repairMode == RepairMode.None) return;
        foreach (var itemData in __instance.GetInventory().GetAllItems())
        {
            var shared = itemData.m_shared;
            var itemType = shared.m_itemType;

            if (shared.m_useDurability == false) continue;
            var isArmor = itemType == Helmet || itemType == Chest || itemType == Legs || itemType == Shoulder;
            // var isOther = itemType == Shield || itemType == Misc || itemType == Tool || itemType == Utility;
            var isOther = !isArmor;

            var value = isArmor && repairMode == RepairMode.Armor
                ? armorKit_percent.Value
                : (isOther && repairMode == RepairMode.Items ? itemKit_percent.Value : 0);
            Debug($"DoRemLogic: isArmor: {isArmor}, isOther: {isOther}, value: {value}");
            itemData.m_durability += itemData.GetMaxDurability() * value / 100;
        }
    }
}