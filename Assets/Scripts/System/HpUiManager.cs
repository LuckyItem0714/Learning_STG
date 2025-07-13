using UnityEngine;
using UnityEngine.UI; // ImageなどのUIコンポーネントを扱うために必要
using System.Collections.Generic; // Listを使うために必要

public class HpUiManager : MonoBehaviour
{
    public List<Image> heartIcons; //ハートのImageコンポーネントを格納するリスト
    
    public void UpdateHp(int currentHp)
    {
        //全てのハートアイコンをループで処理する
        for (int i = 0; i < heartIcons.Count; i++)
        {
            //もし、現在のループのインデックス(i)が、プレイヤーの現在HPより小さければ
            if (i < currentHp)
            {
                //そのハートを表示する
                heartIcons[i].enabled = true;
            }
            else
            {
                //そのハートを非表示にする
                heartIcons[i].enabled = false;
            }
        }
    }
}
