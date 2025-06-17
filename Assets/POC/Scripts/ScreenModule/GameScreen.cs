using ScreenUtils.Manager;

namespace ScreenUtils
{
    public class GameScreen : GameUI
    {
        public ScreenUtils.Screen backScreen;
        public ScreenUtils.Screen nextScreen;

        protected void ShowNextScreen()
        {
            ScreenManager.ShowScreen(nextScreen);
        }

        public static GameScreen currentScreen;
    }
}
