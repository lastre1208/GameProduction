using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class MoveState : IPlayerState
{
    PlayerAnimationController anim;
    MovePlayer movePlayer;

    public MoveState(PlayerAnimationController anim, MovePlayer movePlayer)
    {
        this.anim = anim;
        this.movePlayer = movePlayer;
    }

    public void Enter()
    {
        anim.PlayMove();
        //Debug.Log("移動状態に移行");
    }

    public void Update()
    {
        movePlayer.MoveProcess(); //移動の処理
    }

    public void Exit()
    {
        //Debug.Log("移動状態を終了");
    }
}
