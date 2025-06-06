using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//作成者：桑原

public class GoodPointDisplay : MonoBehaviour
{
    [Header("いいねアクション1に使用するポイント蓄積量表示用テキスト")]
    [SerializeField] TextMeshProUGUI goodPointText1;
    [Header("いいねアクション2に使用するポイント蓄積量表示用テキスト")]
    [SerializeField] TextMeshProUGUI goodPointText2;
    [Header("いいねアクション3に使用するポイント蓄積量表示用テキスト")]
    [SerializeField] TextMeshProUGUI goodPointText3;
    [Header("いいねアクション4に使用するポイント蓄積量表示用テキスト")]
    [SerializeField] TextMeshProUGUI goodPointText4;

    [Header("通常時のテキストの色")]
    [SerializeField] Color normalColor = Color.blue;
    [Header("いいねアクション発動可能時のテキストの色")]
    [SerializeField] Color highlightColor = Color.red;

    [Header("必要なコンポーネント")]
    [SerializeField] GoodAction goodAction;
    [SerializeField] GoodPointNotifier notifier;

    void Start()
    {
        if (notifier != null)
            notifier.OnGoodPointChanged += HandleGoodPointChanged;

        RefreshAll(); //初期表示
    }

    void OnDestroy()
    {
        if (notifier == null) return;

        notifier.OnGoodPointChanged -= HandleGoodPointChanged;
    }

    void HandleGoodPointChanged(int index, int current) //受け取った通知の情報をもとに各表示を更新する
    {
        switch (index)
        {
            case 1:
                UpdateDisplay(goodPointText1, current, goodAction.GoodCost1, 1);
                break;
            
            case 2:
                UpdateDisplay(goodPointText2, current, goodAction.GoodCost2, 2);
                break;
            
            case 3:
                UpdateDisplay(goodPointText3, current, goodAction.GoodCost3, 3);
                break;
            
            case 4:
                UpdateDisplay(goodPointText4, current, goodAction.GoodCost4, 4);
                break;

            default: break;
        }
    }

    void UpdateDisplay(TextMeshProUGUI textObj, int currentPoint, int cost, int index) //表示の更新
    {
        textObj.text = "GP" + index + ":"+ currentPoint;
        textObj.color = (currentPoint >= cost) ? highlightColor : normalColor;
    }

    void RefreshAll() //表示の初期設定
    {
        HandleGoodPointChanged(1, goodAction.CurrentGoodPoint1);
        HandleGoodPointChanged(2, goodAction.CurrentGoodPoint2);
        HandleGoodPointChanged(3, goodAction.CurrentGoodPoint3);
        HandleGoodPointChanged(4, goodAction.CurrentGoodPoint4);
    }
}
