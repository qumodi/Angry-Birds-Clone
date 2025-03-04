using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private GameObject FloatNumber;
    [SerializeField] private AudioClip DeathSound;
    [SerializeField] private AudioSource Source;
    [SerializeField] private float maxHP = 20f;

    [SerializeField] private Block BlockStats;
    [SerializeField] private Pig PigStats;

    private float damageTreshoald = 0.8f;
    public float currHP;
    private float birdMultiplier = 2f;
    private float blockMultiplier = 2f;
    private float sameTypeMultiplier = 5f;
    private bool UseStandartDeathSound = true;

    private const int PigDeathScore = 500;

    private void Awake()
    {
        if (this.gameObject.CompareTag("Block"))
        {
            BlockStats = GetComponent<Block>();
        }
        if (this.gameObject.CompareTag("Pig"))
        {
            PigStats = GetComponent<Pig>();
        }
        Source = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (this.gameObject.CompareTag("Block"))
        {
            maxHP = BlockStats.GetHpFoSize(BlockStats.CurrBlockSize);
        }else if (this.gameObject.CompareTag("Pig"))
        {
            maxHP = PigStats.GetHpFoSize(PigStats.CurrPigSize);
        }

        currHP = maxHP;

    }

    public void Damage(float dmg)
    {
        
        currHP -= dmg;

        if (dmg < 1)
        {
            return;
        }

        if (currHP <= 0)
        {
            Die();
        }
    }

    public void DamageBlock(float dmg, BlockType birdBlockType = BlockType.None)
    {

        if (BlockStats.blockType == birdBlockType)
        {
            dmg *= sameTypeMultiplier;
        }

        currHP -= dmg;

        float percent = currHP / maxHP;
        //Debug.Log("Block percent HP : " + percent);
        //Debug.Log(currHP);

        BlockStats.UpdateSprite(percent);

        if (currHP <= 0)
        {
            Die();
        }
        else if(dmg >= 1)
        {
            GameManager.Instance.AddScore((int)dmg);
            GameManager.Instance.CreateDamageParticle(transform.position, (int)dmg);
        }
        //Debug.Log("Block Damage : " + dmg);

    }

    public void DamagePig(float dmg)
    {
        currHP -= dmg;

        float percent = currHP / maxHP;
        //Debug.Log("Block percent HP : " + percent);
        //Debug.Log(currHP);

        PigStats.UpdateSprite(percent);

        if (currHP <= 0)
        {
            Die();
        }
        else if(dmg >= 1)
        {
            GameManager.Instance.AddScore((int)dmg);
            GameManager.Instance.CreateDamageParticle(transform.position, (int)dmg);
        }

       
    }

    public void Die()
    {
        if (this.gameObject.CompareTag("Block"))
        {
            CreateDeathParticle();
        }
        else if(this.gameObject.CompareTag("Pig"))
        {
            CreateStandartDeathParticle();
            GameManager.Instance.CreateLongDamageParticle(transform.position, PigDeathScore );//*GameManager.Instance.scoreMultiplicator
            GameManager.Instance.AddScore(PigDeathScore);
        }
        
        GameManager.Instance.RemoveEnemy(this);
        int score = (int) maxHP;

        //GameManager.Instance.CreateDamageParticle(transform.position,score);
        //                     TextMeshProUGUI
        //GameManager.Instance.AddScore(score);

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        float impactVelocity = coll.relativeVelocity.magnitude;
        //Debug.Log(impactVelocity);
        if (coll.gameObject.CompareTag("Bird"))
        {
            impactVelocity *= birdMultiplier;
        }
        if (coll.gameObject.CompareTag("Block"))
        {
            impactVelocity *= blockMultiplier;
        }
        if (impactVelocity > damageTreshoald)
        {
            if (this.gameObject.CompareTag("Block"))//&& coll.gameObject.CompareTag("Bird")
            {
                if (coll.gameObject.CompareTag("Bird"))
                {
                    DamageBlock(impactVelocity, coll.gameObject.GetComponent<Bird>().BirdBlockType);
                }
                else
                {
                    DamageBlock(impactVelocity);
                }
                //Debug.Log("Sprite Updatet");
                if (BlockStats.blockType == BlockType.Wood)
                {
                    SoundManager.Instance.PlayRandomWoodClip(Source);
                }else if(BlockStats.blockType == BlockType.Glass)
                {
                    SoundManager.Instance.PlayRandomGlassClip(Source);
                }else if (BlockStats.blockType == BlockType.Stone)
                {
                    SoundManager.Instance.PlayRandomStoneClip(Source);
                }
            }
            else if(this.gameObject.CompareTag("Pig"))
            {
                DamagePig(impactVelocity);
            }
            else
            {

                Damage(impactVelocity);
            }
        }

        if (coll.gameObject.CompareTag("Bird"))
        {

        }

    }

    private void CreateDeathParticle()
    {
        Instantiate(deathParticle, this.transform.position, Quaternion.identity);
        if (UseStandartDeathSound)
        {
            AudioSource.PlayClipAtPoint(SoundManager.Instance.StandartDeathSound, transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(DeathSound, transform.position);
        }
    }

    private void CreateStandartDeathParticle()
    {
        Instantiate(deathParticle, this.transform.position, Quaternion.identity);
        if (UseStandartDeathSound)
        {
            AudioSource.PlayClipAtPoint(SoundManager.Instance.StandartDeathSound, transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(DeathSound, transform.position);
        }
    }


}
