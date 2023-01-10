using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class RaritySettingsUI
{
    public List<Slider> effectAmountSliders = new List<Slider>();
    public FloatField minSum;
    public FloatField maxSum;

    public FloatField minPrimary;
    public FloatField maxPrimary;

}

public class ContentGeneratorEditorWindow : EditorWindow
{
    private const float headerFontSize = 15f;
    private const float subheaderFontSize = 13f;
    private const float normalFontSize = 12f;
    private const float longWidth = 375f;
    private const float shortWidth = 75f;

    private Slider rareProbabilitySlider;
    private Slider epicProbabilitySlider;

    private RaritySettingsUI commonSettings;
    private RaritySettingsUI rareSettings;
    private RaritySettingsUI epicSettings;

    private IntegerField amountToGenerateField;

    [SerializeField] private IngredientGeneratorConfiguration defaultConfig;


    public static void ShowWindow()
    {
        ContentGeneratorEditorWindow editorWindow = GetWindow<ContentGeneratorEditorWindow>();
        editorWindow.titleContent = new GUIContent("Alchemy Ingredients Generator");
        editorWindow.minSize = new Vector2(600, 800);
    }

    private void CreateGUI()
    {
        rootVisualElement.Add(CreateHeader("Rarity probabilities", headerFontSize));
        rareProbabilitySlider = CreateProbabilitySlider("Rare");
        rootVisualElement.Add(rareProbabilitySlider);
        epicProbabilitySlider = CreateProbabilitySlider("Epic");
        rootVisualElement.Add(epicProbabilitySlider);

        commonSettings = CreateRaritySettingsUI();
        rareSettings = CreateRaritySettingsUI();
        epicSettings = CreateRaritySettingsUI();

        rootVisualElement.Add(CreateHeader("Rarity settings", headerFontSize));
        ShowRaritySettingsUI("Common", commonSettings);
        ShowRaritySettingsUI("Rare", rareSettings);
        ShowRaritySettingsUI("Epic", epicSettings);


        rootVisualElement.Add(CreateHeader("Amount to generate", subheaderFontSize));
        amountToGenerateField = CreateIntegerField("");
        rootVisualElement.Add(amountToGenerateField);

        SetDefaultValues();

        Button button = new Button(() => ProceduralGenerationManager.Test());
        button.text = "Generate Ingredients";
        rootVisualElement.Add(button);
    }

    private IngredientGeneratorConfiguration CreateConfiguration()
    {
        IngredientGeneratorConfiguration config = new IngredientGeneratorConfiguration();
        RaritySettings common = CreateRaritySettings(commonSettings);
        RaritySettings rare = CreateRaritySettings(rareSettings);
        RaritySettings epic = CreateRaritySettings(epicSettings);

        int amount = amountToGenerateField.value;
        float rareProbability = rareProbabilitySlider.value;
        float epicProbability = epicProbabilitySlider.value;

        config.common = common;
        config.rare = rare;
        config.epic = epic;
        config.amountToGenerate = amount;
        config.rareProbability = rareProbability;
        config.epicProbability = epicProbability;

        return config;
    }

    private void SetDefaultValues()
    {
        SetRaritySettings(commonSettings, defaultConfig.common);
        SetRaritySettings(rareSettings, defaultConfig.rare);
        SetRaritySettings(epicSettings, defaultConfig.epic);

        amountToGenerateField.value = defaultConfig.amountToGenerate;
        rareProbabilitySlider.value = defaultConfig.rareProbability;
        epicProbabilitySlider.value = defaultConfig.epicProbability;
    }

    private void SetRaritySettings(RaritySettingsUI raritySettingsUI, RaritySettings settings)
    {
        for (int i = 0; i < raritySettingsUI.effectAmountSliders.Count; i++)
        {
            raritySettingsUI.effectAmountSliders[i].value = settings.GetProbability(i);
        }
        raritySettingsUI.minSum.value = settings.GetSumRange().Item1;
        raritySettingsUI.maxSum.value = settings.GetSumRange().Item2;
        raritySettingsUI.minPrimary.value = settings.GetPrimaryValueRange().Item1;
        raritySettingsUI.maxPrimary.value = settings.GetPrimaryValueRange().Item2;
    }

    private RaritySettings CreateRaritySettings(RaritySettingsUI raritySettingsUI)
    {
        List<float> amountProbabilities = new List<float>();
        for (int i = 0; i < raritySettingsUI.effectAmountSliders.Count; i++)
        {
            amountProbabilities.Add(raritySettingsUI.effectAmountSliders[i].value);
        }

        return new RaritySettings(amountProbabilities, raritySettingsUI.minSum.value, 
            raritySettingsUI.maxSum.value, raritySettingsUI.minPrimary.value, raritySettingsUI.maxPrimary.value);

    }

    private ScrollView CreateScrollView(string labelText, FloatField min, FloatField max)
    {
        ScrollView scrollView = new ScrollView(ScrollViewMode.Horizontal);
        scrollView.Add(new Label());
        scrollView.Add(CreateLabel(labelText, normalFontSize));
        scrollView.Add(min);
        scrollView.Add(max);
        return scrollView;
    }

    private RaritySettingsUI CreateRaritySettingsUI()
    {
        RaritySettingsUI raritySettings = new RaritySettingsUI();
        for (int i = 0; i < 3; i++)
        {
            Slider slider = CreateProbabilitySlider((i+1).ToString());
            raritySettings.effectAmountSliders.Add(slider);
        }

        raritySettings.minSum = CreateFloatField("", shortWidth);
        raritySettings.minSum.bindingPath = "minSumValue";
        raritySettings.maxSum = CreateFloatField("", shortWidth);
        raritySettings.minPrimary = CreateFloatField("", shortWidth);
        raritySettings.maxPrimary = CreateFloatField("", shortWidth);

        return raritySettings;
    }

    private void ShowRaritySettingsUI(string header, RaritySettingsUI settings)
    {
        rootVisualElement.Add(CreateHeader(header, subheaderFontSize));
        rootVisualElement.Add(CreateHeader("Effect Amount Probabilities", normalFontSize));
        foreach (Slider slider in settings.effectAmountSliders)
            rootVisualElement.Add(slider);
        ScrollView sumScroll = CreateScrollView("Sum range", settings.minSum, settings.maxSum);
        rootVisualElement.Add(sumScroll);
        ScrollView primaryScroll = CreateScrollView("Primary range", settings.minPrimary, settings.maxPrimary);
        rootVisualElement.Add(primaryScroll);
    }

    private Label CreateHeader(string text, float fontSize)
    {
        Label label = new Label("<b>" + text + "</b>");
        label.style.fontSize = fontSize;
        return label;
    }

    private Label CreateLabel(string text, float fontSize)
    {
        Label label = new Label(text);
        label.style.fontSize = fontSize;
        TextAnchor anchor = TextAnchor.MiddleLeft;
        label.style.unityTextAlign = anchor;
        label.style.width = 100f;
        return label;
    }

    private Slider CreateProbabilitySlider(string labelText)
    {
        Slider slider = new Slider(0f, 1f);
        slider.label = labelText;
        slider.showInputField = true;
        slider.style.width = 375;
        return slider;
    }

    private FloatField CreateFloatField(string labelText, float width = longWidth)
    {
        FloatField field = new FloatField(labelText);
        field.style.width = width;
        return field;
    }

    private IntegerField CreateIntegerField(string labelText, float width = longWidth)
    {
        IntegerField field = new IntegerField(labelText);
        field.style.width = width;
        return field;
    }
}
