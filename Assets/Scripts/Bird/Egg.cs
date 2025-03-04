using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    [SerializeField] private ParticleSystem DeathParticle;

    [SerializeField] private float explotionRange;
    [SerializeField] private float explotionPower;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude >= 0.1f)
        {
            if (collision.gameObject.CompareTag("Bird"))
            {
                Bird bird = collision.gameObject.GetComponent<Bird>();
                if (!bird.flying)
                {
                    Explode();
                }
            }
            else
            {
                Explode();
            }
        }

    }
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

        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].gameObject.GetComponent<Enemy>())
            {
                //float distance

                expDir = cols[i].transform.position - this.transform.position;
                cols[i].GetComponent<Rigidbody2D>().velocity = expDir.normalized * explotionPower;
            }

        }

        Die();

        CameraManager.Instance.SwitchToIdle();
    }
    public void Die()
    {
        Destroy(this.gameObject);
        CreateEggDeathParticle();

    }
    private void CreateEggDeathParticle()
    {
        var main = DeathParticle.main;
        main.startSizeMultiplier = explotionRange;

        Instantiate(DeathParticle, this.transform.position, Quaternion.identity);
        main.startSizeMultiplier = 1;
    }
}
