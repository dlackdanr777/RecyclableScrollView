using Muks.RecyclableScrollView;
using System.Collections.Generic;
using UnityEngine;

namespace Muks.RecyclableScrollView
{
    public abstract class RecyclableHorizontalScrollView<T> : RecyclableScrollView<T>
    {
        [Space]
        [Header("HorizontalScrollView Option")]
        [SerializeField] protected int _itemsPerColumn = 1;
        [SerializeField] protected float _leftOffset;
        [SerializeField] protected float _rightOffset;
        [SerializeField] protected float _verticalOffset;

        RectTransform _scrollRectTransform;
        public override void Init(List<T> dataList)
        {
            _dataList = dataList;

            _scrollRectTransform = _scrollRect.GetComponent<RectTransform>();
            // ���� ũ��
            _itemHeight = _slotPrefab.Height;
            _itemWidth = _slotPrefab.Width;

            // ��ü �ʺ� ���
            int totalColumns = Mathf.CeilToInt((float)_dataList.Count / _itemsPerColumn);
            float contentWidth = _itemWidth * totalColumns + (totalColumns > 0 ? (totalColumns - 1) * _spacing : 0) + _leftOffset + _rightOffset;

            //Anchor�� ����(��� ���� ����)
            _contentRect.anchorMax = new Vector2(1f, 1f);
            _contentRect.anchorMin = new Vector2(0f, 1f);

            //contentRect�� ���� ���
            _contentVisibleSlotCount = (int)(_scrollRectTransform.rect.width / _itemWidth) * _itemsPerColumn;
            Debug.Log(contentWidth - _scrollRectTransform.rect.width);
            _contentRect.sizeDelta = new Vector2(contentWidth - _scrollRectTransform.rect.width, _contentRect.sizeDelta.y);

            // ���� ���� �� ����Ʈ�� �߰�
            _poolSize = _contentVisibleSlotCount + (_bufferCount * 2 * _itemsPerColumn);
            int index = -_bufferCount * _itemsPerColumn;
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
            int index = _tmpfirstVisibleIndex - _bufferCount * _itemsPerColumn;
            foreach (RecyclableScrollSlot<T> item in _slotList)
            {
                UpdateSlot(item, index);
                index++;
            }
        }


        /// <summary>ScrollRect �̺�Ʈ�� �����Ͽ� ������ ��ġ�� �����ϴ� �Լ�</summary>
        protected override void OnScroll(Vector2 scrollPosition)
        {

            float contentX = _contentRect.anchoredPosition.x;

            //���� �ε��� ��ġ ��� 
            int firstVisibleRowIndex = Mathf.Max(0, Mathf.Abs(Mathf.FloorToInt(contentX / (_itemWidth + _spacing))));
            int firstVisibleIndex = firstVisibleRowIndex * _itemsPerColumn;

            // ���� ���� ��ġ�� ���� ��ġ�� �޶����ٸ� ���� ���ġ
            if (_tmpfirstVisibleIndex != firstVisibleIndex)
            {
                int diffIndex = (_tmpfirstVisibleIndex - firstVisibleIndex) / _itemsPerColumn;

                // ���� �ε����� �� ũ�ٸ� (�������� ��ũ�� ��)
                if (diffIndex < 0)
                {
                    int lastVisibleIndex = _tmpfirstVisibleIndex + _contentVisibleSlotCount;
                    for (int i = 0, cnt = Mathf.Abs(diffIndex) * _itemsPerColumn; i < cnt; i++)
                    {
                        RecyclableScrollSlot<T> item = _slotList.First.Value;
                        _slotList.RemoveFirst();
                        _slotList.AddLast(item);

                        int newIndex = lastVisibleIndex + (_bufferCount * _itemsPerColumn) + i;
                        UpdateSlot(item, newIndex);
                    }
                }

                // ���� �ε����� �� ũ�ٸ� (���������� ��ũ�� ��)
                else if (diffIndex > 0)
                {
                    for (int i = 0, cnt = Mathf.Abs(diffIndex) * _itemsPerColumn; i < cnt; i++)
                    {
                        RecyclableScrollSlot<T> item = _slotList.Last.Value;
                        _slotList.RemoveLast();
                        _slotList.AddFirst(item);

                        int newIndex = _tmpfirstVisibleIndex - (_bufferCount * _itemsPerColumn) - i;
                        UpdateSlot(item, newIndex);
                    }
                }

                _tmpfirstVisibleIndex = firstVisibleIndex;
            }
        }


        protected override void UpdateSlot(RecyclableScrollSlot<T> item, int index)
        {
            //���� Index�� ��� ���� ���
            int column = 0 <= index ? index / _itemsPerColumn : (index - 1) / _itemsPerColumn;
            int row = Mathf.Abs(index) % _itemsPerColumn;

            Vector2 pivot = item.RectTransform.pivot;
            // �߾� ���� ������ (¦���� Ȧ���� ���� �ٸ�)
            float rowOffsetY;
            if (_itemsPerColumn % 2 == 0) // ¦���� ���
            {
                // ¦���� ���� �߾Ӱ��� �� �� ���̿� ��ġ
                float middleRow = (_itemsPerColumn / 2f) - 0.5f;
                rowOffsetY = -(row - middleRow) * (_itemHeight + _spacing);
            }
            else // Ȧ���� ���
            {
                // Ȧ���� ���� �߾� ���� �������� ��/�Ʒ��� ����
                int middleRow = (_itemsPerColumn - 1) / 2;
                rowOffsetY = row == middleRow ? 0 : row < middleRow ? -(middleRow - row) * (_itemHeight + _spacing) : row > middleRow ? (row - middleRow) * (_itemHeight + _spacing) : 0;
            }

            // �ǹ� ���� ��
            float pivotAdjustmentY = -_itemHeight * (0.5f - pivot.y);

            // X�� �� Y�� ��ġ ��� (���θ� �������� �߾� ���� �� �ǹ� ����)
            float scrollViewWidth = _contentRect.rect.width;
            float adjustedX = -(scrollViewWidth * 0.5f) + (column * (_itemWidth + _spacing)) + _itemWidth * pivot.x;
            float pivotAdjustmentX = _itemWidth * (0.5f - pivot.x);
            float adjustedY = rowOffsetY + _verticalOffset + pivotAdjustmentY;
            adjustedX += pivotAdjustmentX;
            adjustedX += _leftOffset;
            item.RectTransform.anchoredPosition = new Vector2(adjustedX, adjustedY);

            //Index�� �Էµ� DataList�� ũ�⸦ �Ѿ�ų� 0�̸��̸� ������ ���� Update�� �������� �ʴ´�.
            if (index < 0 || index >= _dataList.Count)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                item.UpdateSlot(_dataList[index]);
                item.gameObject.SetActive(true);
            }
        }
    }
}




