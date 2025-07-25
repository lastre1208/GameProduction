using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    [Header("攻撃を始める判定を行う領域")]
    [SerializeField] SphereCollider triggerRange;
    [Header("判定を行う対象のオブジェクト")]
    [SerializeField] string targetTag = "Player";
    //[Header("判定を始めるプレイヤーとの距離")]
    //[SerializeField] float triggerDistance;
    [Header("攻撃を行うまでに必要な判定時間")]
    [SerializeField] float triggerDuration = 1f;

    float currentTimer = 0f; //経過時間の測定用
    bool isAttackTrigger = false; //攻撃するかどうか

    public bool IsAttackTrigger { get { return isAttackTrigger; } set { isAttackTrigger = value; } }

    void OnTriggerStay(Collider other) //攻撃判定を行うエリアにプレイヤーが侵入しているときの処理
    {
        if (!other.CompareTag(targetTag)) return; //当たり判定を行うオブジェクトのタグがPlayerでなければ処理を行わない

        if (isAttackTrigger == true) return;

        currentTimer += Time.deltaTime;

        if (currentTimer > triggerDuration)
        {
            isAttackTrigger = true;//攻撃の処理
        }
    }

    void OnTriggerExit(Collider other)　//攻撃判定を行うエリアからプレイヤーが出たときの処理
    {
        currentTimer = 0f;
    }
}
