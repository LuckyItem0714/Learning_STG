using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject bulletPrefab; //�e�̃v���n�u���i�[����ϐ�

    public int maxHp = 3; //�ő�HP
    private int currentHp; //���݂�HP

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�Q�[���J�n����HP���ő�ɂ���
        currentHp = maxHp;
        Debug.Log("�v���C���[��HP : " + currentHp);
    }

    // Update is called once per frame
    void Update()
    {
        //�v���C���[�̈ړ�
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(x, y, 0);

        Vector3 newPosition = transform.localPosition + direction * speed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, -2f, 1.7f);
        newPosition.y = Mathf.Clamp(newPosition.y, -5f, 5f);

        transform.localPosition = newPosition;

        //�e��������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            //���������e��BulletController���擾���A�i�s�������u��v�ɐݒ肷��
            bullet.GetComponent<BulletController>().SetDirection(Vector3.up);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //�Ԃ����Ă�������̃^�O���uEnemy�v�܂��́uEmenyBullet�v��������
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyBullet"))
        {
            //�Ԃ����Ă����I�u�W�F�N�g(�G���G�̒e)��j�󂷂�
            Destroy(other.gameObject);

            //HP��1���炷
            currentHp--;
            Debug.Log("�v���C���[��HP : " + currentHp); //HP���R���\�[���ɕ\��

            //����HP��0�ȉ��ɂȂ�����
            if (currentHp <= 0)
            {
                //���̃v���C���[�I�u�W�F�N�g��j�󂷂�
                Destroy(gameObject);
            }
        }
    }
}
