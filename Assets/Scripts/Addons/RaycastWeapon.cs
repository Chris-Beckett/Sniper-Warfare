using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public bool isFiringGun = false;
    public ParticleSystem gunMuzzleFlash;
    public ParticleSystem hitEffect;
    public Transform raycastOrigin;
    public Transform raycastDestination;
    

    Ray ray;
    RaycastHit hitInfo;

    // Start is called before the first frame update
    public void StartFiring()
    {
        isFiringGun = true;

        gunMuzzleFlash.Emit(1);

        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position - raycastOrigin.position;

        if (Physics.Raycast(ray, out hitInfo)) {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);
        }
    }

    public void FinishFiring() {
        isFiringGun = false;

        gunMuzzleFlash.Emit(0);
    }
}
