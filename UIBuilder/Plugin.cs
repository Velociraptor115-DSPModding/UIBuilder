using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DysonSphereProgram.Modding.UI.Builder
{
  public static class UIBuilderPlugin
  {
    private static Harmony _harmony;
    public static ManualLogSource Log;

    private static string GUID;

    public static void Create(string requestingModGUID, System.Action onReadyCallback)
    {
      GUID = requestingModGUID + "-UIBuilder";
      Log = Logger.CreateLogSource(GUID);
      _harmony = new Harmony(GUID);
      _harmony.PatchAll(typeof(UIBuilder));
      UIBuilder.QueueReadyCallback(onReadyCallback);
      if (UIRoot.instance?.uiGame?.created ?? false)
        UIBuilder.Create();
      Log.LogInfo(GUID + " Create() called");
    }

    public static void Destroy()
    {
      UIBuilder.Destroy();
      Log.LogInfo(GUID + " Destroy() called");
      _harmony?.UnpatchSelf();
      Log = null;
    }
  }
}

namespace System.Runtime.CompilerServices
{
  public record IsExternalInit;
}