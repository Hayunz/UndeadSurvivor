using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Init(float damage, int per, Vector3 dir)
    {
           this.damage = damage;
           this.per = per;

        if(per > -1)
        {
            rigid.linearVelocity = dir * 15f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || per == -1)
        {
            return;
        }


        per--;

        if (per == -1)
        {
            rigid.linearVelocity = Vector2.zero; //재활용할거기 때문에 초기화해야함.
            gameObject.SetActive(false);
        }
    }
}
