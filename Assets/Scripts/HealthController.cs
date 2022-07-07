using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] Color _removedHeartColor;

    private SpriteRenderer[] _hearts;
    private int _maxHealth;
    private int _health;

    private void Start()
    {
        _hearts = GetComponentsInChildren<SpriteRenderer>();
        _maxHealth = _hearts.Length;
        _health = _maxHealth;
        UpdateRender(); 
    }

    public void AddLife()
    {
        if (_health < _maxHealth)
        {
            _health++;
            UpdateRender();
        }
    }

    public void RemoveLife()
    {
        if (_health > 0)
        {
            _health--;
            UpdateRender();
        }
    }

    private void UpdateRender()
    {
        Debug.Log("rend");
        for (int i = 0; i < _health; i++)
        {
            _hearts[i].color = Color.white;
        }

        for (int i = _health; i < _maxHealth; i++)
        {
            _hearts[i].color = _removedHeartColor;
        }
    }
}
