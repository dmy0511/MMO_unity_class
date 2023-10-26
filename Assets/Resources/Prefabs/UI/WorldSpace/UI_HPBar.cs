using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_HPBar : UI_Base
{
    enum Gameobjects
    {
        HPBar
    }

    private Stat _stat;

    public override void Init()
    {
        Bind<GameObject>(typeof(Gameobjects));
        _stat = transform.parent.GetComponent<Stat>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Transform parent = transform.parent;
        transform.position = parent.position + Vector3.up * (parent.GetComponent<Collider>().bounds.size.y);
        transform.rotation = Camera.main.transform.rotation;

        float ratio = _stat.Hp / (float)_stat.MaxHp;
        SetHpRatio(ratio);
    }

    private void SetHpRatio(float ratio)
    {
        GetGameObject((int)Gameobjects.HPBar).GetComponent<Slider>().value = ratio;
    }
}
