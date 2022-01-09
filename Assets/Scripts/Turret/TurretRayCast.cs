using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretRayCast : MonoBehaviour
{
    /// <summary>
    /// How far we can shoot in meters
    /// </summary>
    public float MaxRange = 25f;

    /// <summary>
    /// Where our raycast or projectile will spawn from
    /// </summary>
    [Tooltip("Where our raycast or projectile will start from.")]
    public Transform MuzzlePointTransform;

    [Header("Raycast Options : ")]
    public int validLayer = 8;

    public float firingRate = 0.2f;
    private float lastShotTime;

    public GameObject HitFXPrefab; //{ get; private set; }

    public GameObject flameCollider;

    private void Start()
    {
        //not worth it
        //TODO confirm Y is the length
        //flameCollider.transform.localScale = new Vector3(flameCollider.transform.localScale.x, MaxRange, flameCollider.transform.localScale.z);
    }

    //TODO - remove when done testing
    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == validLayer)
        {
            //TODO - add a outline highlight so player can still see enemy

            ApplyParticleFX(other.transform.position, Quaternion.identity);

            StartCoroutine(DestroyAfterDelay(other.gameObject));
        }

        //depr - not needed right now
        // push object if rigidbody
        /*
        Rigidbody hitRigid = other.attachedRigidbody;
        if (hitRigid != null)
        {
            hitRigid.AddForceAtPosition(BulletImpactForce * MuzzlePointTransform.forward, hit.point);
        }
        */

        // Damage if possible
        /*
        Damageable d = hit.collider.GetComponent<Damageable>();
        if (d)
        {
            d.DealDamage(Damage, hit.point, hit.normal, true, gameObject, hit.collider.gameObject);

            if (onDealtDamageEvent != null)
            {
                onDealtDamageEvent.Invoke(Damage);
            }
        }
        

        // Call event
        if (onRaycastHitEvent != null)
        {
            onRaycastHitEvent.Invoke(hit);
        }
        */
    }

    private IEnumerator DestroyAfterDelay(GameObject hitObj)
    {
        yield return new WaitForSeconds(2);

        RePoolEnemy(hitObj);
    }

    private void RePoolEnemy(GameObject objForRepool)
    {
        objForRepool.SetActive(false);
        objForRepool.transform.position = new Vector3(0, 0, 0);
    }

    public virtual void ApplyParticleFX(Vector3 position, Quaternion rotation)
    {
        if (HitFXPrefab)
        {
            GameObject impact = Instantiate(HitFXPrefab, position, rotation) as GameObject;

            // Attach bullet hole to object if possible
            /*
            BulletHole hole = impact.GetComponent<BulletHole>();
            if (hole)
            {
                hole.TryAttachTo(attachTo);
            }
            */
        }
    }
}
