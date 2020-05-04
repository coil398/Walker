﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Searcher : MonoBehaviour
{
    [SerializeField]
    private SearchChecker searcherChecker = null;

    [SerializeField]
    private SearcherMover searcherMover = null;

    [SerializeField]
    private float waitTime = 4f;

    /// <summary>
    /// 目標となるプレイヤーインスタンス
    /// </summary>
    private Transform destinationPlayerTransform = null;

    private float remainTime = 0;

    private State currentState = State.SEARCH;

    enum State
    {
        SEARCH = 0,
        DISCOVERY = 1,
        STOP = 2
    }

    /// <summary>
    /// プレイヤーの設定
    /// </summary>
    /// <param name="_playerTrans"></param>
    public void SetPlayerTransform(Transform _playerTrans)
    {
        destinationPlayerTransform = _playerTrans;
        searcherChecker.SetPlayerTransform(_playerTrans);
    }

    /// <summary>
    /// 動作停止
    /// </summary>
    public void Pause()
    {
        currentState = State.STOP;
        searcherMover.SearchStop();
    }

    private void Update()
    {
        if (currentState == State.STOP)
        {
            return;
        }

        remainTime -= Time.deltaTime;
        if (remainTime < 0)
        {
            search();
            if (currentState == State.SEARCH)
            {
                remainTime += waitTime;
                return;
            }
        }

        if (currentState == State.DISCOVERY)
        {
            discovery();
        }
    }

    private void search()
    {
        RaycastHit hit;
        if (searcherChecker.SearchPlayer(out hit))
        {
            //発見
            switch (currentState)
            {
                case State.SEARCH:
                    currentState = State.DISCOVERY;
                    Debug.Log("<color=yellow>発見</color>");
                    break;
            }
        }
        else
        {
            if (currentState == State.DISCOVERY)
            {
                currentState = State.SEARCH;
                searcherMover.SearchStop();
            }
        }
    }

    private void discovery()
    {
        //発見中はキャラに向かって走る
        searcherMover.MoveToward(destinationPlayerTransform.transform);
    }
}
