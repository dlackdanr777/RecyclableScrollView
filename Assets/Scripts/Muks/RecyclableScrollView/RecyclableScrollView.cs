using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Muks.RecyclableScrollView
{
    public abstract class RecyclableScrollView<T> : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] protected ScrollRect _scrollRect;
        [SerializeField] protected RectTransform _contentRect;
        [SerializeField] protected RecyclableScrollSlot<T> _slotPrefab;

        [Space]
        [Header("Option")]
        [SerializeField] protected int _bufferCount = 5;
        [SerializeField] protected float _spacing;

        protected LinkedList<RecyclableScrollSlot<T>> _slotList = new LinkedList<RecyclableScrollSlot<T>>();
        protected List<T> _dataList = new List<T>(); // �����͸� �����ϴ� ����Ʈ
        protected float _itemHeight;
        protected float _itemWidth;
        protected int _poolSize;
        protected int _tmpfirstVisibleIndex;
        protected int _contentVisibleSlotCount;

        /// <summary>�ʱ� ����</summary>
        public abstract void Init(List<T> dataList);


        /// <summary>������ ������ �����ϴ� �Լ�</summary>
        public abstract void UpdateData(List<T> dataList);


        /// <summary>ScrollRect �̺�Ʈ�� �����Ͽ� ������ ��ġ�� �����ϴ� �Լ�</summary>
        protected abstract void OnScroll(Vector2 scrollPosition);


        /// <summary>������ �����͸� ������Ʈ�ϰ� ��ġ�� �����ϴ� �Լ�</summary>
        protected abstract void UpdateSlot(RecyclableScrollSlot<T> item, int index);
    }
}



