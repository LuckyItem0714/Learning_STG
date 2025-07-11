using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 3f;
    public int hp = 10; //�G��HP
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

    //�_���[�W���󂯂邽�߂̌��J���\�b�h
    public void TakeDamage(int damageAmount)
    {
        hp -= damageAmount;

        if (hp <= 0)
        {
            //�����G�t�F�N�g���A�G�̂����ꏊ�ɐ�������
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            //�G���g��j��
            Destroy(gameObject);

            //�X�R�A�����Z����
            ScoreManager.instance.AddScore(10);
        }
    }

    //�v���C���[�̒ʏ�e�ɓ����������̏����́A������ŕʓr�s��
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            //���������e��BulletController�R���|�[�l���g���擾
            BulletController bullet = other.GetComponent<BulletController>();

            //�����擾�ł�����(���肪�{���ɒe�Ȃ�)
            if (bullet != null)
            {
                TakeDamage(bullet.damage);
            }

            //�e�͓��������������
            Destroy(other.gameObject);
        }
    }

    /*void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            //���������e��BulletController�R���|�[�l���g���擾
            BulletController bullet = other.GetComponent<BulletController>();

            //�����擾�ł�����(���肪�{���ɒe�Ȃ�)
            if (bullet != null)
            {
                //HP��e�̎���damage���������炷
                hp -= bullet.damage;
            }

            //�e�͓��������������
            Destroy(other.gameObject);

            //����HP��0�ȉ��ɂȂ�����
            if (hp <= 0)
            {
                //�����G�t�F�N�g���A�G�̂����ꏊ�ɐ�������
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                //�G���g��j��
                Destroy(gameObject);

                //�X�R�A�����Z����
                ScoreManager.instance.AddScore(10);
            }
        }
    }*/
}
