using ICities;
using UnityEngine;

namespace BulldozerElevationControl
{

    public class Mod : LoadingExtensionBase, IUserMod
    {

        public const string GAME_OBJECT_NAME = "BulldozerElevationControl";

        public string Name
        {
            get { return "Bulldozer Elevation Control"; }
        }

        public string Description
        {
            get { return "Provides hotkeys for toggling between ground and underground modes of bulldozer"; }
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            if (GameObject.Find(GAME_OBJECT_NAME) != null)
            {
                return;
            }

            var gameObject = new GameObject(GAME_OBJECT_NAME);
            var elevationControl = gameObject.AddComponent<BulldozerElevationControl>();
            elevationControl.loadMode = mode;
        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
            var gameObject = GameObject.Find(GAME_OBJECT_NAME);
            if (gameObject == null)
            {
                return;
            }
            Object.Destroy(gameObject);
        }
    }
}
