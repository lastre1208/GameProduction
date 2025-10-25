using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CheeringComment  :AnimationCheeringComment
{
    [Header("加えるHP")]
    [SerializeField] float addHPnum;
    [Header("HPが何%以下になったら回復にするか")]
    [SerializeField,Range(1,100)] float hpPercent;
    [Header("バフする攻撃倍率")]
    [SerializeField] float attackMagnification;
    [Header("バフする速度倍率")]
    [SerializeField] float agilityMagnification;
    [Header("バフの持続時間")]
    [SerializeField] float buffTime;
    [Header("応援コメントの内容")]
    public  List<string> cheeringCommentContent = new List<string>();
    [Header("決定時のエフェクト")]
    [SerializeField] ParticleSystem particleEffect;

    PlayerStatus playerStatus;
    PlayerBuffManager playerBuffManager;
    CommentSpawn commentSpawn;
    CommentMove commentMove;
    
    Button thisComment;

    GameObject PowerBuffImage;
    GameObject AgilityBuffImage;

    private void Start()
    {
        playerBuffManager=GameObject.FindGameObjectWithTag("PlayerBuffManager").GetComponent<PlayerBuffManager>();
        playerStatus=GameObject.FindGameObjectWithTag("PlayerStatus").GetComponent<PlayerStatus>();
        commentSpawn = GameObject.FindGameObjectWithTag("CommentSpawn").GetComponent<CommentSpawn>();
        thisComment=this.gameObject.GetComponent<Button>();
        commentMove=this.gameObject.GetComponent<CommentMove>();

        animator = this.gameObject.GetComponent<Animator>();

        rectTransform=this.GetComponent<RectTransform>();
        PowerBuffImage = GameObject.FindGameObjectWithTag("PowerBuffImage");
        AgilityBuffImage = GameObject.FindGameObjectWithTag("AgilityBuffImage");

        commentSpawn.cheeringCommentIsExist=true;

        SetCommentAction();
    }

    private void SetCommentAction()
    {
        thisComment.onClick.RemoveAllListeners(); // 念のためリスナーをクリア


        if (playerStatus.Hp / playerStatus.MaxHp <= hpPercent/100)  // HPが指定%以下なら回復
        {
            thisComment.onClick.AddListener(AddHP);
        }
        else
        {
            //  HPが指定%以上ならバフ処理
            bool attackActive = playerBuffManager.attackBuff != null && playerBuffManager.attackBuff.isActive;
            bool speedActive = playerBuffManager.speedBuff != null && playerBuffManager.speedBuff.isActive;

            if (attackActive && !speedActive)
            {
                // 攻撃バフ中ならスピードバフ
                thisComment.onClick.AddListener(BuffAgility);
            }
            else if (!attackActive && speedActive)
            {
                // スピードバフ中なら攻撃バフ
                thisComment.onClick.AddListener(BuffAtack);
            }
            else
            {
                // 両方かかっている or 両方かかっていない場合はランダム
                if (Random.value < 0.5f)
                    thisComment.onClick.AddListener(BuffAtack);
                else
                    thisComment.onClick.AddListener(BuffAgility);
            }
        }
    }

    public void AddHP()
    {
        playerBuffManager.AddHP(addHPnum);
        DestroyComment();
    }

    public void BuffAtack()
    {
        particleEffect.Play();
        EventSystem.current.SetSelectedGameObject(null);
        commentMove.enabled = false;

        // コルーチン完了後にBuffAttackを実行するコールバックを渡す
        OnButtonClicked(PowerBuffImage, () =>
        {
            // アニメーション完了後に実行される
            playerBuffManager.BuffAttack(attackMagnification, buffTime);
            DestroyComment();
        });
    }

    public void BuffAgility()
    {
        particleEffect.Play();
        EventSystem.current.SetSelectedGameObject(null);
        commentMove.enabled = false;

        OnButtonClicked(AgilityBuffImage, () =>
        {
            playerBuffManager.BuffMoveSpeed(agilityMagnification, buffTime);
            DestroyComment();
        });
    }

    void DestroyComment()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        commentSpawn.cheeringCommentIsExist = false;
    }
}
