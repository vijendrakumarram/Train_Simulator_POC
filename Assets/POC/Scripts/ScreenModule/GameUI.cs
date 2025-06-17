using ScreenUtils.Manager;

namespace ScreenUtils
{
    //used for popups
    public class GameUI : View
    {        
        public bool hasBlockerUI;
        public bool hasAudio;
        public bool isOverlay;

        public PlaySound playSound = PlaySound.SCENE;

        protected virtual void OnDisable()
        {
            if (ScreenManager.Instance != null)
            {
                ScreenManager.ShowBlocker(false);
            }
        }
    }
}