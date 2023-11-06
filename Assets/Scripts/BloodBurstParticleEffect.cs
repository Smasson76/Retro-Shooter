using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBurstParticleEffect : MonoBehaviour
{
	public ParticleSystem bloodBurstEffect;
	float startTime;
	public int endTime = 5;

    // Start is called before the first frame update
    void Start()
    {
    	bloodBurstEffect = GetComponentInChildren<ParticleSystem>();
		bloodBurstEffect.Play();

		startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
    	float currentTime = Time.time;
		if ((currentTime - startTime) > endTime){
			Destroy(gameObject);
    	}
	}
}
