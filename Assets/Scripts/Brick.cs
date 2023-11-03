using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    public int Hitpoints = 1;
    public ParticleSystem DestroyEffect;

    public static event Action<Brick> OnBrickDestruction;

    private void Awake()
    {
        this.sr = this.GetComponent<SpriteRenderer>();
        this.boxCol = this.GetComponent<BoxCollider2D>();
        Ball.OnLightningBallEnable += OnLightningBallEnable;
        Ball.OnLightningBallDisable += OnLightningBallDisable;

    }

    private void OnLightningBallDisable(Ball ball)
    {
        if (this != null)
        {
            this.boxCol.isTrigger = false;
        }
    }

    private void OnLightningBallEnable(Ball ball)
    {
       if (this != null)
        {
            this.boxCol.isTrigger = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }

    // need this method for lightningBallEffect since it triggers on bricks rather than colliding
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.boxCol.isTrigger) // legal reference unlike my previous one (isLightningBall)
        {
            Ball ball = collision.gameObject.GetComponent<Ball>();

            // we never want Collectables to interact with bricks
            if (collision.tag != "Collectable")
            { 
                ApplyCollisionLogic(ball);  
            }
            
        }
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        this.Hitpoints--;

        if(this.Hitpoints <= 0 || (ball != null && ball.isLightningBall))
        {
            BricksManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            // relates to buffs/debuffs
            OnBrickDestroy();

            SpawnDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            this.sr.sprite = BricksManager.Instance.Sprites[this.Hitpoints - 1];
        }
    }

    private void OnBrickDestroy()
    {
        float buffSpawnChance = UnityEngine.Random.Range(0, 100f);
        float debuffSpawnChance = UnityEngine.Random.Range(0, 100f);
        bool alreadySpawned = false;

        
        if (buffSpawnChance <= CollectablesManager.Instance.BuffChance)
        {
            alreadySpawned = true;
            Collectable newbuff = this.SpawnCollectable(true);
        }

        if (debuffSpawnChance <= CollectablesManager.Instance.DebuffChance && !alreadySpawned)
        {
            Collectable newDebuff = this.SpawnCollectable(false);
        }
    }

    private Collectable SpawnCollectable(bool isBuff)
    {
        List<Collectable> collection;

        if (isBuff)
        {
            collection = CollectablesManager.Instance.AvailableBuffs;
        }
        else
        {
            collection = CollectablesManager.Instance.AvailableDebuffs;
        }

        int buffIndex = UnityEngine.Random.Range(0, collection.Count);
        Collectable prefab = collection[buffIndex];
        Collectable newCollectable = Instantiate(prefab, this.transform.position, Quaternion.identity) as Collectable;

        return newCollectable;
    }

    // ctrl + '.' + enter to quickly make a method
    private void SpawnDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        // Quaternion.identity is a default rotation setting
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);

    }
    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.Hitpoints = hitpoints;
    }
    // always unsubscribe from events when done
    private void OnDisable()
    {
        Ball.OnLightningBallEnable -= OnLightningBallEnable;
        Ball.OnLightningBallDisable -= OnLightningBallDisable;
    }

}



/* LIST OF CURRENT BUGS 12/16 (updated 12/21)
 * 
 * 1) [FIXED] Buffs deal 1 hitpoint dmg to bricks as they fall because of OnTrigger2D
 * --> Can make child objects for colliding and triggering
 * --> Sol: If statement in isTrigger2D bricks method
 * --> (12/21) need to reference the bool field without causing nullreference exception.
 * --> Buffs still deal dmg to bricks if lightningBall is in effect
 * --> [Fixed] collison logic will never apply for bricks 
 * 
 * 2) [FIXED] Lighting balls that pick up multi-balls in the middle of the duration lose their effect
 * --> Can invoke StartLightningBall and pass the duration parameter where the duration 
 * --> parameter must equal the remaining time left of the current active lightning ball.
 * --> (12/23) [FIXED] Passed Ball object down to sync lightningBallDuration with lightningTime
 * 
 * 3) [FIXED] Bug where falling collectables persist between levels.
 * --> Only if the last brick destroyed spawns a collectible, otherwise fixed.
 * --> (12/21) Can keep a list of buffs inPlay and destroy from list
 * --> [FIXED] Used the Update() method which is periodically called to check if game wasn't started,
 *     if the game wasn't started, call Die().
 * 
 * 4) [FIXED] Lightning ball visual persists even when effect is done
 * 
 * 5) [TODO] Some bug with animatePaddle method that makes it confused when getting successive
 *           extend or shrink calls before the duration ends.
 * 
 * 
 * 
 */ 
