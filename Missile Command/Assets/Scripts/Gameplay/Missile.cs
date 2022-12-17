using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour, IDestroyable
{
    [SerializeField] private float speed = 5;
    [SerializeField] private bool player;
    [SerializeField]private PointsData pointsData;

    private Rigidbody2D _rigidbody2D;
    private GameObject _target = null;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    

    public void Fire(GameObject target, int level=1)
    {
        level -= 1;
        _target = target;
        Vector2 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle += 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        _rigidbody2D.velocity = (speed+(0.25f*level))*-1*transform.up;
    }

    private void Explode()
    {
        gameObject.SetActive(false);
        Explosion explosion = ObjectPoolManager.Instance.GetElementByName("explosion").GetComponent<Explosion>();
        explosion.transform.position = _target.transform.position;
        explosion.gameObject.SetActive(true);
        explosion.Explode(() =>
        {
            GameManager.Instance.SubstractMissile(1, player);
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _target)
        {
            if (player) collision.gameObject.SetActive(false);
            Explode();
        }
    }


    public void Destroy()
    {
        if (player) return;
        pointsData.AddPoints(25);
        _target = gameObject;
        Explode();
    }
}
