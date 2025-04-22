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
    Animator animator;
    SpriteRenderer sprite;

    private void Awake() //초기화 잊지 말고 하기
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }
    void FixedUpdate() //물리 효과가 적용된 오브젝트를 조정할때 사용
    {
        if (!isLive) //아니라면, 아래거 실행시키지 말고 그냥 이 함수 나가요라는 뜻
            return;
  
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized *speed * Time.fixedDeltaTime; //fixedDeltatime는 프레임 변화가 있어도 일정하게 움직이도록 곱함.
        rigid.MovePosition(rigid.position + nextVec);
        rigid.linearVelocity = Vector2.zero;
    }

    private void LateUpdate() //모든 업데이트 함수가 호출된 후, 마지막으로 호출ㄷ함. 주로 오브젝트를 따라가게 설정한 카메라는 LateUpdate 활용함.
    {
        if (!isLive) //아니라면, 아래거 실행시키지 말고 그냥 이 함수 나가요라는 뜻
            return;
        sprite.flipX=target.position.x < rigid.position.x; //목표의 x축 값과 자신의 x축 값을 비교하여 작으면 true가 되도록 설정
    }

    private void OnEnable() // 스크립트 활성화될때 호출되는 이벤트 함수.
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
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
        if (!collision.CompareTag("Bullet"))
            return;
        health -= collision.GetComponent<Bullet>().damage;

        if (health > 0)
        {
            //live, HitAction
        }

        else
        {
            //..die
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false); //오브젝트 풀링을 쓰고 있기 때문에 비활성화 한다.
    }
}
