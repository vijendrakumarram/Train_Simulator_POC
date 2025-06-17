using System.Collections.Generic;
using UnityEngine;
namespace ScreenUtils.Manager
{
    public class ScreenManager : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> referenceInScreen;
        [SerializeField] protected GameObject depthCameraCanvasBlocker;
        [SerializeField] protected GameScreen defaultScreen;

        public static ScreenManager Instance;

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject); //destroy new gameobject
            }
            else
            {
                Instance = this;
            }
        }

        protected virtual void Start()
        {
            GameScreen.currentScreen = defaultScreen;
        }

        public static void ShowBackScreen()
        {
            ShowScreen(GameScreen.currentScreen.backScreen);
        }

        public static void HideScreen(string name)
        {
            GameObject screen = Instance.referenceInScreen.Find(x => x.name.Contains(name));
            screen.SetActive(false);
        }
        
        public static void HideScreen(ScreenUtils.Screen currentScreen)
        {
            GameObject screen = Instance.referenceInScreen.Find(x => x.name.Contains(currentScreen.ToString()));
            screen.SetActive(false);
        }

        public static void ShowBlocker(bool on)
        {
            if (Instance.depthCameraCanvasBlocker != null)
            {
                Instance.depthCameraCanvasBlocker.SetActive(on);
            }
        }

        public static void ShowScreen(ScreenUtils.Screen screen)
        {
            //Debug.Log("show screen : " + SceneController.CurrentScene.ToString());
           
            if (screen.ToString() == "Exit")
            {
                Application.Quit();
                return;
            }

            GameObject screenObj = Instance.GetScreen(screen.ToString());
            GameUI gameScreen = screenObj.GetComponent<GameUI>();
            bool isPopup = gameScreen.isOverlay;

            switch (gameScreen.playSound)
            {
                case PlaySound.NONE:
                    //AudioManager.StopMusic();
                    break;
                
                case PlaySound.SELF:
                   // AudioManager.PlayMusic(screen.ToString());
                    break;
            }
            //hide last screen
            if (GameScreen.currentScreen != null)
            {
                if (!isPopup)
                {
                    GameScreen.currentScreen.gameObject.SetActive(false);
                    GameScreen.currentScreen = (GameScreen)gameScreen;
                }
            }

            ShowBlocker(gameScreen.hasBlockerUI);
            screenObj.SetActive(true);
        }

        private GameObject GetScreen(string screenName)
        {
            GameObject screenObj = referenceInScreen.Find(x => x.name.Contains(screenName));
            return screenObj;
        }
    }

    public enum PlaySound { NONE, SELF, SCENE}

}

 