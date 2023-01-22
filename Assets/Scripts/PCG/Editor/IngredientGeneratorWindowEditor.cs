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

public class ToggleUI
{
    public List<Toggle> toggles = new List<Toggle>();
    public Button toggleButton;
    public bool state = true;
}

public class IngredientGeneratorWindowEditor : EditorWindow
{
    private Slider rareProbabilitySlider;
    private Slider epicProbabilitySlider;

    private RaritySettingsUI commonSettings;
    private RaritySettingsUI rareSettings;
    private RaritySettingsUI epicSettings;

    private IntegerField amountToGenerateField;
    private TextField folderPathField;

    private TextField configPathField;
    private TextField configNameField;

    private ToggleUI effectTypesUI;
    private ToggleUI mainEffectsUI;
    private ToggleUI secondaryEffectsUI;

    [SerializeField] private IngredientGeneratorConfiguration defaultConfig;

    private IngredientGenerator pcgManager;

    public VisualElement CreateGUI()
    {
        pcgManager = FindObjectOfType<IngredientGenerator>();

        VisualElement basicConfigurationElem = new VisualElement();
        VisualSetting.SetContainerVisuals(basicConfigurationElem);
        ShowBasicSettings(basicConfigurationElem);
        rootVisualElement.Add(basicConfigurationElem);

        VisualElement constraintsElem = new VisualElement();
        VisualSetting.SetContainerVisuals(constraintsElem);
        ShowConstraintsSettings(constraintsElem);
        rootVisualElement.Add(constraintsElem);

        VisualElement configElem = new VisualElement();
        VisualSetting.SetContainerVisuals(configElem);
        ShowConfigFileSettings(configElem);
        rootVisualElement.Add(configElem);

        rootVisualElement.style.flexDirection = FlexDirection.Row;

        LoadValuesFromConfig();

        return rootVisualElement;
    }

    private void ShowBasicSettings(VisualElement basicConfigurationElem)
    {
        Label title = VisualSetting.CreateHeader("Basic ingredient generator settings", VisualSetting.titleFontSize);
        basicConfigurationElem.Add(title);
        Label header = VisualSetting.CreateHeader("Rarity probabilities", VisualSetting.headerFontSize);
        header.tooltip = "The probabilities with which the rarities are selected. Common is not included as it is a default.";
        basicConfigurationElem.Add(VisualSetting.CreateHeader("Rarity probabilities", VisualSetting.headerFontSize));
        rareProbabilitySlider = VisualSetting.CreateProbabilitySlider("Rare");
        basicConfigurationElem.Add(rareProbabilitySlider);
        epicProbabilitySlider = VisualSetting.CreateProbabilitySlider("Epic");
        basicConfigurationElem.Add(epicProbabilitySlider);

        commonSettings = CreateRaritySettingsUI();
        rareSettings = CreateRaritySettingsUI();
        epicSettings = CreateRaritySettingsUI();

        basicConfigurationElem.Add(VisualSetting.CreateHeader("Rarity settings", VisualSetting.headerFontSize));
        ShowRaritySettingsUI("Common", commonSettings, basicConfigurationElem);
        ShowRaritySettingsUI("Rare", rareSettings, basicConfigurationElem);
        ShowRaritySettingsUI("Epic", epicSettings, basicConfigurationElem);


        basicConfigurationElem.Add(VisualSetting.CreateHeader("Amount to generate", VisualSetting.subheaderFontSize));
        amountToGenerateField = VisualSetting.CreateIntegerField("");
        basicConfigurationElem.Add(amountToGenerateField);

        basicConfigurationElem.Add(VisualSetting.CreateHeader("Target folder path", VisualSetting.subheaderFontSize));
        folderPathField = new TextField();
        folderPathField.style.width = VisualSetting.longWidth;
        basicConfigurationElem.Add(folderPathField);

        basicConfigurationElem.Add(VisualSetting.CreateButton("Generate ingredients", () => GenerateIngredients()));
        basicConfigurationElem.Add(VisualSetting.CreateButton("Reset default values", () => LoadValuesFromConfig()));
        basicConfigurationElem.Add(VisualSetting.CreateButton("Delete ingredients from folder", () => IngredientGenerator.Delete(folderPathField.value)));
    }

