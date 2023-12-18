using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TargetDummyAI : MonoBehaviour
{

    private BulletSimple prefabBullet;

    private void Awake()
    {
        prefabBullet = ((GameObject)Resources.Load(Paths.PATH_PREFABS_BULLET_SIMPLE)).GetComponent<BulletSimple>();
    }

    private void Start()
    {
        StartCoroutine(ShootRoutine());
    }

    private void Update()
    {

    }

    // Privates

    private IEnumerator ShootRoutine()
    {
        Shoot();

        yield return new WaitForSeconds(0.01f);
        
        StartCoroutine(ShootRoutine());
    }

    private void Shoot()
	{
        BulletSimple bullet = Instantiate(prefabBullet, BattleBoardManager.Instance.bulletParent);

        bullet.SetUp(2 * UtilMath.GetRandomUnitVector2());
	}
	
}
