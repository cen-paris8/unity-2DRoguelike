using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject
{

    public int playerDamage;
    public AudioClip EnemyAttack1;
    public AudioClip EnemyAttack2;

    private Animator animator;
    private Transform target;
    private bool skipmove;

    // Start is called before the first frame update
    protected override void Start()
    {
        GameManager.instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
       if (skipmove)
        {
            skipmove = false;
            return;
        }

        base.AttemptMove<T>(xDir, yDir);

        skipmove = true;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            yDir = target.position.y > transform.position.y ? 1 : -1;
        else
            xDir = target.position.x > transform.position.x ? 1 : -1;

        AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCantMove <T> (T componenent)
    {
        Player hitPlayer = componenent as Player;

        hitPlayer.LoseFood(playerDamage);

        animator.SetTrigger("enemyAttack");
        SoundManager.instance.RandomizeSfx(EnemyAttack1, EnemyAttack2);
        
    }
}
