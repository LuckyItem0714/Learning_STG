using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    public GameObject bulletPrefab; //�G�����e�̃v���n�u
    public float fireRate = 1.5f; //�e�����Ԋu(�b)
    public GameObject explosionPrefab; //�����G�t�F�N�g�̃v���n�u

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�G���������ꂽ��A�e�������[�`�����J�n
        StartCoroutine(ShootRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    IEnumerator ShootRoutine()
    {
        while (true)
        {
            //fireRate�b�҂�
            yield return new WaitForSeconds(fireRate);

            //�e�𐶐�
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            //���������e�̃^�O���uEnemyBullet�v�ɐݒ�
            bullet.tag = "EnemyBullet";
            //���������e�ɉ������̗͂�^����
            bullet.GetComponent<BulletController>().SetDirection(Vector3.down);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            //�����G�t�F�N�g���A�G�̂����ꏊ�ɐ�������
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            Destroy(other.gameObject); //�e��j�󂷂�
            Destroy(gameObject); //���̓G���g��j�󂷂�
        }
    }
}
