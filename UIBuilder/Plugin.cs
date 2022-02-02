using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DysonSphereProgram.Modding.UI
{
  [BepInPlugin(GUID, NAME, VERSION)]
  [BepInProcess("DSPGAME.exe")]
  public class Plugin : BaseUnityPlugin
  {
    public const string GUID = "dev.raptor.dsp.UIBuilder";
    public const string NAME = "UIBuilder";
    public const string VERSION = "0.0.1";

    private Harmony _harmony;
    public static ManualLogSource Log;

    private void Awake()
    {
      Plugin.Log = Logger;
      _harmony = new Harmony(GUID);
      UIBuilder.QueueReadyCallback(TestUIBuilder.Create);
      //UIBuilder.QueueReadyCallback(TestAddingResizeSupport.Create);
      _harmony.PatchAll(typeof(UIBuilder));
      if (UIRoot.instance?.uiGame?.created ?? false)
        UIBuilder.Create();
      Logger.LogInfo("UIBuilder Awake() called");
    }

    private void OnDestroy()
    {
      TestUIBuilder.Destroy();
      //TestAddingResizeSupport.Destroy();
      UIBuilder.Destroy();
      Logger.LogInfo("UIBuilder OnDestroy() called");
      _harmony?.UnpatchSelf();
      Plugin.Log = null;
    }
  }
}

namespace System.Runtime.CompilerServices
{
  public record IsExternalInit;
}