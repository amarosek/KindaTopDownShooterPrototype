using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitEffect;
    [SerializeField] Ammo ammoSlot;
    [SerializeField] AmmoType ammoType;
    [SerializeField] float range = 100f;
    [SerializeField] float damage = 20f;
    [SerializeField] float timeBetweenShoots = 0.3f;

    bool canShoot = true;


    void Update()
    {
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;
        if (ammoSlot.GetCurrentAmmo(ammoType) > 0)
        {
            ProcessRaycast();
            PlayMuzzleFlash();
            ammoSlot.ReduceCurrentAmmo(ammoType);
        }

        yield return new WaitForSeconds(timeBetweenShoots);
        canShoot = true;
    }

    private void ProcessRaycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(player.transform.position.x, 1, player.transform.position.z), transform.forward, out hit, range))
        {
            PlayHitEffect(hit);
            EnemyHealth target = hit.transform.GetComponent<EnemyHealth>();
            if (target == null) return;
            target.TakeDamage(damage);
        }
        else
        {
            return;
        }
    }

    private void PlayMuzzleFlash()
    {
        muzzleFlash.Play();
    }

    private void PlayHitEffect(RaycastHit hit)
    {
        GameObject impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impact, 0.1f);
    }
}
