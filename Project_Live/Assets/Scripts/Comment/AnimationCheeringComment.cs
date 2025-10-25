using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class AnimationCheeringComment : MonoBehaviour
{
    [Header("アニメーション秒数")]
    [SerializeField] protected float animationDuration = 0.5f;
    [Header("アニメーションカーブ")]
    [SerializeField] protected AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    protected RectTransform rectTransform;
    protected RectTransform targetPosition;
    protected bool isAnimating = false;
    protected Animator animator;

    protected void OnButtonClicked(GameObject targetObj, Action onComplete = null)
    {
        // ターゲット位置を見つける
        if (targetPosition == null)
        {
            if (targetObj != null)
            {
                targetPosition = targetObj.GetComponent<RectTransform>();
                //Debug.Log("ターゲットが設定されました");
            }
        }

        if (targetPosition != null && !isAnimating)
        {
            //Debug.Log("コメントの移動を開始します");
            // コールバックをコルーチンに渡す
            StartCoroutine(AnimateToTarget(onComplete));
        }
        else
        {
            // アニメーションしない場合でもコールバックを実行
            onComplete?.Invoke();
        }
    }

    protected IEnumerator AnimateToTarget(Action onComplete = null)
    {
        yield return new WaitForSeconds(0.5f);

        animator.Play("ChangeScale");

        isAnimating = true;

        Vector2 startPos = rectTransform.anchoredPosition;
        Vector2 endPos = new Vector2(
            targetPosition.anchoredPosition.x + targetPosition.sizeDelta.x,
            targetPosition.anchoredPosition.y + targetPosition.sizeDelta.y 
        );
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / animationDuration);
            float curveValue = moveCurve.Evaluate(t);

            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, curveValue);
            yield return null;
        }

        rectTransform.anchoredPosition = endPos;
        isAnimating = false;

        // コールバックを実行
        onComplete?.Invoke();
    }
}
