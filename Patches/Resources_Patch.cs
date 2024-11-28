using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FiveHeartsTranslation.Patches
{
    [HarmonyPatch(typeof(Resources))]
    public static class Resources_Patch
    {
        [HarmonyPatch(nameof(Resources.Load), typeof(string), typeof(Type))]
        [HarmonyPostfix]
        public static void Load_Postfix(string path, Type systemTypeInstance, ref Object __result)
        {
            if (systemTypeInstance == typeof(TextAsset))
            {
                Object textAsset = AssetsLoader.LoadPatchedAsset(path, systemTypeInstance);
                if (textAsset != null)
                    __result = textAsset;
            }
        }
    }
}
