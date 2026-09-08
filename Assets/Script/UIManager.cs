using UnityEngine;
using TMPro;
using System.Collections;



public class UIManager : MonoBehaviour
{
//Inspector===============================================================================================================

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private CardUI cardUIPrefab;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private Deck deck;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private GameObject strengthUI;
    [SerializeField] private GameObject poisonUI;
    [SerializeField] private GameObject energyWarningText;
    [SerializeField] private GameObject discardSelectionUI;
    [SerializeField] private TextMeshProUGUI enemyHPText;
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private TextMeshProUGUI playerShieldText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI strengthUIText;
    [SerializeField] private TextMeshProUGUI poisonUIText;
    [SerializeField] private Transform handPanel;
    [SerializeField] private Transform discardSelectionPanel;
    [SerializeField] private Transform discardCardPanel;

//========================================================================================================================

//変数====================================================================================================================

    private bool isDiscardSelectionMode = false;
    //イベント
    public System.Action<Card> OnCardSelectedForDiscard;

//========================================================================================================================

//AddCard=================================================================================================================

    public void AddCardToHand(Card card)
    {
        CardUI cardUI = Instantiate(cardUIPrefab, handPanel);
        cardUI.Setup(card);
        if (isDiscardSelectionMode)
        {
            cardUI.OnCardClicked += OnDiscardFromHandClicked;
        }
        else
        {
            cardUI.OnCardClicked += battleManager.PlayCard;
        }
    }

//========================================================================================================================

//ShowDiscardPile=========================================================================================================

    public void ShowDiscardPile()
    {
        discardSelectionPanel.gameObject.SetActive(true);

        foreach (Transform child in discardCardPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (Card card in deck.DiscardPile)
        {
            CardUI cardUI = Instantiate(cardUIPrefab, discardCardPanel);
            cardUI.Setup(card);
            cardUI.OnCardClicked += OnDiscardCardClicked;
        }
    }

    public void DeleteDiscardPanel()
    {
        discardSelectionPanel.gameObject.SetActive(false);
    }

//========================================================================================================================

//捨て札画面から選択された時==================================================================================================
    private void OnDiscardCardClicked(CardUI cardUI)
    {
        Card card = cardUI.GetCard();
        deck.RetrieveFromDiscard(card);
        Destroy(cardUI.gameObject);
        AddCardToHand(card);
        discardSelectionPanel.gameObject.SetActive(false);
    }

//=======================================================================================================================

//ClearHand===============================================================================================================

    public void ClearHand()
    {
        foreach (Transform child in handPanel)
        {
            Destroy(child.gameObject);
        }
    }

//========================================================================================================================

//UpdateUI系列()HP,Energy,Buff等==========================================================================================================

    //EnemyHP=================================================================================================
    public void UpdateEnemyHPUI()
    {
        enemyHPText.text = "HP: " + enemyManager.HP;
    }

    //Energy=================================================================================================
    public void UpdateEnergyUI()
    {
        
        energyText.text = "Energy: " + playerManager.CurrentEnergy + " / " + playerManager.MaxEnergy;
    }

    //PlayerHP================================================================================================
    public void UpdatePlayerHPUI()
    {
        playerHPText.text = "HP: " + playerManager.HP;
    }

    //PlayerShield============================================================================================
    public void UpdatePlayerShieldUI()
    {
        playerShieldText.text = "Shield: " + playerManager.Shield;
    }

    //StrengthUI==============================================================================================
    public void UpdateStrengthUI()
    {
        strengthUI.SetActive(playerManager.Strength > 0);
    }

    //StrengthText============================================================================================
    public void UpdateStrengthUIText()
    {
        strengthUIText.text = "" + playerManager.Strength;
    }

    //PoisonUI================================================================================================
    public void UpdatePoisonUI()
    {
        poisonUI.SetActive(enemyManager.Poison>0);
    }

    //PoisonUIText============================================================================================
    public void UpdatePoisonUIText()
    {
        poisonUIText.text = "" + enemyManager.Poison;
    }


//=======================================================================================================================

//EnergyShortage()=============================================================================================================

    public IEnumerator ShowEnergyWarning()
    {
        energyWarningText.SetActive(true);
        energyText.color = Color.red;

        yield return new WaitForSeconds(0.3f);

        energyWarningText.SetActive(false);
        energyText.color = Color.white;
    }

//===============================================================================================================================

//手札廃棄UI関係======

    public void ShowHandForDiscardSelection()
    {
        isDiscardSelectionMode = true;

        foreach (Transform child in handPanel)
        {
            CardUI cardUI = child.GetComponent<CardUI>();

            if (cardUI != null)
            {
                cardUI.OnCardClicked -= battleManager.PlayCard;
                cardUI.OnCardClicked += OnDiscardFromHandClicked;
            }
        }
    }

    private void OnDiscardFromHandClicked(CardUI cardUI)
    {
        Card card = cardUI.GetCard();
        
        isDiscardSelectionMode = false;
        ReturnHandToNormalMode();
        OnCardSelectedForDiscard?.Invoke(card);
        Destroy(cardUI.gameObject);
    }

    public void ReturnHandToNormalMode()
    {
        foreach (Transform child in handPanel)
        {
            CardUI cardUI = child.GetComponent<CardUI>();

            if (cardUI != null)
            {
                cardUI.OnCardClicked -= OnDiscardFromHandClicked;
                cardUI.OnCardClicked += battleManager.PlayCard;
            }
        }
    }
//=======================================================================================================================

//Win・Defeat==============================================================================================================

    public void WinUI()
    {
        victoryPanel.SetActive(true);
    }

    public void DefeatUI()
    {
        defeatPanel.SetActive(true);
    }
//=======================================================================================================================
}
