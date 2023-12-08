using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static IngamePlayerSettings;
using Object = UnityEngine.Object;

namespace LethalRebinding.Utilities
{
    internal static class SettingsUtilities
    {
        internal static GameObject GetSettingForInputAction(InputAction action, SettingsOption settingTemplate, Transform parent, int index = 0)
        {
            var newSettingTransform = Object.Instantiate(settingTemplate.transform.parent.gameObject, parent, false);

            // newSettingTransform.transform.localPosition = settingTemplate.GetComponent<Transform>().parent.localPosition + new Vector3(250, -(index * 20), 0);
            var newSetting = newSettingTransform.GetComponentInChildren<SettingsOption>();
            var newSettingText = newSettingTransform.GetComponentInChildren<TextMeshProUGUI>();

            var actionReference = InputActionReference.Create(action);
            newSetting.rebindableAction = actionReference;
            newSettingText.SetText(action.name + ":");

            newSettingTransform.transform.localPosition = new Vector3(225, -(index * 20), 0);

            return newSettingTransform;
        }

        // horrible method. creating scrollrects through code is terrible
        internal static GameObject CreateScrollRect(GameObject template, Image scrollBackgroundImage)
        {
            var copiedText = Object.Instantiate(template.transform.GetChild(0), template.transform, false);
            copiedText.transform.localPosition = new Vector3(-152.7989f, 36.4002f, 0);
            copiedText.GetComponent<TextMeshProUGUI>().SetText("BINDINGS");

            var scrollRectObject = new GameObject("Scroll View");
            var scrollRect = scrollRectObject.AddComponent<ScrollRect>();
            scrollRectObject.transform.SetParent(template.transform, false);
            scrollRectObject.transform.localPosition = new Vector3(-113, -50, 0);
            // ApplyParentSize(scrollRectObject, template.transform.parent);
            scrollRectObject.GetComponent<RectTransform>().sizeDelta = new Vector2(245, 150);
            var viewportObject = new GameObject("Viewport");
            ApplyParentSize(viewportObject, scrollRectObject.transform);
            viewportObject.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            var viewportImage = viewportObject.AddComponent<Image>();
            viewportObject.AddComponent<Mask>();
            // retain masking functionality with the lowest possible brightness image, don't wanna do this properly rn
            viewportImage.color = new Color(0f, 0f, 0f, 0.002f);

            var contentObject = new GameObject("Content");
            ApplyParentSize(contentObject, viewportObject.transform);
            var contentRect = contentObject.GetComponent<RectTransform>();
            contentRect.pivot = new Vector2(0, 1);
            contentRect.transform.localPosition = new Vector3(14, 0, 0);

            var contentFitter = contentObject.AddComponent<ContentSizeFitter>();
            contentFitter.verticalFit = ContentSizeFitter.FitMode.MinSize;
            var contentVerticalLayout = contentObject.AddComponent<VerticalLayoutGroup>();
            contentVerticalLayout.childControlHeight = false;
            contentVerticalLayout.childControlWidth = false;

            var scrollBar = new GameObject("Scrollbar Vertical");
            var scrollBarComponent = scrollBar.AddComponent<Scrollbar>();
            var scrollBarImage = scrollBar.AddComponent<Image>();
            scrollBarImage.sprite = scrollBackgroundImage.sprite;
            scrollBarImage.material = scrollBackgroundImage.material;

            ApplyParentSize(scrollBar, scrollRect.transform);
            var scrollBarRect = scrollBar.GetComponent<RectTransform>();

            scrollBarRect.pivot = new Vector2(1, 1);
            scrollBarRect.sizeDelta = new Vector2(10, 0);
            scrollBarRect.anchorMin = new Vector2(1, 0);
            scrollBarRect.anchorMax = new Vector2(1, 1);

            var slidingAreaObject = new GameObject("Sliding area");
            var slidingAreaRect = ApplyParentSize(slidingAreaObject, scrollBar.transform);
            slidingAreaRect.offsetMin = new Vector2(10, 10);
            slidingAreaRect.offsetMax = new Vector2(-10, -10);

            var handleObject = new GameObject("Handle");
            var handleObjectRect = ApplyParentSize(handleObject, slidingAreaObject.transform);
            handleObjectRect.offsetMin = new Vector2(-10, -10);
            handleObjectRect.offsetMax = new Vector2(10, 10);
            var handleImage = handleObject.AddComponent<Image>();

            scrollBarComponent.handleRect = handleObjectRect;
            scrollBarComponent.targetGraphic = handleImage;
            scrollBarComponent.direction = Scrollbar.Direction.BottomToTop;

            scrollRect.content = contentRect;
            scrollRect.viewport = viewportObject.GetComponent<RectTransform>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.verticalScrollbar = scrollBarComponent;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHideAndExpandViewport;

            return contentObject;
        }

        private static RectTransform ApplyParentSize(GameObject uiElement, Transform parent)
        {
            var rect = uiElement.GetComponent<RectTransform>();
            if (rect == null) rect = uiElement.AddComponent<RectTransform>();

            rect.SetParent(parent);

            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = rect.offsetMax = Vector2.zero;

            rect.localRotation = Quaternion.identity;
            rect.localScale = Vector3.one;
            rect.localPosition = Vector3.zero;

            return rect;
        }
    }
}
