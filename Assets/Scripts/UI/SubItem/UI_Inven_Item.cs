using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Inven_Item : UI_Base
{
    enum GameObjects
    {
        ItemIcon,
        ItemNameText
    }

    private string _name;
    
    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        GetGameObject((int)GameObjects.ItemNameText).GetComponent<TextMeshProUGUI>().text = _name;
        GetGameObject((int)GameObjects.ItemIcon).BindEvent((data =>
        {
            Debug.Log($"아이템 클릭 : {_name}");
        }));
    }
    
    void Start()
    {
        Init();
    }

    public void SetInfo(string name)
    {
        _name = name;
    }

}