    private void ShowConstraintsSettings(VisualElement constraintsElem)
    {
        Label title = VisualSetting.CreateHeader("Ingredient generator constraint settings", VisualSetting.titleFontSize);
        constraintsElem.Add(title);

        // Effect Types
        VisualElement eTypeHeader = new VisualElement();
        constraintsElem.Add(eTypeHeader);
        eTypeHeader.style.flexDirection = FlexDirection.Row;
        Label eToggleHeader = VisualSetting.CreateHeader("Ignore effect types", VisualSetting.headerFontSize);
        eTypeHeader.Add(eToggleHeader);
        ShowEffectTypes();
        effectTypesUI.toggleButton = VisualSetting.CreateButton("Toggle On", () => ToggleAll(effectTypesUI), VisualSetting.shortWidth);
        eTypeHeader.Add(effectTypesUI.toggleButton);

        foreach (Toggle t in effectTypesUI.toggles)
            constraintsElem.Add(t);


        //Main Effect
        VisualElement mHeader = new VisualElement();
        mHeader.style.flexDirection = FlexDirection.Row;
        constraintsElem.Add(mHeader);
        Label mToggleHeader = VisualSetting.CreateHeader("Ignore main effects", VisualSetting.headerFontSize);
        mHeader.Add(mToggleHeader);
        mainEffectsUI = new ToggleUI();
        mainEffectsUI.toggleButton = VisualSetting.CreateButton("Toggle On", () => ToggleAll(mainEffectsUI), VisualSetting.shortWidth);
        mHeader.Add(mainEffectsUI.toggleButton);
        ShowEffects(mainEffectsUI);
        foreach (Toggle t in mainEffectsUI.toggles)
            constraintsElem.Add(t);

        //Secondary Effects
        VisualElement sHeader = new VisualElement();
        sHeader.style.flexDirection = FlexDirection.Row;
        constraintsElem.Add(sHeader);
        Label sToggleHeader = VisualSetting.CreateHeader("Ignore secondary effects", VisualSetting.headerFontSize);
        sHeader.Add(sToggleHeader);
        secondaryEffectsUI = new ToggleUI();
        secondaryEffectsUI.toggleButton = VisualSetting.CreateButton("Toggle On", () => ToggleAll(secondaryEffectsUI), VisualSetting.shortWidth);
        sHeader.Add(secondaryEffectsUI.toggleButton);
        ShowEffects(secondaryEffectsUI);
        foreach (Toggle t in secondaryEffectsUI.toggles)
            constraintsElem.Add(t);
    }

    private void ShowEffectTypes()
    {
        effectTypesUI = new ToggleUI();
        string[] effectTypes = System.Enum.GetNames(typeof(EffectType));
        effectTypesUI.toggles = new List<Toggle>();
        for (int i = 1; i < effectTypes.Length; i++)
        {
            Toggle toggle = new Toggle();
            toggle.label = effectTypes[i];
            effectTypesUI.toggles.Add(toggle);
        }
    }

    private void ShowEffects(ToggleUI ui)
    {
        foreach (Effect effect in FindObjectOfType<AlchemyGeneratorManager>().effects)
        {
            Toggle toggle = new Toggle();
            toggle.label = effect.GetEffectName();
            ui.toggles.Add(toggle);
        }
    }

    private void ToggleAll(ToggleUI ui)
    {
        foreach (Toggle t in ui.toggles)
            t.value = ui.state;
        ui.state = !ui.state;
        ui.toggleButton.text = ui.state
            ? "Toggle On"
            : "Toggle Off";

    }

