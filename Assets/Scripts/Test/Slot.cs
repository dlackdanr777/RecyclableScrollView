using Muks.RecyclableScrollView;
using UnityEngine;
using UnityEngine.UI;

public class Slot : RecyclableScrollSlot<int>
{
    [SerializeField] private Text _text;
    
    public override void Init()
    {
        //여기에서 초기 설정
    }

    public override void UpdateSlot(int data)
    {
        //여기에서 Update시 행동 설정
        _text.text = data.ToString();
    }
}
