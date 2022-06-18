using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    public bool isFiringGun = false;
    public ParticleSystem gunMuzzleFlash;
    public ParticleSystem hitEffect;

    public GameObject selectedBulletHole;
    public GameObject bulletHolePrefab;

    public GameObject bulletHoleBloodPrefab;
    public GameObject bulletHoleMetalPrefab;
    public GameObject bulletHoleWoodPrefab;
    public GameObject bulletHoleGroundPrefab;

    private int maxBulletHolesAtOnce = 10;
    public Transform raycastOrigin;
    public Transform raycastDestination;

    GunSystem gunSystem;

    Ray ray;
    RaycastHit hitInfo;
    List<GameObject> bulletHoles = new List<GameObject>();
    int lastIndex;

    private string currentTargetType = "ground";

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

        if (gameObjectA != null)
            gameObjectA.SetActive(false);
    }

    private void selectBulletHoleEffect(RaycastHit hitObject)
    {

        var multiTag = hitObject.collider.GetComponent<CustomTags>();

        if (multiTag == null)
        {
            selectedBulletHole = bulletHolePrefab;
            currentTargetType = "ground";
        }
        else
        {
            if (multiTag.HasTag("WoodenTarget"))
            {
                selectedBulletHole = bulletHoleWoodPrefab;
                currentTargetType = "wood";
            }
        }
    }

    private string returnBulletType(GameObject bullet)
    {
        var multiTag = bullet.GetComponent<CustomTags>();

        if (multiTag == null)
        {
            return "ground";
        }
        else
        {
            if (multiTag.HasTag("wood"))
            {
                return "wood";
            }
        }

        return "ground";
    }

    private void CreateBulletHole()
    {
        selectBulletHoleEffect(hitInfo);

        if (bulletHoles.Count < maxBulletHolesAtOnce)
        {
            CreateNewBulletHole();
        }
        else
        {
            if (returnBulletType(bulletHoles[lastIndex]) != currentTargetType)
            {
                DestroyImmediate(bulletHoles[lastIndex], true);
                bulletHoles.RemoveAt(lastIndex);

                CreateNewBulletHole();
            }
            else
            {

                bulletHoles[lastIndex].transform.position = hitInfo.point;
                bulletHoles[lastIndex].transform.forward = -hitInfo.normal;

                if (!bulletHoles[lastIndex].activeSelf)
                {
                    bulletHoles[lastIndex].SetActive(true);
                    StartCoroutine(SelfDeactive(bulletHoles[lastIndex]));
                }

                if (lastIndex + 1 >= bulletHoles.Count)
                    lastIndex = 0;
                else
                    lastIndex++;
            }
        }
    }

    private void CreateNewBulletHole()
    {
        GameObject tempBulletHole = Instantiate(selectedBulletHole, hitInfo.point, Quaternion.LookRotation(-hitInfo.normal));

        bulletHoles.Add(tempBulletHole);

        StartCoroutine(SelfDeactive(tempBulletHole));

        lastIndex = 0;
    }

    public void FinishFiring()
    {
        isFiringGun = false;

        gunMuzzleFlash.Emit(0);
    }
}