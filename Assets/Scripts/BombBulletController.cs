using UnityEngine;

public class BombBulletController : MonoBehaviour
{
    public Transform orbitCenter; //��]�̒��S(�v���C���[)
    public float orbitSpeed = 200f; //��]���x
    public float orbitRadius = 1.5f; //��]�̔��a
    public float orbitDuration = 1.5f; //��]���鎞��

    public float homingSpeed = 15f; //�z�[�~���O���̑��x
    private Transform target; //�ǐՂ���G
    private bool isHoming = false; //�z�[�~���O���[�h�ɐ؂�ւ�������ǂ����̖ڈ�

    private float angle; //���݂̊p�x

    public int damage = 10; //�{���̒e���^����_���[�W��

    public void SetInitialAngle(float initialAngle)
    {
        angle = initialAngle;
    }

    // Update is called once per frame
    void Update()
    {
        //������]���Ԃ��܂��c���Ă�����
        if (orbitDuration > 0) {
            //�p�x�����ԂƂƂ��ɕω�������
            angle += orbitSpeed * Time.deltaTime;

            //�V�����ʒu���v�Z
            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
            float y = Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;

            //���S�̎���̈ʒu�Ɉړ�
            transform.position = orbitCenter.position + new Vector3(x, y, 0);

            //��]���Ԃ����炵�Ă���
            orbitDuration -= Time.deltaTime;
        }
        //�I�[�r�b�g���I�������
        else
        {
            if(!isHoming)
            {
                //�ł��߂��G��T���āA�����̃^�[�Q�b�g�ɐݒ肷��
                target = PlayerController.FindClosestEnemy(transform.position);
                isHoming=true;
            }

            //�����^�[�Q�b�g���������Ă�����A���̕����֐i��
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                transform.Translate(direction * homingSpeed * Time.deltaTime);
            }
            //�����^�[�Q�b�g�����Ȃ�������A�Ƃ肠�����^��ɔ��
            else
            {
                transform.Translate(Vector3.up * homingSpeed * Time.deltaTime);
            }
        }
    }

    //�����蔻��̃��\�b�h
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //�����EnemyController���擾���āA�_���[�W��^����
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            //�������g��j�󂷂�
            Destroy(gameObject);
        }
        else if (other.CompareTag("EnemyBullet"))
        {
            //�G�̒e��j�󂷂�
            Destroy(other.gameObject);

            //�������g��j�󂷂�
            // Destroy(gameObject);
        }
    }
}
