using System.Collections;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animatorCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator animator;
    SpriteRenderer sprite;
    WaitForFixedUpdate wait;

    private void Awake() //초기화 잊지 말고 하기
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }
    void Start()
    {
        
    }
    void FixedUpdate() //물리 효과가 적용된 오브젝트를 조정할때 사용
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) //아니라면, 아래거 실행시키지 말고 그냥 이 함수 나가요라는 뜻
            return;
  
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized *speed * Time.fixedDeltaTime; //fixedDeltatime는 프레임 변화가 있어도 일정하게 움직이도록 곱함.
        rigid.MovePosition(rigid.position + nextVec);
        rigid.linearVelocity = Vector2.zero;
    }

    private void LateUpdate() //모든 업데이트 함수가 호출된 후, 마지막으로 호출ㄷ함. 주로 오브젝트를 따라가게 설정한 카메라는 LateUpdate 활용함.
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }

        if (!isLive) //아니라면, 아래거 실행시키지 말고 그냥 이 함수 나가요라는 뜻
            return;
        sprite.flipX=target.position.x < rigid.position.x; //목표의 x축 값과 자신의 x축 값을 비교하여 작으면 true가 되도록 설정
    }

    private void OnEnable() // 스크립트 활성화될때 호출되는 이벤트 함수.
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        sprite.sortingOrder = 2;
        animator.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData spawnData)
    {
        animator.runtimeAnimatorController = animatorCon[spawnData.spriteType];
        speed= spawnData.speed;
        maxHealth = spawnData.health;
        health = spawnData.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;
        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            //live, HitAction
            animator.SetTrigger("Hit");
        }

        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            sprite.sortingOrder = 1;
            animator.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait; //1프레임 쉬기, 하나의 물리 프레임을 딜레이 주기
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);



        //yield return new WaitForSeconds(2f); // 2초 쉬기 최적화때 안좋을 수 있어서 보통 변수로 만든다.
    }

    void Dead()
    {
        gameObject.SetActive(false); //오브젝트 풀링을 쓰고 있기 때문에 비활성화 한다.
    }
}
