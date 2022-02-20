using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection.Emit;
using UnityEngine;
using DysonSphereProgram.Modding.UI.Builder;

namespace DysonSphereProgram.Modding.Raptor.TestUIBuilder
{
  
  
  [BepInPlugin(GUID, NAME, VERSION)]
  [BepInProcess("DSPGAME.exe")]
  public class Plugin : BaseUnityPlugin
  {
    public const string GUID = "dev.raptor.dsp.TestUIBuilder";
    public const string NAME = "TestUIBuilder";
    public const string VERSION = "0.0.1";

    private Harmony _harmony;
    public static ManualLogSource Log;
    internal static string Path;

    private void Awake()
    {
      Plugin.Log = Logger;
      Plugin.Path = Info.Location;
      _harmony = new Harmony(GUID);
      UIBuilderPlugin.Create(GUID, UIBuilderTest.Create);
      Plugin.Log.LogInfo("TestUIBuilder Awake() called");
    }

    private void OnDestroy()
    {
      UIBuilderTest.Destroy();
      UIBuilderPlugin.Destroy();
      Plugin.Log.LogInfo("TestUIBuilder OnDestroy() called");
      _harmony?.UnpatchSelf();
      Plugin.Log = null;
    }
  }
}
