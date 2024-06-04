using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHud : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameTmp;
    [SerializeField] private TextMeshProUGUI _hpTmp;
    [SerializeField] private Image _hpBarFilling;



    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void UpdateName(string name)
    {
        //if (IsOwner)
        //{
            _nameTmp.text = name;
        //}
    }

    public void UpdateHpBar(int currentHp, int maxHp)
    {
        _hpTmp.text = currentHp.ToString();
        _hpBarFilling.fillAmount = (float)currentHp / maxHp;
    }
}
