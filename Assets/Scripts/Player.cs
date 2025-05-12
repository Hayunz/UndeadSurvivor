using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float speed;
    public Scanner scanner;
    public Hand[] hands;
    public RuntimeAnimatorController[] anicontroller;

    public Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);  //���� �� true�� ������ ��Ȱ��ȭ �� ������Ʈ�� OK
    }

    private void OnEnable()
    {
        speed *= Character.Speed;
        animator.runtimeAnimatorController = anicontroller[GameManager.instance.playerId];
    }

    private void Update()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        inputVec.x=Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }



    private void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
        {
            return;
        }
        Vector2 nextVec = inputVec.normalized * Time.fixedDeltaTime*speed;
            rigid.MovePosition (rigid.position + nextVec);
            
    }

    private void LateUpdate()
    {
        animator.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriteRenderer.flipX = inputVec.x < 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.instance.isLive)
            return;

        GameManager.instance.health -= Time.deltaTime * 10;

        if (GameManager.instance.health < 0)
        {
            for (int index = 2; index < transform.childCount; index++) //childcount : �ڽ� ������Ʈ�� ����
            {
                {
                    transform.GetChild(index).gameObject.SetActive(false);
                }

            }
            animator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }


    }

}
