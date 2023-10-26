using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class PlayerController : BaseController
{
    //public float _speed = 3.0f;
    private PlayerStat _stat;

    private Texture2D _attackIcon;
    private Texture2D _handIcon;

    enum CursorType
    {
        None,
        Attack,
        Hand,
    }

    private CursorType _cursorType = CursorType.None;
    
    void Start()
    {
        WorldObjectType = Define.WorldObject.Player;
        _attackIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        _handIcon = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");

        _stat = gameObject.GetComponent<PlayerStat>();
        
        Managers.Input.MouseAction += OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
        
        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    void UpdateMouseCursor()
    {
        
    }

    void OnHitEvent()
    {
        Debug.Log("OnHitEvent");

        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }
        
        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
    }

    protected override void UpdateSkill()
    {
        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    protected override void UpdateMoving()
    {
        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }
        
        Vector3 dir = _destPos - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
            return;
        }

        Debug.DrawRay(transform.position + Vector3.up * 0.5f, dir.normalized, Color.green);
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
        {
            if (Input.GetMouseButton(0) == false)
                State = Define.State.Idle;
            return;
        }
        
        float moveDist = Math.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
        transform.position += dir.normalized * moveDist;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 50 * Time.deltaTime);
    }

    private int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    private bool _stopSkill = false;
    
    private void OnMouseEvent(Define.MouseEvent evt)
    {
        if (State == Define.State.Die)
            return;

        switch (State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
            {
                if (evt == Define.MouseEvent.PointerUp)
                    _stopSkill = true;
            }
                break;
        }
    }

    private void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        bool raycastHit = Physics.Raycast(ray, out hit, 100, _mask);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                if (raycastHit)
                {
                    _destPos = hit.point;
                    State = Define.State.Moving;
                    _stopSkill = false;

                    if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        _lockTarget = hit.collider.gameObject;
                    else
                        _lockTarget = null;
                }

                break;
            case Define.MouseEvent.Press:
                if (_lockTarget != null)
                {
                    _destPos = _lockTarget.transform.position;
                }
                else
                {
                    if (raycastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
}
