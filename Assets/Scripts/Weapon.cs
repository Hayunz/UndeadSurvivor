using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>(); // 부모 오브젝트의 컴포넌트도 가져올 수 있다.
    }

    private void Start()
    {
        Init();
    }
    private void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;

            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        //테스트코드
        if (Input.GetButtonDown("Jump"))
        {
            LevelUP(10, 1);
        }
    }

    public void LevelUP(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if (id == 0)
            Batch();
            
    }
    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150; 
                Batch();

                break;

            default:
                speed = 0.3f; //연사속도를 의미 :적을 수록 많이 발사
                break;
        }
    }

    void Batch()
    {
        for (int index = 0; index < count; index++)
        {
            Transform bullet;

            //기본 오브젝트 먼저 활용하고 모자란 것은 풀링에서 가져오기
            if (index < transform.childCount)
            {
                bullet = transform.GetChild(index);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }

            bullet.parent = transform;

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up *1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);  //-1 is Infinity Per. per은 관통.
        }
    }

    void Fire()
    {
        if(!player.scanner.nearestTarget)
        {
            return;
        }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos-transform.position;
        dir = dir.normalized; //현재 벡터의 방향은 유지하고 크기를 1로 변환된 속성

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir); //지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.GetComponent<Bullet>().Init(damage,  count, dir);

    }

}
