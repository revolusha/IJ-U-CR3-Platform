using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]

public class GoldLabel : MonoBehaviour
{
    private Text _text;

    private void Start()
    {
        _text = GetComponent<Text>();
    }

    public void UpdateGold(int amount)
    {
        const int MaxGold = 9999;

        if (amount < 0)
        {
            amount = 0;
        }
        else if (amount > MaxGold)
        {
            amount = MaxGold;
        }

        _text.text = amount.ToString();
    }
}
