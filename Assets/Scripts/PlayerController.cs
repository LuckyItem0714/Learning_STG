using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float normalSpeed = 5f; //�ʏ�X�s�[�h
    public float slowSpeed = 2.5f; //�ᑬ�X�s�[�h
    public float moveSmoothing = 0.1f; //�ړ��̊��炩��(�������قǋ@�q�A�傫���قǊ���)
    private Vector3 currentVelocity; //���݂̈ړ����x�ƕ�����ێ�����ϐ�

    public int maxHp = 3; //�ő�HP
    private int currentHp; //���݂�HP
    public HpUiManager hpUiManager;

    [Header("�ˌ��ݒ�")] //�C���X�y�N�^�[�Ɍ��o����ǉ�
    public GameObject bulletPrefab; //�e�̃v���n�u���i�[����ϐ�
    public float fireRate = 0.25f; //�e�̔��ˊԊu(0.25�b��1��)
    public int normalShotDamage = 2; //�ʏ�e�̃_���[�W
    public int homingShotDamage = 1; //�z�[�~���O�e�̃_���[�W
    private float nextFireTime = 0f; //���ɒe�𔭎˂ł��鎞��

    [Header("�{���ݒ�")]
    public GameObject bombBulletPrefab; //�{���̒e�v���n�u
    public int bombBulletCount = 10; //��������{���̒e�̐�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�Q�[���J�n����HP���ő�ɂ���
        currentHp = maxHp;
        Debug.Log("�v���C���[��HP : " + currentHp);
        hpUiManager.UpdateHp(currentHp);

        //�����ʒu�ύX
        transform.position = new Vector3(0, -3.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //���݂̃t���[���œK�p����X�s�[�h�����肷��
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? slowSpeed : normalSpeed;

        //�v���C���[�̈ړ�
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector3 targetVelocity = new Vector3(x, y, 0) * targetSpeed;

        //���݂̑��x(currentVelocity)���A�ڕW�̑��x(targetVelocity)�Ɋ��炩�ɋ߂Â���
        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 1 - Mathf.Pow(moveSmoothing, Time.deltaTime));

        Vector3 newPosition = transform.localPosition + currentVelocity * Time.deltaTime;

        //���W�𐧌�
        newPosition.x = Mathf.Clamp(newPosition.x, -4.45f, 4.45f);
        newPosition.y = Mathf.Clamp(newPosition.y, -5f, 5f);

        transform.localPosition = newPosition;

        //�e��������
        if (Input.GetKey(KeyCode.Z) && Time.time > nextFireTime)
        {
            //���ɔ��ˉ\�Ȏ��Ԃ��A���݂̎��Ԃɔ��ˊԊu�����������̂ɍX�V
            nextFireTime = Time.time + fireRate;

            //Shift�L�[��������Ă��邩�Ŏˌ����[�h��؂�ւ�
            if(Input.GetKey(KeyCode.LeftShift))
            {
                //�z�[�~���O�e�𔭎�
                FireHomingBullet();
            }
            else
            {
                //�ʏ�e�𔭎�
                FireNormalBullet();
            }
        }

        //�{���̔�������
        if (Input.GetKeyDown(KeyCode.X))
        {
            FireBomb();
        }
    }

    //�ʏ�e�𔭎˂��郁�\�b�h
    void FireNormalBullet()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        BulletController bullet = bulletObj.GetComponent<BulletController>();

        //���������e��BulletController���擾���A�i�s�������u��v�ɐݒ肷��
        bullet.SetDirection(Vector3.up);
        //�ʏ�e�̃_���[�W��ݒ�
        bullet.damage = normalShotDamage;
    }

    //�z�[�~���O�e�𔭎˂��郁�\�b�h
    void FireHomingBullet()
    {
        //�ł��߂��G��T��
        Transform closestEnemy = FindClosestEnemy(transform.position);

        if (closestEnemy != null)
        {
            GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            BulletController bullet = bulletObj.GetComponent<BulletController>();
            
            //�e�̃^�[�Q�b�g�Ƃ��āA�������ł��߂��G��ݒ肷��
            bullet.target = closestEnemy;
            //�z�[�~���O�e�̃_���[�W��ݒ�
            bullet.damage = homingShotDamage;
        }
        else
        {
            //�����G����̂����Ȃ���΁A����ɒʏ�e�𔭎˂���
            FireNormalBullet();
        }
    }

    //�ł��߂��G��T���o�����\�b�h
    public static Transform FindClosestEnemy(Vector3 fromPosition)
    {
        //"Enemy"�^�O�������ׂẴI�u�W�F�N�g��T���āA�z��ɓ����
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity; //�ł��߂��G�Ƃ̋�����2����L�^
        // Vector3 currentPosition = transform.position;

        //�������S�Ă̓G�ɑ΂��ă��[�v����
        foreach (GameObject potentialEnemy in enemies)
        {
            Vector3 directionToTarget = potentialEnemy.transform.position - fromPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude; //������2����v�Z

            //�����A���܂ł̍ŒZ���������߂����
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialEnemy.transform;
            }
        }

        return bestTarget; //�������ł��߂��G��Ԃ�
    }

    //�{���𔭎˂��郁�\�b�h
    void FireBomb()
    {
        //360�x���A�e�̐��ŋϓ��Ɋ���
        float angleStep = 360f / bombBulletCount;

        for (int i = 0; i < bombBulletCount; i++)
        {
            //���݂̒e�̊p�x���v�Z
            float currentAngle = i * angleStep;

            //�e���v���C���[�̈ʒu�ɐ���
            GameObject bomb = Instantiate(bombBulletPrefab, transform.position, Quaternion.identity);

            //�e�̃R���g���[���[���擾���A���S(�������g)�Ə����p�x���Z�b�g����
            BombBulletController bombController = bomb.GetComponent<BombBulletController>();
            if (bombController != null)
            {
                bombController.orbitCenter = transform; //�utransform�v�̓v���C���[���g���w��
                bombController.SetInitialAngle(currentAngle); //�e�e�ɁA���ꂼ��̔��ˊp�x��������
            }
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
            hpUiManager.UpdateHp(currentHp);

            //����HP��0�ȉ��ɂȂ�����
            if (currentHp <= 0)
            {
                //���̃v���C���[�I�u�W�F�N�g��j�󂷂�
                Destroy(gameObject);
            }
        }
    }
}
