using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(
    typeof(FiveHeartsTranslation.Core),
    "Five Hearts Translation",
    "1.0.0",
    "kiraio",
    "https://github.com/kiraio-moe/FiveHeartsTranslation/releases"
)]
[assembly: MelonGame("Storytaco", "MilkGame")]
[assembly: MelonColor(1, 48, 133, 213)]
[assembly: MelonAuthorColor(1, 255, 42, 122)]

namespace FiveHeartsTranslation
{
    public class Core : MelonMod
    {
        private readonly KeyCode _showGUIKey = KeyCode.Home;
        private bool _showGUI;

        public override async void OnGUI()
        {
            base.OnGUI();

            if (!_showGUI)
                return;

            if (GUI.Button(new Rect(10, 10, 120, 60), "Dump Game Text"))
            {
                await AssetsLoader.DumpTextAssets();
            }
        }

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
            HarmonyInstance.PatchAll();
        }

        public override void OnLateUpdate()
        {
            base.OnLateUpdate();

            if (Input.GetKeyDown(_showGUIKey))
                _showGUI = !_showGUI;
        }
    }
}
