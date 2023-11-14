using BepInEx;
using BepInEx.Configuration;
using ItemManager;
using LocalizationManager;

namespace RepairKit;

[BepInPlugin(ModGUID, ModName, ModVersion)]
public class Plugin : BaseUnityPlugin
{
    private const string
        ModName = "RepairKit",
        ModVersion = "1.0.0",
        ModAuthor = "Frogger",
        ModGUID = $"com.{ModAuthor}.{ModName}";

    public static ConfigEntry<float> itemKit_percent;
    public static ConfigEntry<float> armorKit_percent;
    public static ConfigEntry<float> aditionalPercentsByKitQuality;


    private void Awake()
    {
        CreateMod(this, ModName, ModAuthor, ModVersion, ModGUID);

        itemKit_percent = config("General", "Items kit repair percent", 20f,
            "How much percent of durability is repaired for items");
        armorKit_percent = config("General", "Armor kit repair percent", 20f,
            "How much percent of durability is repaired for armor");
        aditionalPercentsByKitQuality = config("General", "Additional percents by kit quality", 15f,
            "This value is added to item and armor kit repair percent for each quality level");

        LoadAssetBundle("repairkit");
        var itemsRepairKit = new Item(bundle, "JF_ItemsRepairKit");
        itemsRepairKit.Name
            .Russian("Ремкомплект для предметов")
            .English("Repair kit for items");
        itemsRepairKit.Crafting.Add(CraftingTable.Workbench, 2);
        itemsRepairKit.RequiredItems.Add("FineWood", 5);
        itemsRepairKit.RequiredItems.Add("Ruby", 1);
        itemsRepairKit.CraftAmount = 2;

        var armorRepairKit = new Item(bundle, "JF_ArmorRepairKit");
        armorRepairKit.Name
            .Russian("Ремкомплект для брони")
            .English("Repair kit for armor");
        armorRepairKit.Crafting.Add(CraftingTable.Workbench, 2);
        armorRepairKit.RequiredItems.Add("FineWood", 5);
        armorRepairKit.RequiredItems.Add("Ruby", 1);
        armorRepairKit.CraftAmount = 2;

        bundle?.Unload(false);

        Localizer.Load();
        Localizer.AddPlaceholder("item_desc_ItemsRepairKit", "repairPercent", itemKit_percent);
        Localizer.AddPlaceholder("item_desc_ItemsRepairKit", "aditionalPercentsByKitQuality",
            aditionalPercentsByKitQuality);
        Localizer.AddPlaceholder("item_desc_ArmorRepairKit", "repairPercent", armorKit_percent);
        Localizer.AddPlaceholder("item_desc_ArmorRepairKit", "aditionalPercentsByKitQuality",
            aditionalPercentsByKitQuality);
    }
}


//TODO: make JF_ItemsRepairKit bigger