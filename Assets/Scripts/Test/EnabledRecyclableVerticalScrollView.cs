using Muks.RecyclableScrollView;
using System.Collections.Generic;
using UnityEngine;

public class EnabledRecyclableVerticalScrollView : RecyclableVerticalScrollView<int>
{
    [SerializeField] private int _slotCount;

    void Start()
    {
        List<int> dataList = new List<int>();
        for(int i = 0; i < _slotCount; i++)
        {
            dataList.Add(i);
        }

        Init(dataList);
    }
}
