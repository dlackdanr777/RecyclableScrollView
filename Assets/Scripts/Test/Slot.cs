using Muks.RecyclableScrollView;
using UnityEngine;
using UnityEngine.UI;

public class Slot : RecyclableScrollSlot<int>
{
    [SerializeField] private Text _text;
    
    public override void Init()
    {
        //���⿡�� �ʱ� ����
    }

    public override void UpdateSlot(int data)
    {
        //���⿡�� Update�� �ൿ ����
        _text.text = data.ToString();
    }
}