    private void ShowConfigFileSettings(VisualElement configElem)
    {
        configElem.Add(VisualSetting.CreateHeader("Configuration files", VisualSetting.titleFontSize));
        configElem.Add(VisualSetting.CreateHeader("Folder", VisualSetting.subheaderFontSize));
        configPathField = new TextField();
        configPathField.style.width = VisualSetting.longWidth;
        configElem.Add(configPathField);
        configPathField.value = "Assets/ScriptableObjects/PCG/IngredientConfigs/";

        configElem.Add(VisualSetting.CreateHeader("Name", VisualSetting.subheaderFontSize));
        configNameField = new TextField();
        configNameField.style.width = VisualSetting.longWidth;
        configElem.Add(configNameField);
        configElem.Add(VisualSetting.CreateButton("Save config file", () => SaveConfigFile()));
        configElem.Add(VisualSetting.CreateButton("Delete config file and its settings", () => DeleteConfigFile()));
        configElem.Add(VisualSetting.CreateButton("Load values from config", () => LoadValuesFromConfig(configPathField.value + configNameField.value + ".asset")));
    }

    private void GenerateIngredients()
    {
        IngredientGeneratorConfiguration config = CreateConfiguration();
        // Check that the configuration is valid
        pcgManager.GenerateIngredients(config);
    }

