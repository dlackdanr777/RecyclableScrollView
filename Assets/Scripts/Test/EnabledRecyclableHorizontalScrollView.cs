using Muks.RecyclableScrollView;
using System.Collections.Generic;
using UnityEngine;

public class EnabledRecyclableHorizontalScrollView : RecyclableHorizontalScrollView<int>
{
    [SerializeField] private int _slotCount;

    void Awake()
    {
        //������ �߰�
        List<int> dataList = new List<int>();
        for(int i = 0; i < _slotCount; i++)
        {
            dataList.Add(i);
        }

        //�ʱ� ���� �� ���� ������ �Է�
        Init(dataList);

        //������ ���� ���
        dataList.Add(-1);
        UpdateData(dataList);
    }

    public override void Init(List<int> dataList)
    {
        //���⼭ �ʱ� ���� ��� �߰�
        base.Init(dataList);
    }
}
