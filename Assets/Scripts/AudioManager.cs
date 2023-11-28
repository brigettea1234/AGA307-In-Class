using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    public AudioClip[] enemyHitSound;
    public AudioClip[] enemyDieSound;
    public AudioClip[] footsteps;
    public AudioClip[] enemyAttackSound;

    public AudioClip GetEnemyHitSound()
    {
        //Gives us a random sound from the enemy hit sounds array
        return enemyHitSound[Random.Range(0, enemyHitSound.Length)];
    }

    public AudioClip GetEnemyDieSound()
    {
        return enemyDieSound[Random.Range(0, enemyDieSound.Length)];

    }

    public AudioClip GetEnemyAttackSound()
    {
        return enemyAttackSound[Random.Range(0, enemyAttackSound.Length)];

    }

    public AudioClip GetEnemyFootstepSound()
    {
        return footsteps[Random.Range(0, footsteps.Length)];

    }


    public void PlaySound(AudioClip _clip, AudioSource _source, float _volume = 1)
    {
        //Plays audio clip with adjusted pitch values
        if (_source == null || _clip == null)
            return;

        _source.clip = _clip;
        //Get a random pitch for the enemy hit sounds between 0.8 and 1.2
        _source.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        _source.volume = _volume;
        _source.Play();
    }

}
