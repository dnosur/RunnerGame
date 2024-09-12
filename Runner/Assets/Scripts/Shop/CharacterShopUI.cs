using UnityEngine;
using UnityEngine.UI;

public class CharacterShopUI : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] Transform ShopMenu;
    [SerializeField] Transform ShopItemsContainer;
    [SerializeField] GameObject itemPrefab;
    [Space(20)]
    [SerializeField] CharacterShopDatabase characterDB;

    [Space(20)]
    [Header("Shop Events")]
    [SerializeField] GameObject shopUI;
    [SerializeField] Button openShopButton;
    [SerializeField] Button closeShopButton;

    [Space(20)]
    [Header("Scroll View")]
    [SerializeField] ScrollRect scrollRect;

    [SerializeField] PlayerController playerController;

    int newSelectedItemIndex = 0;
    int previousSelectedItemIndex = 0;

    void Start()
    {
        AddShopEvents();

        GenerateShopItemsUI();

        SetSelectedCharacter();
        PlayerController playerController = FindObjectOfType<PlayerController>();

        SelectItemUI(playerController.GetPlayerIndex());

        AutoScrollShopList(playerController.GetPlayerIndex());
    }

    void AutoScrollShopList(int itemIndex)
    {
        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(1f - (itemIndex / (float)(characterDB.CharactersCount - 1)));
    }

    void SetSelectedCharacter()
    {
        int index = playerController.GetPlayerIndex();
        ShopDataManager.SetSelectedCharacter(characterDB.GetCharacter(index), index);
    }

    void GenerateShopItemsUI()
    {
        for (int i = 0; i < characterDB.CharactersCount; i++)
        {
            Character character = characterDB.GetCharacter(i);
            CharacterItemUI uiItem = Instantiate(itemPrefab, ShopItemsContainer).GetComponent<CharacterItemUI>();

            uiItem.SetCharacterImage(character.image);
            uiItem.SetCharacterPrice(character.price);

            if (character.isPurchased)
            {
                uiItem.SetCharacterAsPurchased();
                uiItem.OnItemSelect(i, OnItemSelected);
            }
            else
            {
                uiItem.OnItemPurchase(i, OnItemPurchased);
            }

  

        }

    }

    void OnItemSelected(int index)
    {
        SelectItemUI(index);
        ShopDataManager.SetSelectedCharacter(characterDB.GetCharacter(index), index);
        playerController.SetPlayerIndex(index);
        playerController.ChangePlayerSkin();

    }

    void SelectItemUI(int itemIndex)
    {
        previousSelectedItemIndex = newSelectedItemIndex;
        newSelectedItemIndex = itemIndex;

        CharacterItemUI prevUiItem = GetItemUI(previousSelectedItemIndex);
        CharacterItemUI newUiItem = GetItemUI(newSelectedItemIndex);

        prevUiItem.DeselectItem();
        newUiItem.SelectItem();

    }

    CharacterItemUI GetItemUI(int index)
    {
        return ShopItemsContainer.GetChild(index).GetComponent<CharacterItemUI>();
    }

    void OnItemPurchased(int index)
    {
        Character character = characterDB.GetCharacter(index);
        CharacterItemUI uiItem = GetItemUI(index);

        if (ShopDataManager.CanSpendCoins(character.price))
        {
            playerController.SpendCoins(character.price);

            characterDB.PurchaseCharacter(index);

            uiItem.SetCharacterAsPurchased();
            uiItem.OnItemSelect(index, OnItemSelected);

            ShopDataManager.AddPurchasedCharacter(index);

        }
        else
        {
            //No enough coins..
        }
    }

    void AddShopEvents()
    {
        openShopButton.onClick.RemoveAllListeners();
        openShopButton.onClick.AddListener(OpenShop);


        closeShopButton.onClick.RemoveAllListeners();
        closeShopButton.onClick.AddListener(CloseShop);
    }


    public void OpenShop()
    {
        shopUI.SetActive(true);
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
    }
}