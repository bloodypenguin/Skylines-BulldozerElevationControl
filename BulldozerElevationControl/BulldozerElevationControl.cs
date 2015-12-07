using System.Reflection;
using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace BulldozerElevationControl
{
    public class BulldozerElevationControl : MonoBehaviour
    {
        private SavedInputKey m_buildElevationUp;
        private SavedInputKey m_buildElevationDown;
        private InfoManager.InfoMode previousGroundMode = InfoManager.InfoMode.None;
        private InfoManager.InfoMode previousUndergroundMode = InfoManager.InfoMode.None;

        public LoadMode loadMode;


        public void Awake()
        {
            this.m_buildElevationUp = new SavedInputKey(Settings.buildElevationUp, Settings.gameSettingsFile, DefaultSettings.buildElevationUp, true);
            this.m_buildElevationDown = new SavedInputKey(Settings.buildElevationDown, Settings.gameSettingsFile, DefaultSettings.buildElevationDown, true);
            UIInput.eventProcessKeyEvent += new UIInput.ProcessKeyEventHandler(this.ProcessKeyEvent);
        }

        private void ProcessKeyEvent(EventType eventType, KeyCode keyCode, EventModifiers modifiers)
        {
            if (ToolsModifierControl.GetCurrentTool<BulldozeTool>() == null)
            {
                previousGroundMode = InfoManager.InfoMode.None;
                previousUndergroundMode = InfoManager.InfoMode.None;
                return;
            }
            if (eventType != EventType.KeyDown)
                return;
            var mode = Singleton<InfoManager>.instance.CurrentMode;
            var subMode = Singleton<InfoManager>.instance.CurrentSubMode;

            if (this.m_buildElevationUp.IsPressed(eventType, keyCode, modifiers))
            {
                if ((mode == InfoManager.InfoMode.Transport || mode == InfoManager.InfoMode.Traffic) && subMode == InfoManager.SubInfoMode.Default)
                {
                    previousUndergroundMode = mode;
                    ForceInfoMode(previousGroundMode, InfoManager.SubInfoMode.Default);
                }
            }
            else if (this.m_buildElevationDown.IsPressed(eventType, keyCode, modifiers))
            {
                if (mode != InfoManager.InfoMode.Transport && mode != InfoManager.InfoMode.Traffic 
                    && (mode != InfoManager.InfoMode.Water || (loadMode != LoadMode.NewGame && loadMode != LoadMode.LoadGame)))
                {
                    if (previousUndergroundMode == InfoManager.InfoMode.None)
                    {
                        previousUndergroundMode = InfoManager.InfoMode.Traffic;
                    }
                    previousGroundMode = mode;
                    ForceInfoMode(previousUndergroundMode, InfoManager.SubInfoMode.Default);
                }
            }
        }

        protected void ForceInfoMode(InfoManager.InfoMode mode, InfoManager.SubInfoMode subMode)
        {
            var bulldozeTool = ToolsModifierControl.GetCurrentTool<BulldozeTool>();
            if (bulldozeTool == null)
            {
                return;
            }
//TODO(earalov): restore if needed
//            if (loadMode == LoadMode.LoadGame || loadMode == LoadMode.NewGame)
//            {
//                var mainToolbar = ToolsModifierControl.mainToolbar;
//                if (mainToolbar != null)
//                {
//                    if (typeof(MainToolbar).GetField("m_LastTool", BindingFlags.Instance | BindingFlags.NonPublic)
//                            .GetValue(mainToolbar) is DefaultTool)
//                    {
//                        typeof(MainToolbar).GetField("m_LastInfoMode", BindingFlags.Instance | BindingFlags.NonPublic)
//                            .SetValue(mainToolbar, mode);
//                        typeof(MainToolbar).GetField("m_LastSubInfoMode", BindingFlags.Instance | BindingFlags.NonPublic)
//                            .SetValue(mainToolbar, subMode);
//                    }
//                }  
//            }
            typeof(ToolBase).GetField("m_forcedInfoMode", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(bulldozeTool, mode);
            Singleton<InfoManager>.instance.SetCurrentMode(mode, subMode);
        }

    }
}