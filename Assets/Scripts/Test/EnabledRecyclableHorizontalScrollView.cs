using Muks.RecyclableScrollView;
using System.Collections.Generic;
using UnityEngine;

public class EnabledRecyclableHorizontalScrollView : RecyclableHorizontalScrollView<int>
{
    [SerializeField] private int _slotCount;

    void Awake()
    {
        //데이터 추가
        List<int> dataList = new List<int>();
        for(int i = 0; i < _slotCount; i++)
        {
            dataList.Add(i);
        }

        //초기 설정 및 슬롯 데이터 입력
        Init(dataList);

        //데이터 갱신 기능
        dataList.Add(-1);
        UpdateData(dataList);
    }

    public override void Init(List<int> dataList)
    {
        //여기서 초기 설정 기능 추가
        base.Init(dataList);
    }
}
