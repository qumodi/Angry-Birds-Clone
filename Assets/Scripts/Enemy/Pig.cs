using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PigSize { Small, Medium, Big, Boss ,OneShot}

public class Pig : MonoBehaviour
{
    [SerializeField] private List<Sprite> States;

    public PigSize CurrPigSize;

    private SpriteRenderer CurrSprite;

    void Start()
    {
        CurrSprite = GetComponent<SpriteRenderer>();
    }

    public float GetHpFoSize(PigSize size)
    {
        float multiplicator = 1f;
        float oneshotPig = 1f;
        float smallPig = 20f * multiplicator;
        float mediumPig = 25f * multiplicator;
        float bigPig = 30f * multiplicator;
        float bossPig = 40f * multiplicator;

        switch (size)
        {
            case PigSize.Small:
                return smallPig;
            case PigSize.Medium:
                return mediumPig;
            case PigSize.Big:
                return bigPig;
            case PigSize.Boss:
                return bossPig;
            case PigSize.OneShot:
                return oneshotPig;
            default:
                return smallPig;
        }

    }

    public void UpdateSprite(float PercentHP)
    {

        if (PercentHP >= 0.8f)
        {
            CurrSprite.sprite = States[0];
        }
        else if (PercentHP >= 0.5f)
        {
            CurrSprite.sprite = States[1];
        }
        else
        {
            CurrSprite.sprite = States[2];
        }

    }
}
