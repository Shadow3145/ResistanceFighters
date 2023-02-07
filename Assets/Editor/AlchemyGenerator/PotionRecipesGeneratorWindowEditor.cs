using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


public class RecipeRaritySettingsUI
{
    public FloatField minAmount;
    public FloatField maxAmount;

    public List<Slider> effectAmountSliders = new List<Slider>();

    public FloatField minMainEffect;
    public FloatField maxMainEffect;

    public FloatField minSecondaryEffect;
    public FloatField maxSecondaryEffect;
}

public class PotionRecipesGeneratorWindowEditor : EditorWindow
{
    [SerializeField] private PotionRecipeGeneratorConfiguration defaultConfig;

    private RecipeRaritySettingsUI commonSettings;
    private RecipeRaritySettingsUI rareSettings;
    private RecipeRaritySettingsUI epicSettings;

    private FloatField minStrengthField;

    private TextField folderPathField;

    private TextField configPathField;
    private TextField configNameField;

    private PotionGenerator pcgManager;

    public VisualElement CreateGUI()
    {
        pcgManager = FindObjectOfType<PotionGenerator>();
        rootVisualElement.style.flexDirection = FlexDirection.Row;


        VisualElement settingsElem = new VisualElement();
        VisualSetting.SetContainerVisuals(settingsElem);
        ShowBasicSettings(settingsElem);
        rootVisualElement.Add(settingsElem);

        VisualElement configSettingsElem = new VisualElement();
        VisualSetting.SetContainerVisuals(configSettingsElem);
        ShowConfigSettings(configSettingsElem);
        rootVisualElement.Add(configSettingsElem);

        LoadValuesFromConfig();

        return rootVisualElement;
    }

    private void ShowBasicSettings(VisualElement parent)
    {
        parent.Add(VisualSetting.CreateHeader("Basic recipe generator settings",
            VisualSetting.titleFontSize));
        minStrengthField = VisualSetting.CreateFloatField("Minimal required strength");
        parent.Add(minStrengthField);

        parent.Add(VisualSetting.CreateHeader("Rarity settings", VisualSetting.headerFontSize));

        commonSettings = CreateRaritySettingsUI();
        rareSettings = CreateRaritySettingsUI();
        epicSettings = CreateRaritySettingsUI();

        ShowRaritySettings("Common", commonSettings, parent);
        ShowRaritySettings("Rare", rareSettings, parent);
        ShowRaritySettings("Epic", epicSettings, parent);

        parent.Add(VisualSetting.CreateHeader("Target folder path", VisualSetting.subheaderFontSize));
        folderPathField = new TextField();
        folderPathField.style.width = VisualSetting.longWidth;
        parent.Add(folderPathField);

        parent.Add(VisualSetting.CreateButton("Generate recipes", () => GenerateRecipes()));
        parent.Add(VisualSetting.CreateButton("Reset default values", () => LoadValuesFromConfig()));
        parent.Add(VisualSetting.CreateButton("Delete recipes from folder", () => pcgManager.Delete()));
    }

    private void ShowConfigSettings(VisualElement parent)
    {
        parent.Add(VisualSetting.CreateHeader("Configuration files", VisualSetting.titleFontSize));
        parent.Add(VisualSetting.CreateHeader("Folder", VisualSetting.subheaderFontSize));
        configPathField = new TextField();
        configPathField.style.width = VisualSetting.longWidth;
        parent.Add(configPathField);
        configPathField.value = "Assets/ScriptableObjects/PCG/RecipeConfigs/";

        parent.Add(VisualSetting.CreateHeader("Name", VisualSetting.subheaderFontSize));
        configNameField = new TextField();
        configNameField.style.width = VisualSetting.longWidth;
        parent.Add(configNameField);
        parent.Add(VisualSetting.CreateButton("Save config file", () => SaveConfigFile()));
        parent.Add(VisualSetting.CreateButton("Delete config file and its settings", () => DeleteConfigFile()));
        parent.Add(VisualSetting.CreateButton("Load values from config", () => LoadValuesFromConfig(configPathField.value + configNameField.value + ".asset")));
    }

    private PotionRecipeGeneratorConfiguration CreateConfiguration()
    {
        PotionRecipeGeneratorConfiguration config = CreateInstance<PotionRecipeGeneratorConfiguration>();
        
        PotionRaritySettings common = GetRaritySettings(commonSettings);
        PotionRaritySettings rare = GetRaritySettings(rareSettings);
        PotionRaritySettings epic = GetRaritySettings(epicSettings);

        int minStrength = (int)minStrengthField.value;
        string folderPath = folderPathField.value;

        config.common = common;
        config.rare = rare;
        config.epic = epic;
        config.minStrength = minStrength;
        config.folderPath = folderPath;

        return config;
    }


    private void GenerateRecipes()
    {
        PotionRecipeGeneratorConfiguration config = CreateConfiguration();
        // Check the config is valid
        pcgManager.GeneratePotionRecipes(config);
    }

    private void LoadValuesFromConfig(string path = "")
    {
        PotionRecipeGeneratorConfiguration config = path == ""
            ? defaultConfig
            : AssetDatabase.LoadAssetAtPath<PotionRecipeGeneratorConfiguration>(path);

        if (config == null)
        {
            Debug.LogError("Invalid Config");
            return;
        }

        SetRaritySettings(commonSettings, config.common);
        SetRaritySettings(rareSettings, config.rare);
        SetRaritySettings(epicSettings, config.epic);

        minStrengthField.value = config.minStrength;
        folderPathField.value = config.folderPath;
    }

