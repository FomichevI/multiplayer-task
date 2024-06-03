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

    [SerializeField] private NetworkVariable<FixedString32Bytes> _name = 
        new NetworkVariable<FixedString32Bytes>("", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    [ClientRpc]
    public void UpdateNameClientRpc(string name)
    {
        //_name.Value = name;
        //_nameTmp.text = _name.Value.ToString();

        _nameTmp.text = name;
    }

    public void UpdateHpBar(int currentHp, int maxHp)
    {
        _hpTmp.text = currentHp.ToString();
        _hpBarFilling.fillAmount = (float)currentHp / maxHp;
    }
}
