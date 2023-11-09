using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public int characterIndex;//0:Wheel, 1:Amy, 2:Michelle ...
    public GameObject[] shopCharacters;
    
    public GameObject mainMenu;
    public Button readyButton;
    public MainMenu mainMenuScript;

    void Start()
    {
        characterIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        foreach (var ch in shopCharacters) ch.SetActive(false);
        shopCharacters[characterIndex].SetActive(true);
    }

   
    public void ChangeNextCharacter()
    {
        shopCharacters[characterIndex].SetActive(false);

        characterIndex++;
        if (characterIndex == shopCharacters.Length)
            characterIndex = 0;

        shopCharacters[characterIndex].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
    }

    public void ChangePreviousCharacter()
    {
        shopCharacters[characterIndex].SetActive(false);

        characterIndex--;
        if (characterIndex == -1)
            characterIndex = shopCharacters.Length - 1;

        shopCharacters[characterIndex].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
    }

    public void Ready()
    {
        readyButton.interactable = false;
        mainMenuScript.PlayGame();
    }
}