    private void SaveConfigFile()
    {
        PotionRecipeGeneratorConfiguration config = CreateConfiguration();

        string potionRaritySettingsFolder = "Assets/ScriptableObjects/PCG/PotionRecipeRaritySettings/" + configNameField.value;
        AssetDatabase.CreateFolder("Assets/ScriptableObjects/PCG/PotionRecipeRaritySettings", configNameField.value);
        AssetDatabase.CreateAsset(config.common, potionRaritySettingsFolder + "/" + configNameField.value + "Common.asset");
        AssetDatabase.CreateAsset(config.rare, potionRaritySettingsFolder + "/" + configNameField.value + "Rare.asset");
        AssetDatabase.CreateAsset(config.epic, potionRaritySettingsFolder + "/" + configNameField.value + "Epic.asset");


        AssetDatabase.CreateAsset(config, configPathField.value + configNameField.value + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }   
    
    private void DeleteConfigFile()
    {
        AssetDatabase.DeleteAsset(configPathField.value + configNameField.value + ".asset");
        AssetDatabase.DeleteAsset("Assets/ScriptableObjects/PCG/PotionRecipeRaritySettings/" + configNameField.value);
    }

    private PotionRaritySettings GetRaritySettings(RecipeRaritySettingsUI raritySettingsUI)
    {
        Range ingredientsAmount = new Range((int)raritySettingsUI.minAmount.value, (int)raritySettingsUI.maxAmount.value);
        Range mainEffect = new Range((int)raritySettingsUI.minMainEffect.value, (int)raritySettingsUI.maxMainEffect.value);
        Range secondaryEffect = new Range((int)raritySettingsUI.minSecondaryEffect.value, (int)raritySettingsUI.maxSecondaryEffect.value);

        List<float> amountProbabilities = new List<float>();
        for (int i = 0; i < raritySettingsUI.effectAmountSliders.Count; i++)
        {
            amountProbabilities.Add(raritySettingsUI.effectAmountSliders[i].value);
        }

        return CreateInstance<PotionRaritySettings>().Init(ingredientsAmount, amountProbabilities, mainEffect, secondaryEffect);
    }

    private void SetRaritySettings(RecipeRaritySettingsUI raritySettingsUI, PotionRaritySettings settings)
    {
        raritySettingsUI.minAmount.value = settings.GetIngredientAmount().minValue;
        raritySettingsUI.maxAmount.value = settings.GetIngredientAmount().maxValue;

        raritySettingsUI.minMainEffect.value = settings.GetMainEffectStrength().minValue;
        raritySettingsUI.maxMainEffect.value = settings.GetMainEffectStrength().maxValue;

        raritySettingsUI.minSecondaryEffect.value = settings.GetSecondaryEffectStrength().minValue;
        raritySettingsUI.maxSecondaryEffect.value = settings.GetSecondaryEffectStrength().maxValue;

        for (int i = 0; i < raritySettingsUI.effectAmountSliders.Count; i++)
        {
            float val = settings.GetAmountOfEffects(i);
            raritySettingsUI.effectAmountSliders[i].value = val;
        }
    }

    private RecipeRaritySettingsUI CreateRaritySettingsUI()
    {
        RecipeRaritySettingsUI raritySettings = new RecipeRaritySettingsUI();

        for (int i = 0; i < 3; i++)
        {
            Slider slider = VisualSetting.CreateProbabilitySlider(i.ToString());
            raritySettings.effectAmountSliders.Add(slider);
        }

        raritySettings.minAmount = VisualSetting.CreateFloatField("", VisualSetting.shortWidth);
        raritySettings.maxAmount = VisualSetting.CreateFloatField("", VisualSetting.shortWidth);
        raritySettings.minMainEffect = VisualSetting.CreateFloatField("", VisualSetting.shortWidth);
        raritySettings.maxMainEffect = VisualSetting.CreateFloatField("", VisualSetting.shortWidth); 
        raritySettings.minSecondaryEffect = VisualSetting.CreateFloatField("", VisualSetting.shortWidth); 
        raritySettings.maxSecondaryEffect = VisualSetting.CreateFloatField("", VisualSetting.shortWidth);

        return raritySettings;
    }

    private void ShowRaritySettings(string header, RecipeRaritySettingsUI settingsUI, VisualElement parent)
    {
        parent.Add(VisualSetting.CreateHeader(header, VisualSetting.subheaderFontSize));
        parent.Add(VisualSetting.CreateHeader("Required amount of unique ingredients", VisualSetting.normalFontSize));
        ScrollView ingredientsScroll = VisualSetting.CreateScrollView("Amount range", settingsUI.minAmount, settingsUI.maxAmount);
        parent.Add(ingredientsScroll);

        parent.Add(VisualSetting.CreateHeader("Secondary effect amount probabilities", VisualSetting.normalFontSize));
        foreach (Slider slider in settingsUI.effectAmountSliders)
            parent.Add(slider);

        ScrollView mainScroll = VisualSetting.CreateScrollView("Main effect strength range", settingsUI.minMainEffect, settingsUI.maxMainEffect, VisualSetting.mediumWidth);
        parent.Add(mainScroll);

        ScrollView secondaryScroll = VisualSetting.CreateScrollView("Secondary effect strength range", settingsUI.minSecondaryEffect, settingsUI.maxSecondaryEffect, VisualSetting.mediumWidth);
        parent.Add(secondaryScroll);
    }
}
