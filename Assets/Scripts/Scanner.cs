using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange; //감지 범위(반지름)
    public LayerMask targetLayer; //감지할 대상의 레이어
    public RaycastHit2D[] targets; //감지된 대상들을 저장
    public Transform nearestTarget; //그 중 가장 가까운 대상

    private void FixedUpdate()
    {
        //CircleCastAll(캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상 레이어)
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100; //최소한의 거리

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }
        return result;
    }
}
