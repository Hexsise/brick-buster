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

    public AudioClip[] collisionSounds;
    private AudioSource audioSource;

    private void Awake()
    {
        // lower-level access in Collectable
        this.sr = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        Brick.OnBrickDestruction += PlayBrickDestructionSFX;
    }

    private void PlayBrickDestructionSFX(Brick brick)
    {
        PlaySound(2);
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

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Paddle"))
        {
            PlaySound(0);
        }
        else if (coll.gameObject.CompareTag("Wall"))
        {
            PlaySound(0);
        }
        else if (coll.gameObject.CompareTag("Ball"))
        {
            PlaySound(1);
        }
    }

    private void PlaySound(int soundIndex)
    {
        if (audioSource != null && soundIndex >= 0 && soundIndex < collisionSounds.Length)
        {
            BallsManager ballsManager = BallsManager.Instance; // Get the BallsManager instance
            if (ballsManager != null)
            {
                int activeBallCount = ballsManager.GetActiveBallCount();
                // Calculate the volume adjustment based on the number of active ball prefabs (current workaround to volume bug)
                float volumeAdjustment = Mathf.Max(0.5f, 1f / (activeBallCount == 0 ? 1 : activeBallCount));

                if (!audioSource.enabled)
                {
                    audioSource.enabled = true; // added check to address warning on non-enabled audioSource
                    audioSource.clip = collisionSounds[soundIndex];
                    audioSource.Play();
                    audioSource.enabled = false;
                }
                else
                {
                    audioSource.volume = volumeAdjustment; // Adjust the volume
                    audioSource.clip = collisionSounds[soundIndex];
                    audioSource.Play();
                }
            }
        }
    }

    private void OnDestroy()
    {
        Brick.OnBrickDestruction -= PlayBrickDestructionSFX;
    }

    private void OnDisable()
    {
        Brick.OnBrickDestruction -= PlayBrickDestructionSFX;
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