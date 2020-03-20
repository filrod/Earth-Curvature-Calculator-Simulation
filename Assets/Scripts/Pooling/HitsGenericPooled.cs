using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitsGenericPooled : MonoBehaviour
{
    private float lifeTime;
    [SerializeField] [Tooltip("Seconds for the hit object to linger")] private float maxLifetime = 0.05f;

    private void OnEnable()
    {
        lifeTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > maxLifetime)
            HitsPool.Instance.ReturnToPool(this);
    }
}
