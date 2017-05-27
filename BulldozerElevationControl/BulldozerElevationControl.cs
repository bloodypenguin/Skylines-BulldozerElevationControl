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
        protected UIMultiStateButton m_BulldozerUndergroundToggle;
        public LoadMode loadMode;


        public void Awake()
        {
            this.m_buildElevationUp = new SavedInputKey(Settings.buildElevationUp, Settings.gameSettingsFile, DefaultSettings.buildElevationUp, true);
            this.m_buildElevationDown = new SavedInputKey(Settings.buildElevationDown, Settings.gameSettingsFile, DefaultSettings.buildElevationDown, true);
            this.m_BulldozerUndergroundToggle = UIView.Find<UIMultiStateButton>("BulldozerUndergroundToggle");
            UIInput.eventProcessKeyEvent += this.ProcessKeyEvent;
        }

        private void ProcessKeyEvent(EventType eventType, KeyCode keyCode, EventModifiers modifiers)
        {
            if (ToolsModifierControl.GetCurrentTool<BulldozeTool>() == null)
            {
                return;
            }
            if (eventType != EventType.KeyDown)
                return;
            if (this.m_buildElevationUp.IsPressed(eventType, keyCode, modifiers))
            {
                m_BulldozerUndergroundToggle.activeStateIndex = 0;
            }
            else if (this.m_buildElevationDown.IsPressed(eventType, keyCode, modifiers))
            {
                m_BulldozerUndergroundToggle.activeStateIndex = 1;
            }
        }

    }
}