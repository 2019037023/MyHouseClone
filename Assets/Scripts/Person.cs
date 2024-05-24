using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    Status _status;
    Animator _anim;
    public Melee _melee;
    void Start()
    {
        gameObject.AddComponent<Status>();
        _status = gameObject.GetComponent<Status>();
        _anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y)) 
        {
            _anim.SetTrigger("setAttack");
            _melee.Use();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("��Ҵ�");

        /*
         * ���� ���⿡�� �����
         * ���߿� ���Ÿ� ���⿡ �ǰ� ������ ���� �ϵ� ó���� ��� ��
         * Tag�� �̿��ϱ� ���ٴ� Weapon.cs�� ��� Type���� ������ ���� 
         */
        if(other.tag == "Melee")
        {
            _status.TakedDamage(other.GetComponent<Weapon>().Attack);

            if (_status.Hp <= 0)
            {
                _anim.SetTrigger("setDie");
            }
        }
    }
}
