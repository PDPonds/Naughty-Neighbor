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
    bool isDoubleAttack;

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

    public void OnSetupBullet(Vector3 targetPoint, Target targetType, bool isDoubleAttack)
    {
        target = targetPoint;
        this.targetType = targetType;
        this.isDoubleAttack = isDoubleAttack;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyManager pigEnemy = GameManager.Instance.RichPig.GetComponent<EnemyManager>();
        PlayerManager pigPlayer = GameManager.Instance.RichPig.GetComponent<PlayerManager>();
        PlayerManager auntPlayer = GameManager.Instance.AuntNextDoor.GetComponent<PlayerManager>();
        if (!GameManager.Instance.IsGameState(GameState.Winner))
        {

            if (collision.CompareTag("Ground"))
            {
                AfterHit();

                if (targetType == Target.Aunt)
                {
                    if (auntPlayer.isHurt()) auntPlayer.Anim_Play("Happy UnFriendly");
                    else auntPlayer.Anim_Play("Happy Friendly");
                    SoundManager.Instance.PlayOneShot("AuntDodge");
                }
                else
                {
                    if (GameManager.IsGameMode(GameMode.SinglePlayer))
                    {
                        if (pigEnemy.isHurt()) pigEnemy.Anim_Play("Happy UnFriendly");
                        else pigEnemy.Anim_Play("Happy Friendly");
                    }
                    else
                    {
                        if (pigPlayer.isHurt()) pigPlayer.Anim_Play("Happy UnFriendly");
                        else pigPlayer.Anim_Play("Happy Friendly");
                    }
                    SoundManager.Instance.PlayOneShot("PigDodge");
                }

                SoundManager.Instance.PlayOneShot("Bounce");
            }

            if (targetType == Target.Pig)
            {
                if (collision.CompareTag("PigSmall"))
                {
                    if (auntPlayer.isHurt()) auntPlayer.Anim_Play("Happy UnFriendly");
                    else auntPlayer.Anim_Play("Happy Friendly");

                    AfterHit();

                    if (GameManager.IsGameMode(GameMode.SinglePlayer))
                    {
                        if (isPowerThrow)
                        {
                            if (pigEnemy.TakeDamage(GameManager.gameData.PowerThrowDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                        else if (isDoubleAttack)
                        {
                            if (pigEnemy.TakeDamage(GameManager.gameData.DoubleAttackDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                        else
                        {
                            if (pigEnemy.TakeDamage(GameManager.gameData.SmallAttackDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                    }
                    else
                    {
                        if (isPowerThrow)
                        {
                            if (pigPlayer.TakeDamage(GameManager.gameData.PowerThrowDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                        else if (isDoubleAttack)
                        {
                            if (pigPlayer.TakeDamage(GameManager.gameData.DoubleAttackDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                        else
                        {
                            if (pigPlayer.TakeDamage(GameManager.gameData.SmallAttackDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                    }

                    SoundManager.Instance.PlayOneShot("Bounce");
                    SoundManager.Instance.PlayOneShot("HitPig");

                }
                else if (collision.CompareTag("PigNormal"))
                {
                    if (auntPlayer.isHurt()) auntPlayer.Anim_Play("Happy UnFriendly");
                    else auntPlayer.Anim_Play("Happy Friendly");

                    AfterHit();

                    if (GameManager.IsGameMode(GameMode.SinglePlayer))
                    {
                        if (isPowerThrow)
                        {
                            if (pigEnemy.TakeDamage(GameManager.gameData.PowerThrowDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                        else if (isDoubleAttack)
                        {
                            if (pigEnemy.TakeDamage(GameManager.gameData.DoubleAttackDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                        else
                        {
                            if (pigEnemy.TakeDamage(GameManager.gameData.NormalAttackDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                    }
                    else
                    {
                        if (isPowerThrow)
                        {
                            if (pigPlayer.TakeDamage(GameManager.gameData.PowerThrowDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                        else if (isDoubleAttack)
                        {
                            if (pigPlayer.TakeDamage(GameManager.gameData.DoubleAttackDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                        else
                        {
                            if (pigPlayer.TakeDamage(GameManager.gameData.NormalAttackDamage))
                            {
                                auntPlayer.Anim_Play("Cheer Friendly");
                            }
                        }
                    }

                    SoundManager.Instance.PlayOneShot("Bounce");
                    SoundManager.Instance.PlayOneShot("HitPig");

                }
            }
            else if (targetType == Target.Aunt)
            {
                if (collision.CompareTag("AuntSmall"))
                {
                    if (GameManager.IsGameMode(GameMode.SinglePlayer))
                    {
                        if (pigEnemy.isHurt()) pigEnemy.Anim_Play("Happy UnFriendly");
                        else pigEnemy.Anim_Play("Happy Friendly");
                    }
                    else
                    {
                        if (pigPlayer.isHurt()) pigPlayer.Anim_Play("Happy UnFriendly");
                        else pigPlayer.Anim_Play("Happy Friendly");
                    }

                    AfterHit();

                    if (isPowerThrow)
                    {
                        if (auntPlayer.TakeDamage(GameManager.gameData.PowerThrowDamage))
                        {
                            SelectPigPlayCheerAnim(pigPlayer, pigEnemy);
                        }
                    }
                    else if (isDoubleAttack)
                    {
                        if (auntPlayer.TakeDamage(GameManager.gameData.DoubleAttackDamage))
                        {
                            SelectPigPlayCheerAnim(pigPlayer, pigEnemy);
                        }
                    }
                    else
                    {
                        if (auntPlayer.TakeDamage(GameManager.gameData.SmallAttackDamage))
                        {
                            SelectPigPlayCheerAnim(pigPlayer, pigEnemy);
                        }
                    }

                    SoundManager.Instance.PlayOneShot("Bounce");
                    SoundManager.Instance.PlayOneShot("HitAunt");

                }
                else if (collision.CompareTag("AuntNormal"))
                {
                    if (GameManager.IsGameMode(GameMode.SinglePlayer))
                    {
                        if (pigEnemy.isHurt()) pigEnemy.Anim_Play("Happy UnFriendly");
                        else pigEnemy.Anim_Play("Happy Friendly");
                    }
                    else
                    {
                        if (pigPlayer.isHurt()) pigPlayer.Anim_Play("Happy UnFriendly");
                        else pigPlayer.Anim_Play("Happy Friendly");
                    }

                    AfterHit();

                    if (isPowerThrow)
                    {
                        if (auntPlayer.TakeDamage(GameManager.gameData.PowerThrowDamage))
                        {
                            SelectPigPlayCheerAnim(pigPlayer, pigEnemy);
                        }
                    }
                    else if (isDoubleAttack)
                    {
                        if (auntPlayer.TakeDamage(GameManager.gameData.DoubleAttackDamage))
                        {
                            SelectPigPlayCheerAnim(pigPlayer, pigEnemy);
                        }
                    }
                    else
                    {
                        if (auntPlayer.TakeDamage(GameManager.gameData.NormalAttackDamage))
                        {
                            SelectPigPlayCheerAnim(pigPlayer, pigEnemy);
                        }
                    }

                    SoundManager.Instance.PlayOneShot("Bounce");
                    SoundManager.Instance.PlayOneShot("HitAunt");

                }
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
                    enemy.InstantiatBullet(enemy.curBullet, enemy.targetDis);
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

    void SelectPigPlayCheerAnim(PlayerManager pigPlayer, EnemyManager pigEnemy)
    {
        if (GameManager.IsGameMode(GameMode.SinglePlayer))
        {
            pigEnemy.Anim_Play("Cheer Friendly");
        }
        else if (GameManager.IsGameMode(GameMode.MultiPlayer))
        {
            pigPlayer.Anim_Play("Cheer Friendly");
        }
    }

}
