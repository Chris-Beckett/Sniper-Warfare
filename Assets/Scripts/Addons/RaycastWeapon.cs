using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public bool isFiringGun = false;
    public ParticleSystem gunMuzzleFlash;
    public ParticleSystem hitEffect;
    public GameObject bulletHolePrefab;
    private int maxBulletHolesAtOnce = 10;
    public Transform raycastOrigin;
    public Transform raycastDestination;

    GunSystem gunSystem;

    Ray ray;
    RaycastHit hitInfo;
    List<GameObject> bulletHoles = new List<GameObject>();
    int lastIndex;

    // Start is called before the first frame update
    public void StartFiring()
    {
        isFiringGun = true;

        gunMuzzleFlash.Emit(1);

        ray.origin = raycastOrigin.position;
        ray.direction = raycastDestination.position - raycastOrigin.position;

        if (Physics.Raycast(ray, out hitInfo))
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            CreateBulletHole();
        }
    }
    IEnumerator SelfDeactive(GameObject gameObjectA)
    {
        yield return new WaitForSeconds(3f);
        gameObjectA.SetActive(false);
    }

    private void CreateBulletHole()
    {
        if (bulletHoles.Count < maxBulletHolesAtOnce)
        {
            
            GameObject tempBulletHole = Instantiate(bulletHolePrefab, hitInfo.point, Quaternion.LookRotation(-hitInfo.normal));
            
            bulletHoles.Add(tempBulletHole);

            StartCoroutine(SelfDeactive(tempBulletHole));
        }
        else
        {
            bulletHoles[lastIndex].transform.position = hitInfo.point;
            bulletHoles[lastIndex].transform.forward = -hitInfo.normal;

            if(!bulletHoles[lastIndex].activeSelf) {
                bulletHoles[lastIndex].SetActive(true);
                StartCoroutine(SelfDeactive(bulletHoles[lastIndex]));
            }

            if (lastIndex + 1 >= bulletHoles.Count)
                lastIndex = 0;
            else
                lastIndex++;
        }
    }

    public void FinishFiring()
    {
        isFiringGun = false;

        gunMuzzleFlash.Emit(0);
    }
}