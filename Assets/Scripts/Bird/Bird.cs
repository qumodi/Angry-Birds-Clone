using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum BirdAbility { None, Acceleration, Bomb, Multiplication,LayingEgg }
public class Bird : MonoBehaviour
{
    [SerializeField] private ParticleSystem DeathParticle;
    [SerializeField] private AudioSource SoundSource;
    [SerializeField] private AudioClip CollisionClip;
    [SerializeField] private AudioClip LaunchSound;
    [SerializeField] private SpriteRenderer Renderer;
    [SerializeField] private List<Sprite> BombSprites;
    [SerializeField] private Egg EggPrefab;

    private Rigidbody2D Rb;
    private Collider2D Coll;

    public BlockType BirdBlockType;
    public BirdAbility CurrBirdAbility;

    public bool OnSlingshot = false;//{ get; private set; } 
    public bool flying = false;
    public bool wasShot = false;
    private bool dieActivated = false;

    private bool activeBombTimer;
    private float explotionTime = 2f;
    private float currExplotionTime;

    //ability values
    [SerializeField] private float accelerationPower;
    [SerializeField] private float explotionRange;
    [SerializeField] private float explotionPower;
    [SerializeField] private float LayingEggJumpPower;
    [SerializeField] private float multiplicationOffset = 15; //in degrees 
    public bool CanMultiplicate = true;
    public bool Original = true;

    [SerializeField] private Sprite ThickSprite;
    //Animations
    [SerializeField] private AnimationCurve JumpCurve;

