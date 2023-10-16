using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedMakerUI : MonoBehaviour
{
    [Header("Instance")]
    public static SeedMakerUI Instance;

    [Header("Sub Panel")]
    public GameObject subPanel;

    public Image seedImage;
    public TextMeshProUGUI seedName;
    public TextMeshProUGUI timeNeeded;
    public TextMeshProUGUI count;

    public Image resourceImage;
    public TextMeshProUGUI resourceCount;
    public TextMeshProUGUI resourceName;

    public UnitValueSlider slider;

    public TextMeshProUGUI craftNumberText;

    [Header("Main Panel")]
    public GameObject mainPanel;

    public List<BaseItemProfileSO> seedProfiles;
    public GameObject itemPrefab;
    public List<ItemUI> listItem;

    protected BaseItemProfileSO curItem = null;

    [Header("Machine")]
    public MachineProfile curMachine;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    private void Start()
    {
        InitializeItemList();
        InitializeSliderListener();
    }

    private void InitializeItemList()
    {
        foreach (BaseItemProfileSO profile in seedProfiles)
        {
            GameObject itemGO = Instantiate(itemPrefab, mainPanel.transform);
            ItemUI itemUI = itemGO.GetComponent<ItemUI>();
            listItem.Add(itemUI);
            itemUI.SetupProfile(profile);
            if (profile.unlocked)
                itemUI.button.onClick.AddListener(() => SetupSubPanel(profile));
        }
    }

    private void InitializeSliderListener()
    {
        slider.OnValueChangedAction += OnSliderValueChanged;
    }

    public void SetupSubPanel(BaseItemProfileSO profile)
    {
        if (!subPanel.activeSelf)
            subPanel.SetActive(true);
        curItem = profile;

        // Set UI elements based on profile data

        UpdateCraftAmountUI(profile);
    }

    public void ChangeCraftAmount(bool increase)
    {
        if (increase)
            slider.UpdateValue(Mathf.Clamp(slider.curUnit + 1, 0, Mathf.FloorToInt(1 / slider.valuePerUnit)));
        else
            slider.UpdateValue(Mathf.Clamp(slider.curUnit - 1, 0, Mathf.FloorToInt(1 / slider.valuePerUnit)));
    }

    private void OnSliderValueChanged()
    {
        craftNumberText.text = "Craft " + slider.curUnit.ToString();
    }

    private void UpdateCraftAmountUI(BaseItemProfileSO profile)
    {
        // Update UI elements based on slider.curUnit, slider.totalAmount, slider.amountPerUnit
        seedImage.sprite = profile.sprite;
        seedName.text = profile.itemName;
        timeNeeded.text = profile.itemRecipe.timeNeeded.ToString() + "s";
        count.text = "x1";

        ItemRecipeIngredient ingredient = profile.itemRecipe.ingredients[0];

        resourceImage.sprite = ingredient.itemProfile.sprite;
        resourceName.text = ingredient.itemProfile.itemName;

        // update slider props
        int amountPerUnit = ingredient.count;
        int totalAmount = Inventory.Instance.ItemTotalCount(ingredient.itemProfile.itemCode);
        resourceCount.text = totalAmount.ToString() + "/" + amountPerUnit.ToString();
        totalAmount -= totalAmount % amountPerUnit;

        slider.valuePerUnit = totalAmount < amountPerUnit ? 0 : (float)amountPerUnit / totalAmount;
        slider.UpdateValue(0);

        craftNumberText.text = "Craft " + slider.curUnit.ToString();
    }

    public void Craft()
    {
        if (curMachine is not SeedMakerProfile seedMakerMachine) return;

        if (curItem == null) return;
        bool success = ItemCraft.Instance.CraftItem(curItem, slider.curUnit, false);

        if (!success) return;

        UIManager.Instance.ToggleUI(UIType.SeedMakerUI, false);

        seedMakerMachine.CraftItem(curItem, slider.curUnit);
    }

    public void ToggleUI(bool show)
    {
        if (!show) subPanel.SetActive(false);
        gameObject.SetActive(show);
    }
}
