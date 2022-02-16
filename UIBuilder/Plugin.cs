using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DysonSphereProgram.Modding.UI.Builder
{
  [BepInPlugin(GUID, NAME, VERSION)]
  [BepInProcess("DSPGAME.exe")]
  public class UIBuilderPlugin : BaseUnityPlugin
  {
    public const string GUID = "dev.raptor.dsp.UIBuilder";
    public const string NAME = "UIBuilder";
    public const string VERSION = "0.0.1";

    private Harmony _harmony;
    public static ManualLogSource Log;

    private void Awake()
    {
      UIBuilderPlugin.Log = Logger;
      _harmony = new Harmony(GUID);
      _harmony.PatchAll(typeof(UIBuilder));
      if (UIRoot.instance?.uiGame?.created ?? false)
        UIBuilder.Create();
      Logger.LogInfo("UIBuilder Awake() called");
    }

    private void OnDestroy()
    {
      UIBuilder.Destroy();
      Logger.LogInfo("UIBuilder OnDestroy() called");
      _harmony?.UnpatchSelf();
      UIBuilderPlugin.Log = null;
    }
  }
}

namespace System.Runtime.CompilerServices
{
  public record IsExternalInit;
}