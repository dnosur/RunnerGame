using System.Collections.Generic;

//Shop Data Holder
[System.Serializable]
public class CharactersShopData
{
    public List<int> purchasedCharactersIndexes = new List<int>();
}

public static class ShopDataManager
{
    static PlayerController playerData = new PlayerController();
    static CharactersShopData charactersShopData = new CharactersShopData();

    static Character selectedCharacter;

    public static Character GetSelectedCharacter()
    {
        return selectedCharacter;
    }

    public static void SetSelectedCharacter(Character character, int index)
    {
        selectedCharacter = character;
        playerData.SetPlayerIndex(index);
    }

    public static bool CanSpendCoins(int amount)
    {
        return (playerData.GetCoins() >= amount);
    }

    public static void AddPurchasedCharacter(int characterIndex)
    {
        charactersShopData.purchasedCharactersIndexes.Add(characterIndex);
    }

    public static List<int> GetAllPurchasedCharacter()
    {
        return charactersShopData.purchasedCharactersIndexes;
    }

    public static int GetPurchasedCharacter(int index)
    {
        return charactersShopData.purchasedCharactersIndexes[index];
    }

}