using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnDisable()
    {
        StopAllCoroutines();
        transform.localScale = new Vector2(0.4f, 0.4f);
    }

    public void Explode(Action onExplode)
    {
        StartCoroutine(nameof(ExplodeCoroutine), onExplode);
    }

    private IEnumerator ExplodeCoroutine(Action onExplode)
    {
        while (transform.localScale.x < 1)
        {
            yield return new WaitForSeconds(0.1f);
            transform.localScale += new Vector3(0.1f, 0.1f, 0);
        }
        while (transform.localScale.x > 0.1)
        {
            yield return new WaitForSeconds(0.1f);
            transform.localScale -= new Vector3(0.1f, 0.1f, 0);
        }
        gameObject.SetActive(false);
        onExplode.Invoke();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        IDestroyable toDestroy = other.gameObject.GetComponent<IDestroyable>();
        if (toDestroy != null)
        {
            toDestroy.Destroy();
        }
    }
}
