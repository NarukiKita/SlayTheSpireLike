using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class CardUI : MonoBehaviour
{

//inspector領域====================================================================================

    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI effectText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private Button button;

//=============================================================================================

//変数=============================================================================================

    private Card card;

    //event
    public System.Action<CardUI> OnCardClicked;

//=========================================================================================

//Awake()=====================================================================================

    private void Awake()
    {
        if (button == null)
        {
            Debug.LogError("Buttonが設定されていません");
            return;
        }
        button.onClick.AddListener(OnClick);
    }

//===============================================================================================

//カードを表示する=========================================================================================

    public void Setup(Card cardData)
    {
        card = cardData;
    
        cardNameText.text = cardData.cardName;
        effectText.text = cardData.effectText;
        costText.text = cardData.cost.ToString();
    }

//===========================================================================================

//クリック検知の確認============================================================================================

    private void OnClick()
    {
        Debug.Log("OnclickまではOK");
        OnCardClicked?.Invoke(this);
    }

//==============================================================================================

    public Card GetCard()
    {
        return card;
    }


}
