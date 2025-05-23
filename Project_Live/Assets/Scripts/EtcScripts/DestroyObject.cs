using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//作成者：桑原

public class DestroyObject : MonoBehaviour
{
    [Header("消滅するまでの時間")]
    [SerializeField] float destroyDelay = 3f;

    void Start()
    {
        Destroy(this.gameObject, destroyDelay);
    }
}
