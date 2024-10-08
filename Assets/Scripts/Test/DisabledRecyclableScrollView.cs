using UnityEngine;

public class DisabledRecyclableScrollView : MonoBehaviour
{
    [SerializeField] private int _slotCount;
    [SerializeField] private RectTransform _contentRect;
    [SerializeField] private Slot _slotPrefab;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < _slotCount; i++)
        {
            Slot slot = Instantiate(_slotPrefab, _contentRect);
            slot.UpdateSlot(i);
        }
    }
}
