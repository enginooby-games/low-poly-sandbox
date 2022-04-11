#if UNITY_EDITOR
using UnityEditor.UIElements;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Enginooby.Utils {
  public static class UIElementsUtils {
    public static VisualElement AddButton(this VisualElement container, string label, StyleSheet styleSheet = null) {
      var button = new Button {text = label};
      return container.AddVisualElement(button, styleSheet);
    }

#if UNITY_EDITOR
    public static VisualElement AddColorField(this VisualElement container, StyleSheet styleSheet = null) {
      var colorField = new ColorField();
      return container.AddVisualElement(colorField, styleSheet);
    }
#endif

    public static VisualElement AddLabel(this VisualElement container, string text, StyleSheet styleSheet = null) {
      var label = new Label(text);
      return container.AddVisualElement(label, styleSheet);
    }

    /// <summary>
    /// Apply stylesheet to child element and add it to the container element.
    /// <returns>Child element.</returns>
    /// </summary>
    public static VisualElement AddVisualElement(
      this VisualElement container,
      VisualElement child,
      StyleSheet styleSheet = null) {
      if (styleSheet != null) child.styleSheets.Add(styleSheet);
      container.Add(child);
      return child;
    }

    public static float GetCheckedTogglePercentage(this IEnumerable<Toggle> toggles) {
      var toggleArray = toggles as Toggle[] ?? toggles.ToArray();
      if (!toggleArray.Any()) return 0;
      var checkedToggle = toggleArray.Count(toggle => toggle.value);
      return checkedToggle * 100f / toggleArray.Count();
    }

    public static IEnumerable<T> Children<T>(this VisualElement visualElement) where T : VisualElement =>
      visualElement.Children().Cast<T>();

    // TODO: generalize key event type
    public static void OnKeyDown(
      this CallbackEventHandler handler,
      KeyCode keyCode,
      Action action) {
      handler.RegisterCallback<KeyDownEvent>(e => {
        if (e.keyCode == keyCode) action.Invoke();
      });
    }
  }
}