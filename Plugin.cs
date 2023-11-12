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
    

    private void Awake()
    {
        CreateMod(this, ModName, ModAuthor, ModVersion, ModGUID);

        itemKit_percent = config("General", "Items kit repair percent", 25f,
            "How much percent of durability is repaired for items");
        armorKit_percent = config("General", "Armor kit repair percent", 25f,
            "How much percent of durability is repaired for armor");

        LoadAssetBundle("repairkit");
        var itemsRepairKit = new Item(bundle, "JF_ItemsRepairKit");
        itemsRepairKit.Name
            .Russian("Ремкомплект")
            .English("Repair kit");
        itemsRepairKit.Crafting.Add(CraftingTable.Workbench, 2);
        itemsRepairKit.RequiredItems.Add("FineWood", 5);
        itemsRepairKit.RequiredItems.Add("Ruby", 1);
        itemsRepairKit.CraftAmount = 2;

        bundle?.Unload(false);

        Localizer.Load();
        Localizer.AddPlaceholder("item_desc_ItemsRepairKit", "repairPercent", itemKit_percent);
    }
}


//TODO: make JF_ItemsRepairKit bigger