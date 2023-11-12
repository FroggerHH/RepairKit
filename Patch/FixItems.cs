namespace RepairKit.Patch;

[HarmonyPatch]
public static class FixItems
{
    [HarmonyPatch(typeof(ZNetScene), nameof(ZNetScene.Awake))] [HarmonyPostfix] [HarmonyWrapSafe]
    private static void Patch()
    {
        //var soup = ZNetScene.instance.GetItem("CarrotSoup");
        //var soupParticles = soup.GetComponent<ParticleSystem>();
        var kits = ZNetScene.instance.GetPrefabs("JF_ItemsRepairKit");
        foreach (var kit in kits)
        {
            //kit.gameObject.GetOrAddComponent<ParticleSystem>();
            foreach (var renderer in kit.GetComponentsInChildren<Renderer>())
            {
                var material = renderer.material;
                material.shader = Shader.Find(material.shader.name);
            }
        }

        kits.First().transform.localScale *= 1.2f;
    }
}