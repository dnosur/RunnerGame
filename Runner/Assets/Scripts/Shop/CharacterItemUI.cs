using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class CharacterItemUI : MonoBehaviour
{
    [SerializeField] Color itemNotSelectedColor;
    [SerializeField] Color itemSelectedColor;

    [Space(20f)]
    [SerializeField] Image characterImage;
    [SerializeField] TMP_Text characterPriceText;
    [SerializeField] Button characterPurchaseButton;

    [Space(20f)]
    [SerializeField] Button itemButton;
    [SerializeField] Image itemImage;

    public void SetCharacterImage(Sprite sprite)
    {
        characterImage.sprite = sprite;
    }

    public void SetCharacterPrice(int price)
    {
        characterPriceText.text = price.ToString();
    }

    public void SetCharacterAsPurchased()
    {
        characterPurchaseButton.gameObject.SetActive(false);
        characterPriceText.gameObject.SetActive(false);
        itemButton.interactable = true;

        itemImage.color = itemNotSelectedColor;
    }

    public void OnItemPurchase(int itemIndex, UnityAction<int> action)
    {
        characterPurchaseButton.onClick.RemoveAllListeners();
        characterPurchaseButton.onClick.AddListener(() => action.Invoke(itemIndex));
    }

    public void OnItemSelect(int itemIndex, UnityAction<int> action)
    {
        itemButton.interactable = true;
        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(() => action.Invoke(itemIndex));
    }

    public void SelectItem()
    {
        itemImage.color = itemSelectedColor;
        itemButton.interactable = false;
    }

    public void DeselectItem()
    {
        itemImage.color = itemNotSelectedColor;
        itemButton.interactable = true;
    }
}