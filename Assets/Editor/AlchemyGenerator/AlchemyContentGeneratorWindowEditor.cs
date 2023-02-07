using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public static class VisualSetting
{
    public const float titleFontSize = 20f;
    public const float headerFontSize = 15f;
    public const float subheaderFontSize = 13f;
    public const float normalFontSize = 12f;
    public const float longWidth = 375f;
    public const float mediumWidth = 200f;
    public const float shortWidth = 75f;

    public static Color borderColor = Color.black;
    public const float borderWidth = 3.5f;
    public const float margin = 10f;
    public const float containerWidth = longWidth + 50f;

    public static void SetContainerVisuals(VisualElement visElement)
    {
        visElement.style.borderBottomColor = borderColor;
        visElement.style.borderLeftColor = borderColor;
        visElement.style.borderRightColor = borderColor;
        visElement.style.borderTopColor = borderColor;

        visElement.style.width = containerWidth;

        visElement.style.borderBottomWidth = borderWidth;
        visElement.style.borderTopWidth = borderWidth;
        visElement.style.borderLeftWidth = borderWidth;
        visElement.style.borderRightWidth = borderWidth;

        visElement.style.marginRight = margin;
        visElement.style.marginLeft = margin;
        visElement.style.marginTop = margin;
        visElement.style.marginBottom = margin*2;
    }

    public static ScrollView CreateScrollView(string labelText, FloatField min, FloatField max, float labelWidth = 100f)
    {
        ScrollView scrollView = new ScrollView(ScrollViewMode.Horizontal);
        scrollView.Add(CreateLabel(labelText, VisualSetting.normalFontSize, labelWidth));
        scrollView.Add(min);
        scrollView.Add(max);
        return scrollView;
    }

    public static Label CreateHeader(string text, float fontSize)
    {
        Label label = new Label("<b>" + text + "</b>");
        label.style.fontSize = fontSize;
        return label;
    }

    public static Label CreateLabel(string text, float fontSize, float width = 100f)
    {
        Label label = new Label(text);
        label.style.fontSize = fontSize;
        TextAnchor anchor = TextAnchor.MiddleLeft;
        label.style.unityTextAlign = anchor;
        label.style.width = width;
        return label;
    }

    public static Slider CreateProbabilitySlider(string labelText)
    {
        Slider slider = new Slider(0f, 1f);
        slider.label = labelText;
        slider.showInputField = true;
        slider.style.width = longWidth;
        return slider;
    }
    public static FloatField CreateFloatField(string labelText, float width = VisualSetting.longWidth)
    {
        FloatField field = new FloatField(labelText);
        field.style.width = width;
        return field;
    }

    public static IntegerField CreateIntegerField(string labelText, float width = VisualSetting.longWidth)
    {
        IntegerField field = new IntegerField(labelText);
        field.style.width = width;
        return field;
    }

    public static Button CreateButton(string labelText, System.Action clickEvent, float width = VisualSetting.longWidth)
    {
        Button button = new Button(clickEvent);
        button.text = labelText;
        button.style.width = width;
        return button;
    }
}
public class AlchemyContentGeneratorWindowEditor : EditorWindow
{
    IngredientGeneratorWindowEditor ingredientEditor;
    PotionRecipesGeneratorWindowEditor recipeEditor;
    VisualElement ingredientWindow;
    VisualElement recipesWindow;

    Button ingredientTabBtn;
    Button recipesTabBtn;

    static int initState;

    public static void ShowWindow(int state = -1)
    {
        AlchemyContentGeneratorWindowEditor editorWindow = GetWindow<AlchemyContentGeneratorWindowEditor>();
        editorWindow.titleContent = new GUIContent("Alchemy Ingredients Generator");
        editorWindow.minSize = new Vector2(Mathf.Min(1305, Screen.width), Mathf.Min(800, Screen.height));
        editorWindow.maximized = true;
        initState = state;
    }

    private void CreateGUI()
    {
        VisualElement tabMenu = new VisualElement();
        ingredientEditor = CreateInstance<IngredientGeneratorWindowEditor>();
        VisualElement content = new VisualElement();
        ingredientWindow = ingredientEditor.CreateGUI();

        tabMenu.style.flexDirection = FlexDirection.Row;
        ingredientTabBtn = new Button(() => ShowIngredientEditor(content));
        ingredientTabBtn.text = "Ingredient Generator";
        ingredientTabBtn.style.width = VisualSetting.mediumWidth;

        
        recipeEditor = CreateInstance<PotionRecipesGeneratorWindowEditor>();
        recipesWindow = recipeEditor.CreateGUI();
        recipesTabBtn = new Button(() => ShowRecipesEditor(content));
        recipesTabBtn.text = "Recipes Generator";
        recipesTabBtn.style.width = VisualSetting.mediumWidth;

        tabMenu.Add(ingredientTabBtn);
        tabMenu.Add(recipesTabBtn);

        if (initState == 0)
            ShowIngredientEditor(content);
        else if (initState == 1)
            ShowRecipesEditor(content);
        rootVisualElement.Add(tabMenu);
        rootVisualElement.Add(content);
    }

    private void ShowIngredientEditor(VisualElement parent)
    {
        if (parent.Contains(ingredientWindow))
            return;
        parent.Add(ingredientWindow);
        recipesTabBtn.style.backgroundColor = ingredientTabBtn.style.backgroundColor;
        ingredientTabBtn.style.backgroundColor = VisualSetting.borderColor;
        if (parent.Contains(recipesWindow))
            parent.Remove(recipesWindow);
    }

    private void ShowRecipesEditor(VisualElement parent)
    {
        if (parent.Contains(recipesWindow))
            return;
        if (parent.Contains(ingredientWindow))
            parent.Remove(ingredientWindow);
        ingredientTabBtn.style.backgroundColor = recipesTabBtn.style.backgroundColor;
        recipesTabBtn.style.backgroundColor = VisualSetting.borderColor;
        parent.Add(recipesWindow);
    }
}
