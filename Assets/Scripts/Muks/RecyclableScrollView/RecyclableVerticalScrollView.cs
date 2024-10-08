using System.Collections.Generic;
using UnityEngine;

namespace Muks.RecyclableScrollView
{
    public abstract class RecyclableVerticalScrollView<T> : RecyclableScrollView<T>
    {
        [Space]
        [Header("VerticalScrollView Option")]
        [SerializeField] protected int _itemsPerRow = 1;
        [SerializeField] protected float _topOffset;
        [SerializeField] protected float _bottomOffset;
        [SerializeField] protected float _horizontalOffset;

      
        public override void Init(List<T> dataList)
        {
            _dataList = dataList;

            RectTransform scrollRectTransform = _scrollRect.GetComponent<RectTransform>();
            // ���� ũ��
            _itemHeight = _slotPrefab.Height;
            _itemWidth = _slotPrefab.Width;

            // ��ü ���� ���
            int totalRows = Mathf.CeilToInt((float)_dataList.Count / _itemsPerRow);
            float contentHeight = _itemHeight * totalRows + ((totalRows - 1) * _spacing) + _topOffset + _bottomOffset;

            //Anchor�� ����(��� ���� ����)
            _contentRect.anchorMax = new Vector2(1f, 1f);
            _contentRect.anchorMin = new Vector2(0f, 1f);

            //contentRect�� ���� ���
            _contentVisibleSlotCount = (int)(scrollRectTransform.rect.height / _itemHeight) * _itemsPerRow;
            _contentRect.sizeDelta = new Vector2(_contentRect.sizeDelta.x, contentHeight);

            // ���� ���� �� ����Ʈ�� �߰�
            _poolSize = _contentVisibleSlotCount + (_bufferCount * 2 * _itemsPerRow);
            int index = -_bufferCount * _itemsPerRow;
            for (int i = 0; i < _poolSize; i++)
            {
                RecyclableScrollSlot<T> item = Instantiate(_slotPrefab, _contentRect);
                _slotList.AddLast(item);
                item.Init();
                UpdateSlot(item, index++);
            }
            _scrollRect.onValueChanged.AddListener(OnScroll);
        }


        public override void UpdateData(List<T> dataList)
        {
            _dataList = dataList;

            //���� ���Ե��� ����� index ���� �� Update
            int index = _tmpfirstVisibleIndex - _bufferCount * _itemsPerRow;
            foreach (RecyclableScrollSlot<T> item in _slotList)
            {
                UpdateSlot(item, index);
                index++;
            }
        }


        protected override void OnScroll(Vector2 scrollPosition)
        {
            float contentY = _contentRect.anchoredPosition.y;

            //���� �ε��� ��ġ ��� 
            int firstVisibleRowIndex = Mathf.Max(0, Mathf.FloorToInt(contentY / (_itemHeight + _spacing)));
            int firstVisibleIndex = firstVisibleRowIndex * _itemsPerRow;

            // ���� ���� ��ġ�� ���� ��ġ�� �޶����ٸ� ���� ���ġ
            if (_tmpfirstVisibleIndex != firstVisibleIndex)
            {
                int diffIndex = (_tmpfirstVisibleIndex - firstVisibleIndex) / _itemsPerRow;

                // ���� �ε����� �� ũ�ٸ� (���� ��ũ�� ��)
                if (diffIndex < 0)
                {
                    int lastVisibleIndex = _tmpfirstVisibleIndex + _contentVisibleSlotCount;
                    for (int i = 0, cnt = Mathf.Abs(diffIndex) * _itemsPerRow; i < cnt; i++)
                    {
                        RecyclableScrollSlot<T> item = _slotList.First.Value;
                        _slotList.RemoveFirst();
                        _slotList.AddLast(item);

                        int newIndex = lastVisibleIndex + (_bufferCount * _itemsPerRow) + i;
                        UpdateSlot(item, newIndex);
                    }
                }

                // ���� �ε����� �� ũ�ٸ� (�Ʒ��� ��ũ�� ��)
                else if (diffIndex > 0)
                {
                    for (int i = 0, cnt = Mathf.Abs(diffIndex) * _itemsPerRow; i < cnt; i++)
                    {
                        RecyclableScrollSlot<T> item = _slotList.Last.Value;
                        _slotList.RemoveLast();
                        _slotList.AddFirst(item);

                        int newIndex = _tmpfirstVisibleIndex - (_bufferCount * _itemsPerRow) - i;
                        UpdateSlot(item, newIndex);
                    }
                }

                _tmpfirstVisibleIndex = firstVisibleIndex;
            }
        }


        protected override void UpdateSlot(RecyclableScrollSlot<T> item, int index)
        {
            //���� Index�� ��� ���� ���
            int row = 0 <= index ? index / _itemsPerRow : (index - 1) / _itemsPerRow;
            int column = Mathf.Abs(index) % _itemsPerRow;

            // X�� �� Y�� ��ġ ��� (���θ� �������� �߾� ���� �� �ǹ� ����)
            Vector2 pivot = item.RectTransform.pivot;
            float totalWidth = (_itemsPerRow * (_itemWidth + _spacing)) - _spacing;
            float contentWidth = _contentRect.rect.width;
            float offsetX = (contentWidth - totalWidth) / 2f;
            float adjustedY = -(row * (_itemHeight + _spacing)) - _itemHeight * (1 - pivot.y);
            float adjustedX = column * (_itemWidth + _spacing) + _itemWidth * pivot.x;
            adjustedX += offsetX + _horizontalOffset;
            adjustedY -= _topOffset;
            item.RectTransform.localPosition = new Vector3(adjustedX, adjustedY, 0);

            //Index�� �Էµ� DataList�� ũ�⸦ �Ѿ�ų� 0�̸��̸� ������ ���� Update�� �������� �ʴ´�.
            if (index < 0 || index >= _dataList.Count)
            {
                item.gameObject.SetActive(false);
                return;
            }
            else
            {
                item.UpdateSlot(_dataList[index]);
                item.gameObject.SetActive(true);
            }
        }
    }
}