    private void SaveConfigFile()
    {
        IngredientGeneratorConfiguration config = CreateConfiguration();

        string ingredientRaritySettignsFolder = "Assets/ScriptableObjects/PCG/IngredientRaritySettings/" + configNameField.value;
        AssetDatabase.CreateFolder("Assets/ScriptableObjects/PCG/IngredientRaritySettings", configNameField.value);
        AssetDatabase.CreateAsset(config.common, ingredientRaritySettignsFolder + "/" + configNameField.value + "Common.asset");
        AssetDatabase.CreateAsset(config.rare, ingredientRaritySettignsFolder + "/" + configNameField.value + "Rare.asset");
        AssetDatabase.CreateAsset(config.epic, ingredientRaritySettignsFolder + "/" + configNameField.value + "Epic.asset");


        AssetDatabase.CreateAsset(config, configPathField.value + configNameField.value + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void DeleteConfigFile()
    {
        AssetDatabase.DeleteAsset(configPathField.value + configNameField.value + ".asset");
        AssetDatabase.DeleteAsset("Assets/ScriptableObjects/PCG/IngredientRaritySettings/" + configNameField.value);
    }

    private IngredientGeneratorConfiguration CreateConfiguration()
    {
        IngredientGeneratorConfiguration config = ScriptableObject.CreateInstance<IngredientGeneratorConfiguration>();
        IngredientRaritySettings common = CreateRaritySettings(commonSettings);
        IngredientRaritySettings rare = CreateRaritySettings(rareSettings);
        IngredientRaritySettings epic = CreateRaritySettings(epicSettings);

        int amount = amountToGenerateField.value;
        float rareProbability = rareProbabilitySlider.value;
        float epicProbability = epicProbabilitySlider.value;

        config.common = common;
        config.rare = rare;
        config.epic = epic;
        config.amountToGenerate = amount;
        config.rareProbability = rareProbability;
        config.epicProbability = epicProbability;

        config.folderPath = folderPathField.value;

        for (int i = 0; i < effectTypesUI.toggles.Count; i++)
        {
            if (effectTypesUI.toggles[i].value)
                config.ignoreEffectTypes.Add((EffectType)(i + 1));
        }

        for (int i = 0; i < mainEffectsUI.toggles.Count; i++)
        {
            if (mainEffectsUI.toggles[i].value)
                config.ignoreMainEffects.Add(FindObjectOfType<AlchemyGeneratorManager>().effects[i]);
            if (secondaryEffectsUI.toggles[i].value)
                config.ignoreSecondaryEffects.Add(FindObjectOfType<AlchemyGeneratorManager>().effects[i]);
        }

        return config;
    }

    private void LoadValuesFromConfig(string path = "")
    {
        IngredientGeneratorConfiguration config = path == ""
            ? defaultConfig
            : AssetDatabase.LoadAssetAtPath<IngredientGeneratorConfiguration>(path);
        if (config == null)
        {
            Debug.LogError("Invalid Config");
            return;
        }

        SetRaritySettings(commonSettings, config.common);
        SetRaritySettings(rareSettings, config.rare);
        SetRaritySettings(epicSettings, config.epic);

        amountToGenerateField.value = config.amountToGenerate;
        rareProbabilitySlider.value = config.rareProbability;
        epicProbabilitySlider.value = config.epicProbability;
        folderPathField.value = config.folderPath;

        for (int i = 0; i < effectTypesUI.toggles.Count; i++)
        {
            effectTypesUI.toggles[i].value = config.ignoreEffectTypes.Contains((EffectType)(i + 1));
        }

        for (int i = 0; i < mainEffectsUI.toggles.Count; i++)
        {
            mainEffectsUI.toggles[i].value = config.ignoreMainEffects.Contains(FindObjectOfType<AlchemyGeneratorManager>().effects[i]);
            secondaryEffectsUI.toggles[i].value = config.ignoreSecondaryEffects.Contains(FindObjectOfType<AlchemyGeneratorManager>().effects[i]);
        }
    }

    private void SetRaritySettings(RaritySettingsUI raritySettingsUI, IngredientRaritySettings settings)
    {
        for (int i = 0; i < raritySettingsUI.effectAmountSliders.Count; i++)
        {
            float val = settings.GetProbability(i);
            raritySettingsUI.effectAmountSliders[i].value = val;
        }
        raritySettingsUI.minSum.value = settings.GetSumRange().Item1;
        raritySettingsUI.maxSum.value = settings.GetSumRange().Item2;
        raritySettingsUI.minPrimary.value = settings.GetPrimaryValueRange().Item1;
        raritySettingsUI.maxPrimary.value = settings.GetPrimaryValueRange().Item2;
    }

    private IngredientRaritySettings CreateRaritySettings(RaritySettingsUI raritySettingsUI)
    {
        List<float> amountProbabilities = new List<float>();
        for (int i = 0; i < raritySettingsUI.effectAmountSliders.Count; i++)
        {
            amountProbabilities.Add(raritySettingsUI.effectAmountSliders[i].value);
        }

        return ScriptableObject.CreateInstance<IngredientRaritySettings>().Init(amountProbabilities, raritySettingsUI.minSum.value,
            raritySettingsUI.maxSum.value, raritySettingsUI.minPrimary.value, raritySettingsUI.maxPrimary.value);

    }    

    private RaritySettingsUI CreateRaritySettingsUI()
    {
        RaritySettingsUI raritySettings = new RaritySettingsUI();
        for (int i = 0; i < 3; i++)
        {
            Slider slider = VisualSetting.CreateProbabilitySlider((i + 1).ToString());
            raritySettings.effectAmountSliders.Add(slider);
        }

        raritySettings.minSum = VisualSetting.CreateFloatField("", VisualSetting.shortWidth);
        raritySettings.maxSum = VisualSetting.CreateFloatField("", VisualSetting.shortWidth);
        raritySettings.minPrimary = VisualSetting.CreateFloatField("", VisualSetting.shortWidth);
        raritySettings.maxPrimary = VisualSetting.CreateFloatField("", VisualSetting.shortWidth);

        return raritySettings;
    }

    private void ShowRaritySettingsUI(string header, RaritySettingsUI settings, VisualElement basicConfigurationElem)
    {
        Label h = VisualSetting.CreateHeader(header, VisualSetting.subheaderFontSize);
        h.tooltip = "All settings for the rarity.";
        basicConfigurationElem.Add(h);
        Label sH = VisualSetting.CreateHeader("Effect Amount Probabilities", VisualSetting.normalFontSize);
        sH.tooltip = "Probabilities for the amount of effects the ingredient is going to have. The probabilities have to sum up to 1";
        basicConfigurationElem.Add(sH);
        foreach (Slider slider in settings.effectAmountSliders)
            basicConfigurationElem.Add(slider);
        ScrollView sumScroll = VisualSetting.CreateScrollView("Sum range", settings.minSum, settings.maxSum);
        sumScroll.tooltip = "The range for the sum of strenghts of the effects.";
        basicConfigurationElem.Add(sumScroll);
        ScrollView primaryScroll = VisualSetting.CreateScrollView("Primary range", settings.minPrimary, settings.maxPrimary);
        primaryScroll.tooltip = "The range for the strength of the primary effect of the ingredient";
        basicConfigurationElem.Add(primaryScroll);
    }  
}