    private void Awake()
    {
        //Time.timeScale = 0.05f;
        Rb = GetComponent<Rigidbody2D>();
        Coll = GetComponent<Collider2D>();
        Renderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        if (Original)
        {
            Rb.isKinematic = true;
            Coll.enabled = false;
        }

        if (Original && !OnSlingshot && !flying)
        {
            float jumpTime = Random.Range(0.5f, 1);
            //StartCoroutine(Jump(jumpTime));
            StartCoroutine(JumpAfterSeconds(jumpTime));
        }
    }

 
    private void Update()
    {
        if (Original)
        {
            UpdateBombTimer();

            if (flying && CurrBirdAbility != BirdAbility.Bomb)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    UseAbility();
                }
            }
            if (!OnSlingshot && CurrBirdAbility == BirdAbility.Bomb && wasShot)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Explode();
                }
            }

            if (CurrBirdAbility != BirdAbility.Bomb && !dieActivated)
            {
                CheckIfStop();
            }
        }

    }

    private void FixedUpdate()
    {
        if (!OnSlingshot && flying)
        {
            transform.right = Rb.velocity.normalized;
        }
    }

    public void LaunchBird(Vector3 launchVector, float force)
    {
        Rb.isKinematic = false;
        Coll.enabled = true;

        Rb.AddForce(launchVector * force, ForceMode2D.Impulse);
        OnSlingshot = false;
        flying = true;
        wasShot = true;
    }

    public void LaunchBird(Vector3 velocityVector)
    {
        Rb.isKinematic = false;
        Coll.enabled = true;
        OnSlingshot = false;
        flying = true;

        Rb.velocity = velocityVector;
        wasShot = true;

        Debug.Log("Multiplication was used");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //SoundManager.Instance.PlayClip(CollisionClip, SoundSource);
        CameraManager.Instance.SwitchToIdle();
        
        if (CurrBirdAbility == BirdAbility.Bomb && flying)
        {
            UseAbility();
        }
        flying = false;
        //Destroy(this);
    }

    private void UseAbility()
    {
        if (CurrBirdAbility == BirdAbility.Acceleration)
        {
            Rb.velocity *= accelerationPower;
        }
        else if (CurrBirdAbility == BirdAbility.Bomb)
        {
            activeBombTimer = true;
        }else if (CurrBirdAbility == BirdAbility.Multiplication)
        {
            if (CanMultiplicate)
            {
                UseMultiplication();
            }
        }else if(CurrBirdAbility == BirdAbility.LayingEgg)
        {
            LayEgg();
            MakeSpriteThick();
            MakeUpJump();
        }
    }
    //Bomb
    private void Explode()
    {
        Debug.Log("Darkness blacker than black and darker than dark,\r\n" +
            "I beseech thee, combine with my deep crimson.\r\n" +
            "The time of awakening cometh.\r\n" +
            "Justice, fallen upon the infallible boundary,\r\n" +
            "appear now as an intangible distortions!\r\n" +
            "I desire for my torrent of power a destructive force:\r\n" +
            "a destructive force without equal!\r\n" +
            "Return all creation to cinders,\r\n" +
            "and come frome the abyss!\r\n" +
            "Explosion!");
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explotionRange);
        Vector2 expDir;

        for(int i = 0; i < cols.Length; i++)
        {
            //if (cols[i].gameObject.CompareTag("Block"))
            //{
            //    expDir = cols[i].transform.position - this.transform.position;
            //    cols[i].GetComponent<Rigidbody2D>().velocity = expDir.normalized * explotionPower;
            //}
            if (cols[i].gameObject.GetComponent<Enemy>())
            {
                expDir = cols[i].transform.position - this.transform.position;
                cols[i].GetComponent<Rigidbody2D>().velocity = expDir.normalized * explotionPower;
            }
            
        }

        activeBombTimer = false;
        CreateBombDeathParticle();
        Die();

        CameraManager.Instance.SwitchToIdle();
    }

    private void UpdateBombTimer()
    {
        if (activeBombTimer)
        {
            currExplotionTime += Time.deltaTime;
        }
        if (activeBombTimer && currExplotionTime >= explotionTime)
        {
            Explode();
            //activeBombTimer = false;
        }

        if (activeBombTimer)
        {
            float percent = currExplotionTime / explotionTime;
            if(percent <= .5f)
            {
                Renderer.sprite = BombSprites[1];
            }else if(currExplotionTime <= 0.9f)
            {
                Renderer.sprite = BombSprites[2];
            }
            //else
            //{
            //    Renderer.sprite = BombSprites[3];

            //}

        }
    }
    // Blue birds
    private void UseMultiplication()
    {
        Vector2 currDir = Rb.velocity;
        for(int i = -1; i < 2; i++)
        {
            if (i == 0) continue;

            Vector2 dupliOffset = Quaternion.AngleAxis(i * multiplicationOffset,Vector3.forward ) * currDir;
            //Vector2 dupliOffset = Quaternion.AngleAxis(-multiplicationOffset, currDir) * Vector2.one;

            Bird dupliBird = Instantiate(this,this.transform.position,Quaternion.identity);
            dupliBird.LaunchBird(dupliOffset);

            CanMultiplicate = false;
            dupliBird.CanMultiplicate = false;
            dupliBird.StopSound();
            dupliBird.Original = false;
        }
        
        //Vector2 firstOffset = Quaternion.AngleAxis(-multiplicationOffset, currDir) * Vector2.one;

        //Debug.Break();
    }

    // Mathilda functions
    private void LayEgg()
    {
        Egg egg = Instantiate(EggPrefab, this.transform.position, Quaternion.identity);
        egg.GetComponent<Rigidbody2D>().velocity += new Vector2(0, -2);
    }
    private void MakeSpriteThick()
    {
        GetComponent<SpriteRenderer>().sprite = ThickSprite;
    }
    private void MakeUpJump()
    {
        //Debug.LogAssertion(Rb.velocity);
        Vector2 newVelocity = new Vector2(LayingEggJumpPower,LayingEggJumpPower);
        Rb.velocity = newVelocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explotionRange);

    }

    private void CheckIfStop()
    {
        if(Rb.velocity.magnitude <= 0.01f && !OnSlingshot && wasShot)
        {
            Invoke(nameof(Die),1f);
            dieActivated = true;
        }
    }
    private void CreateStandartDeathParticle()
    {
        Instantiate(DeathParticle, this.transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(SoundManager.Instance.StandartDeathSound, transform.position);
    }
    private void CreateBombDeathParticle()
    {
        //DeathParticle.startSize = explotionRange;
        var main = DeathParticle.main;
        main.startSizeMultiplier = explotionRange;

        Instantiate(DeathParticle, this.transform.position, Quaternion.identity);
        main.startSizeMultiplier = 1;

        AudioSource.PlayClipAtPoint(SoundManager.Instance.StandartDeathSound, transform.position);

    }

    public void Die()
    {
        Destroy(this.gameObject);
        CreateStandartDeathParticle();

    }

    public IEnumerator Jump(float allTime)
    {
        float timePassed = 0;
        Vector2 startPosition = transform.position;

        float jumpHeight = Random.Range(0.2f, 1.5f);

        //Debug.Log($"Jump Height : {jumpHeight}");

        while (timePassed < allTime) {
            if (OnSlingshot || flying || wasShot)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                yield break;
            }
            //1Debug.Log($"{gameObject.name} | {percent}");
            float percent = timePassed / allTime;
            Vector2 newPos = new Vector2(startPosition.x, startPosition.y + JumpCurve.Evaluate(percent) * jumpHeight);
            transform.position = newPos;

            float angle = 360 * percent;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            timePassed += Time.deltaTime;

            yield return null;
        }

        transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);

        if (!OnSlingshot && !flying & !wasShot)
        {
            float againTime = Random.Range(0.5f, 1);
            float waitTime = Random.Range(1, 5);

            yield return new WaitForSeconds(waitTime);


            StartCoroutine(Jump(againTime));
        }
        else
        {
            yield break;
            //yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator JumpAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        float jumpTime = Random.Range(0.5f, 1);

        StartCoroutine (Jump(jumpTime));
    }

    public void PlayLaunchSound()
    {
        if (Original)
        {
            SoundManager.Instance.PlayClip(LaunchSound, SoundSource);
        }
    }
    public void StopSound()
    {
        SoundSource.Stop();
    }
}
