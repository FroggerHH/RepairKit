// ReSharper disable VariableHidesOuterVariable

namespace RepairKit;

[HarmonyPatch]
public static class TerminalCommands
{
    [HarmonyPatch(typeof(Terminal), nameof(Terminal.InitTerminal))] [HarmonyPostfix]
    private static void AddCommands()
    {
        new Terminal.ConsoleCommand("brokeAllStaff",
            "", args => RunCommand(args =>
            {
                if (!IsAdmin) throw new ConsoleCommandException("You are not an admin on this server");
                foreach (var itemData in m_localPlayer.GetInventory().GetAllItems())
                {
                    var maxDurability = itemData.GetMaxDurability();
                    itemData.m_durability =
                        Mathf.Clamp(itemData.m_durability - (0.25f * maxDurability), 0, maxDurability);
                }

                args.Context.AddString("Done");
            }, args), true);
    }
}