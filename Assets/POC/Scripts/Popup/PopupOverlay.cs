// Copyright (C) 2015-2021 gamevanilla - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UltimateClean
{
    /// <summary>
    // This class is responsible for popup management. Popups follow the traditional behavior of
    // automatically blocking the input on elements behind it and adding a background texture.
    /// </summary>
    public class PopupOverlay : MonoBehaviour
    {
        public Color backgroundColor = new Color(10.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f, 0.9f);

        public float destroyTime = 0.5f;

        protected GameObject m_background;

        private void OnDisable()
        {
            Destroy(m_background);
            Debug.Log("Popup Disabled");
            //Destroy(gameObject);
        }
        public virtual void OnEnable()
        {
            AddBackground();
        }
        public void Open()
        {
            
        }

        public void Close()
        {
            var animator = GetComponent<Animator>();
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Open"))
            {
                animator.Play("Close");
            }

            RemoveBackground();
            StartCoroutine(RunPopupDestroy());
        }

        // We destroy the popup automatically 0.5 seconds after closing it.
        // The destruction is performed asynchronously via a coroutine. If you
        // want to destroy the popup at the exact time its closing animation is
        // finished, you can use an animation event instead.
        private IEnumerator RunPopupDestroy()
        {
            yield return new WaitForSeconds(destroyTime);
            Destroy(m_background);
            //Destroy(gameObject);
        }

        public virtual void AddBackground()
        {
            var bgTex = new Texture2D(1, 1);
            bgTex.SetPixel(0, 0, backgroundColor);
            bgTex.Apply();
            if (m_background != null)
                return;
            //Constants.isPopupEnabled = true;
            m_background = new GameObject("PopupBackground");
            m_background.AddComponent<PopupBGHandler>();
            var image = m_background.AddComponent<Image>();
            var rect = new Rect(0, 0, bgTex.width, bgTex.height);
            var sprite = Sprite.Create(bgTex, rect, new Vector2(0.5f, 0.5f), 1);
            image.material.mainTexture = bgTex;
            image.sprite = sprite;
            var newColor = image.color;
            image.color = newColor;
            image.canvasRenderer.SetAlpha(0.0f);
            //image.CrossFadeAlpha(1.0f, 0.4f, false);
            image.CrossFadeAlpha(1.0f, 0, false);

            var canvas = GetComponentInParent<Canvas>();// GameObject.Find("Canvas");
            m_background.transform.localScale = new Vector3(1, 1, 1);
            m_background.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
            m_background.transform.SetParent(transform.parent, false);
            //m_background.transform.SetParent(canvas.transform, false);
            m_background.transform.SetSiblingIndex(transform.GetSiblingIndex());
            m_background.layer = LayerMask.NameToLayer("RayBlockerUI");
        }

        private void RemoveBackground()
        {
            var image = m_background.GetComponent<Image>();
            if (image != null)
            {
                //image.CrossFadeAlpha(0.0f, 0.2f, false);
                image.CrossFadeAlpha(0.0f, 0, false);
            }
            //Constants.isPopupEnabled = false;
            Destroy(m_background);
            Debug.Log("Popup Disabled");
        }
    }
}
