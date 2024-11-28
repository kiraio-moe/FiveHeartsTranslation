using MelonLoader;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FiveHeartsTranslation
{
    internal class AssetsLoader : MelonMod
    {
        private static readonly string MODS_PATH = Path.Combine("Mods", "FiveHeartsTranslation");
        private static readonly string DUMP_PATH = Path.Combine(MODS_PATH, "Dumps");
        private static readonly string PATCH_PATH = Path.Combine(MODS_PATH, "Patches");

        public static async Task DumpTextAssets()
        {
            try
            {
                string textAssetOutput = Path.Combine(DUMP_PATH, nameof(TextAsset));
                if (!Directory.Exists(textAssetOutput))
                    Directory.CreateDirectory(textAssetOutput);

                TextAsset[] assets = Resources.LoadAll<TextAsset>("/");

                if (assets == null || assets.Length == 0)
                {
                    MelonLogger.Msg("No TextAssets found!");
                    return;
                }

                MelonLogger.Msg($"Found {assets.Length} TextAssets.");
                MelonLogger.Msg($"Dump to {Path.GetFullPath(textAssetOutput)}...");

                foreach (TextAsset asset in assets)
                {
                    if (asset == null || string.IsNullOrEmpty(asset.name))
                    {
                        MelonLogger.Warning("Empty TextAsset. Skipped.");
                        continue;
                    }

                    string assetName = asset.name;
                    string assetContent = asset.text;
                    string filePath = Path.Combine(textAssetOutput, assetName);

                    try
                    {
                        object parsedJson = JsonConvert.DeserializeObject(assetContent);
                        assetContent = JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
                        filePath = Path.Combine(textAssetOutput, $"{assetName}.json");
                        MelonLogger.Msg($"Dumping {assetName} as JSON.");
                    }
                    catch
                    {
                        MelonLogger.Warning(
                            $"Unable to parse \"{assetName}\" as JSON. Dumped as TXT."
                        );
                        filePath = Path.Combine(textAssetOutput, $"{assetName}.txt");
                    }

                    await File.WriteAllTextAsync(filePath, assetContent);
                }

                MelonLogger.Msg("Dump completed!");
            }
            catch (Exception ex)
            {
                MelonLogger.Msg($"Error dumping TextAssets: {ex.Message}\n{ex.StackTrace}");
            }
        }

        public static Object LoadPatchedAsset(string originalAsset, Type assetType)
        {
            try
            {
                if (assetType == typeof(TextAsset))
                {
                    string assetPath = Path.Combine(
                        PATCH_PATH,
                        nameof(TextAsset),
                        Path.GetFileName(originalAsset)
                    );

                    if (File.Exists($"{assetPath}.json"))
                        assetPath = $"{assetPath}.json";
                    else if (File.Exists($"{assetPath}.txt"))
                        assetPath = $"{assetPath}.txt";
                    else
                        return null;

                    MelonLogger.Msg($"Replacing {originalAsset} with {assetPath}.");
                    string assetText = File.ReadAllText(assetPath);
                    return new TextAsset(assetText);
                }

                return null;
            }
            catch (Exception ex)
            {
                MelonLogger.Error(
                    $"Error loading patched asset {originalAsset}: {ex.Message}\n{ex.StackTrace}"
                );
                return null;
            }
        }
    }
}
