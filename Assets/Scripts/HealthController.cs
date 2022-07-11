using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] Color _removedHeartColor;

    private SpriteRenderer[] _hearts;
    private int _maxHealth = 3;

    private void Start()
    {
        _hearts = GetComponentsInChildren<SpriteRenderer>();
        UpdateRender(_maxHealth); 
    }

    public void UpdateRender(int count)
    {
        if (count > _maxHealth)
        {
            count = _maxHealth;
        }
        else if (count < 0)
        {
            count = 0;
        }

        for (int i = 0; i < count; i++)
        {
            _hearts[i].color = Color.white;
        }

        for (int i = count; i < _maxHealth; i++)
        {
            _hearts[i].color = _removedHeartColor;
        }
    }
}
