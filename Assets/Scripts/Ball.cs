using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isLightningBall; 
    private SpriteRenderer sr;

    public ParticleSystem lightningBallEffect;

    public float lightningBallDuration = 10f;

    public float lightningTime;

    public static event Action<Ball> OnBallDeath;
    public static event Action<Ball> OnLightningBallEnable;
    public static event Action<Ball> OnLightningBallDisable;

    private void Awake()
    {
        // lower-level access in Collectable
        this.sr = GetComponentInChildren<SpriteRenderer>();
    }

    public void Die()
    {
        OnBallDeath?.Invoke(this);
        // second paramter is time-delay
        Destroy(gameObject, 1);
    }

    
    public void StartLightningBall()
    {
        if (isLightningBall)
        {
            // reset duration if player gets another lightningBall collectable
            lightningTime = lightningBallDuration;
        }

        if (!this.isLightningBall) // if not lightning ball, become one
        {
            this.isLightningBall = true;
            this.sr.enabled = false;
            lightningBallEffect.gameObject.SetActive(true);
            StartCoroutine(StopLightningBallAfterTime());

            OnLightningBallEnable?.Invoke(this);
        }
    }


    private IEnumerator StopLightningBallAfterTime()
    {
        // loop where I can track remaining time for lightningEffect
        for (lightningTime = lightningBallDuration; lightningTime > 0; lightningTime -= Time.deltaTime)
        {
            yield return null;
        }

        StopLightningBall();
    }

    private void StopLightningBall()
    {
        if (this.isLightningBall)
        {
            this.isLightningBall = false;
            this.sr.enabled = true;
            lightningBallEffect.gameObject.SetActive(false);

            OnLightningBallDisable?.Invoke(this);
        }
    }
}





/*
public IEnumerator StartLightningBall()
{
    if (this.isLightningBall)
    {
        lightningBallTimeLeftt += 10;
    }
    if (!this.isLightningBall) // if not lightning ball, become one
    {
        this.isLightningBall = true;
        this.sr.enabled = false;
        lightningBallEffect.gameObject.SetActive(true);

        for (lightningBallTimeLeftt = 10; lightningBallTimeLeftt > 0; lightningBallTimeLeftt -= Time.deltaTime)
        {
            yield return null;
        }
        StopLightningBall();
    }
}
*/


/*
private IEnumerator StopLightningBallAfterTime(float seconds)
{
    yield return new WaitForSeconds(seconds);

    StopLightningBall();
}
*/