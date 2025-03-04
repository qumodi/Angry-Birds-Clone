using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BlockType { None, Wood, Stone, Glass }
public enum BlockSize { Small,Short,Long,Large}

public class Block : MonoBehaviour
{
    //private Enemy BlockStats;
    [SerializeField] private List<Sprite> States;
    [SerializeField] private List<Sprite> HollowStates;
    [SerializeField] private bool IsSolid = true;
    public BlockSize CurrBlockSize;

    private SpriteRenderer CurrSprite;
    public BlockType blockType;

    //[SerializeField] private float MaxHP;

    void Start()
    {
        //BlockStats = GetComponent<Enemy>();
        CurrSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateSprite(float PercentHP)
    {
        if (IsSolid)
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
        else
        {
            if (PercentHP >= 0.5f)
            {
                CurrSprite.sprite = HollowStates[0];
            }
            else
            {
                CurrSprite.sprite = HollowStates[1];
            }
        }

    }

    public float GetHpFoSize(BlockSize size)
    {
        float multiplicator = 4f;

        float smallBlock = 20f * multiplicator;
        float shortBlock = 25f * multiplicator;
        float longBlock = 30f * multiplicator;
        float largeBlock = 40f * multiplicator;

        switch (size)
        {
            case BlockSize.Small:
                return smallBlock;
            case BlockSize.Short:
                return shortBlock;
            case BlockSize.Long:
                return longBlock;
            case BlockSize.Large:
                return largeBlock;
            default:
                return smallBlock;
        }
            
    }
}
