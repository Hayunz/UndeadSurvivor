using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

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
                break;
        }

        //테스트코드
        if (Input.GetButtonDown("Jump"))
        {
            LevelUP(20, 5);
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

            bullet.GetComponent<Bullet>().Init(damage, -1);  //-1 is Infinity Per. per은 관통.
        }
    }

}
