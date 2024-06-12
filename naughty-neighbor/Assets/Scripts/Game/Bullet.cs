using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target { Pig, Aunt }

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D circleCollider;

    [SerializeField] Target targetType;
    Vector3 target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

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
            AfterHit();
        }

        if (targetType == Target.Pig)
        {
            if (collision.CompareTag("PigSmall"))
            {
                AfterHit();
                if (GameManager.IsGameMode(GameMode.SinglePlayer))
                {
                    EnemyManager pigEnemy = GameManager.Instance.RichPig.GetComponent<EnemyManager>();
                    pigEnemy.TakeDamage(GameManager.gameData.SmallAttack);
                }
                else
                {
                    PlayerManager pigPlayer = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
                    pigPlayer.TakeDamage(GameManager.gameData.SmallAttack);
                }
            }
            else if (collision.CompareTag("PigNormal"))
            {
                AfterHit();
                if (GameManager.IsGameMode(GameMode.SinglePlayer))
                {
                    EnemyManager pigEnemy = GameManager.Instance.RichPig.GetComponent<EnemyManager>();
                    pigEnemy.TakeDamage(GameManager.gameData.NormalAttack);
                }
                else
                {
                    PlayerManager pigPlayer = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
                    pigPlayer.TakeDamage(GameManager.gameData.NormalAttack);
                }
            }
        }
        else if (targetType == Target.Aunt)
        {
            if (collision.CompareTag("AuntSmall"))
            {
                AfterHit();
                PlayerManager auntPlayer = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
                auntPlayer.TakeDamage(GameManager.gameData.SmallAttack);
            }
            else if (collision.CompareTag("AuntNormal"))
            {
                AfterHit();
                PlayerManager auntPlayer = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
                auntPlayer.TakeDamage(GameManager.gameData.NormalAttack);
            }
        }

    }

    void AfterHit()
    {
        circleCollider.isTrigger = false;
        rb.gravityScale = 1;
        GameManager.Instance.SwitchToWaitingForNextTurnState(targetType);
    }

}
