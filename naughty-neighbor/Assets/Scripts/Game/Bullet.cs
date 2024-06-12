using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target { Pig, Aunt }

public class Bullet : MonoBehaviour
{
    [SerializeField] Target targetType;
    Vector3 target;

    private void Start()
    {
        StartCoroutine(Curve(transform.position, target));
    }

    IEnumerator Curve(Vector3 start, Vector2 target)
    {
        float timePassed = 0f;
        Vector2 end = target;
        while (timePassed < GameManager.gameData.ShootDuration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / GameManager.gameData.ShootDuration;
            float heightT = GameManager.Instance.trajectoryAnimationCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, GameManager.gameData.TrajectoryMaxHeight, heightT);
            transform.position = Vector2.Lerp(start, end, linearT) + new Vector2(0f, height);
            yield return null;
        }
    }

    public void OnSetupBullet(Vector3 target)
    {
        this.target = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Debug.Log("Ground");
            Destroy(gameObject);
        }

        if (targetType == Target.Pig)
        {
            if (collision.CompareTag("PigSmall"))
            {
                Debug.Log("PigSmall");
                Destroy(gameObject);

            }
            else if (collision.CompareTag("PigNormal"))
            {
                Debug.Log("PigNormal");
                Destroy(gameObject);

            }
        }
        else if (targetType == Target.Aunt)
        {
            if (collision.CompareTag("AuntSmall"))
            {
                Debug.Log("AuntSmall");
                Destroy(gameObject);

            }
            else if (collision.CompareTag("AuntNormal"))
            {
                Debug.Log("AuntNormal");
                Destroy(gameObject);

            }
        }

    }

}
