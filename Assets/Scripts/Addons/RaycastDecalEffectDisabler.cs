using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDecalEffectDisabler : MonoBehaviour {

    private float raycastEffectDecalLifeTime = 3.0f;
    private void OnEnable()
    {
        StartCoroutine(DisableAfterTime());
    }
    private IEnumerator DisableAfterTime()
    {
        yield return new WaitForSeconds(raycastEffectDecalLifeTime);
        gameObject.SetActive(false);
    }
}
