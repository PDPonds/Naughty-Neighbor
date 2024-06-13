using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Target { Pig, Aunt }

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    CircleCollider2D circleCollider;

    Target targetType;
    Vector3 target;
    [SerializeField] bool isPowerThrow;

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

    public void OnSetupBullet(Vector3 targetPoint, Target targetType)
    {
        target = targetPoint;
        this.targetType = targetType;
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
                    if (isPowerThrow)
                        pigEnemy.TakeDamage(GameManager.gameData.PowerThrow);
                    else
                        pigEnemy.TakeDamage(GameManager.gameData.SmallAttack);
                }
                else
                {
                    PlayerManager pigPlayer = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
                    if (isPowerThrow)
                        pigPlayer.TakeDamage(GameManager.gameData.PowerThrow);
                    else
                        pigPlayer.TakeDamage(GameManager.gameData.SmallAttack);
                }
            }
            else if (collision.CompareTag("PigNormal"))
            {
                AfterHit();
                if (GameManager.IsGameMode(GameMode.SinglePlayer))
                {
                    EnemyManager pigEnemy = GameManager.Instance.RichPig.GetComponent<EnemyManager>();
                    if (isPowerThrow)
                        pigEnemy.TakeDamage(GameManager.gameData.PowerThrow);
                    else
                        pigEnemy.TakeDamage(GameManager.gameData.NormalAttack);
                }
                else
                {
                    PlayerManager pigPlayer = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
                    if (isPowerThrow)
                        pigPlayer.TakeDamage(GameManager.gameData.PowerThrow);
                    else
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
                if (isPowerThrow)
                    auntPlayer.TakeDamage(GameManager.gameData.PowerThrow);
                else
                    auntPlayer.TakeDamage(GameManager.gameData.SmallAttack);
            }
            else if (collision.CompareTag("AuntNormal"))
            {
                AfterHit();
                PlayerManager auntPlayer = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
                if (isPowerThrow)
                    auntPlayer.TakeDamage(GameManager.gameData.PowerThrow);
                else
                    auntPlayer.TakeDamage(GameManager.gameData.NormalAttack);
            }
        }

    }

    void AfterHit()
    {
        circleCollider.isTrigger = false;
        rb.gravityScale = 1;
        rb.AddForce(Vector2.up, ForceMode2D.Impulse);

        if (targetType == Target.Pig)
        {
            PlayerManager auntPlayer = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
            if (auntPlayer.isDoubleAttack)
            {
                auntPlayer.InstantiatBullet(auntPlayer.curBullet);
                auntPlayer.isDoubleAttack = false;
            }
            else
            {
                GameManager.Instance.SwitchToWaitingForNextTurnState(targetType);
            }
        }
        else if (targetType == Target.Aunt)
        {
            if (GameManager.IsGameMode(GameMode.SinglePlayer))
            {
                EnemyManager enemy = GameManager.Instance.RichPig.GetComponent<EnemyManager>();
                if (enemy.isDoubleAttack)
                {
                    enemy.InstantiatBullet(enemy.curBullet);
                    enemy.isDoubleAttack = false;
                }
                else
                {
                    GameManager.Instance.SwitchToWaitingForNextTurnState(targetType);
                }
            }
            else if (GameManager.IsGameMode(GameMode.MultiPlayer))
            {
                PlayerManager pigPlayer = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
                if (pigPlayer.isDoubleAttack)
                {
                    pigPlayer.InstantiatBullet(pigPlayer.curBullet);
                    pigPlayer.isDoubleAttack = false;
                }
                else
                {
                    GameManager.Instance.SwitchToWaitingForNextTurnState(targetType);
                }
            }
        }
    }

}
