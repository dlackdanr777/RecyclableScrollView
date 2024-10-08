using Muks.RecyclableScrollView;
using System.Collections.Generic;
using UnityEngine;

public class EnabledRecyclableScrollView : RecyclableVerticalScrollView<int>
{
    [SerializeField] private int _slotCount;


    // Start is called before the first frame update
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
