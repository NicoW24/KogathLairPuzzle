using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Minigame
{
    public class SlidingPuzzleManager : MinigameManager
    {
        [Header("Grid Configuration")]
        [SerializeField] int _gridSize = 3;
        [SerializeField] float _slideDuration = 0.15f;

        [Header("UI References")]
        [SerializeField] RectTransform _container;
        [SerializeField] GridLayoutGroup _gridLayout;
        [SerializeField] SlidingTileObject _tilePrefab;

        SlidingTileObject[] _spawnedTiles;
        int _emptyIndex;
        int _totalTiles;
        bool _isAnimating = false;

        public override void StartMinigame()
        {
            base.StartMinigame();
            SetupMinigame();
        }

        public override void MinigameFinish()
        {
            base.MinigameFinish();
            for (int i = 0; i < _emptyIndex; i++)
            {
                _spawnedTiles[i].gameObject.SetActive(false);
            }
            this.gameObject.SetActive(false);
        }

        public override void MinigameWin()
        {
            OnMinigameWin?.Invoke();
            base.MinigameWin();
        }

        public override void SetupMinigame()
        {
            _totalTiles = _gridSize * _gridSize;
            _spawnedTiles = new SlidingTileObject[_totalTiles];
            _emptyIndex = _totalTiles - 1;

            List<int> numbers = GenerateSolvableBoard();
            List<SlidingTileObject> existingTiles = new List<SlidingTileObject>();
            foreach (Transform child in _container)
            {
                if (child.TryGetComponent<SlidingTileObject>(out var tile))
                {
                    existingTiles.Add(tile);
                }
            }

            int existingCount = existingTiles.Count;

            for (int i = 0; i < _totalTiles; i++)
            {
                int number = numbers[i];

                if (number == 0)
                {
                    _spawnedTiles[i] = null;
                    _emptyIndex = i;
                    continue;
                }

                SlidingTileObject tileInstance;

                if (i < existingCount)
                {
                    tileInstance = existingTiles[i];
                    tileInstance.gameObject.SetActive(true);
                }
                else
                {
                    tileInstance = Instantiate(_tilePrefab, _container);
                }

                tileInstance.Setup(number, i, OnTileClicked);
                _spawnedTiles[i] = tileInstance;
            }

            for (int i = _totalTiles; i < existingCount; i++)
            {
                existingTiles[i].gameObject.SetActive(false);
            }
        }

        void OnTileClicked(SlidingTileObject clickedTile)
        {
            if (_isAnimating) return;

            int tileIdx = clickedTile.CurrentIndex;
            if (IsAdjacent(tileIdx, _emptyIndex))
            {
                StartCoroutine(SwapTiles(tileIdx, _emptyIndex));
            }
        }

        bool IsAdjacent(int indexA, int indexB)
        {
            int rowA = indexA / _gridSize;
            int colA = indexA % _gridSize;
            int rowB = indexB / _gridSize;
            int colB = indexB % _gridSize;

            return (Mathf.Abs(rowA - rowB) + Mathf.Abs(colA - colB)) == 1;
        }

        IEnumerator SwapTiles(int tileIndex, int targetEmptyIndex)
        {
            _isAnimating = true;

            SlidingTileObject tileToMove = _spawnedTiles[tileIndex];
            Vector3 targetPos = GetGridPosition(targetEmptyIndex);
            _spawnedTiles[targetEmptyIndex] = tileToMove;
            _spawnedTiles[tileIndex] = null;
            tileToMove.UpdateIndex(targetEmptyIndex);
            _emptyIndex = tileIndex;

            tileToMove.SlideToPosition(targetPos, _slideDuration);
            yield return new WaitForSeconds(_slideDuration);

            _isAnimating = false;

            CheckWinCondition();
        }

        Vector3 GetGridPosition(int index)
        {
            int row = index / _gridSize;
            int col = index % _gridSize;

            float cellWidth = _gridLayout.cellSize.x + _gridLayout.spacing.x;
            float cellHeight = _gridLayout.cellSize.y + _gridLayout.spacing.y;

            float startX = -((_gridSize - 1) * cellWidth) / 2f;
            float startY = ((_gridSize - 1) * cellHeight) / 2f;

            Vector3 localPos = new Vector3(startX + col * cellWidth, startY - row * cellHeight, 0f);
            return _container.TransformPoint(localPos);
        }

        List<int> GenerateSolvableBoard()
        {
            int total = _gridSize * _gridSize;
            List<int> numbers = new List<int>();

            for (int i = 1; i < total; i++) numbers.Add(i);
            numbers.Add(0);
            do
            {
                for (int i = 0; i < numbers.Count - 1; i++)
                {
                    int rand = Random.Range(i, numbers.Count - 1);
                    int temp = numbers[i];
                    numbers[i] = numbers[rand];
                    numbers[rand] = temp;
                }
            } while (!IsSolvable(numbers) || IsSolved(numbers));

            return numbers;
        }

        bool IsSolvable(List<int> puzzle)
        {
            int inversions = 0;
            int total = puzzle.Count;

            for (int i = 0; i < total - 1; i++)
            {
                for (int j = i + 1; j < total; j++)
                {
                    if (puzzle[i] != 0 && puzzle[j] != 0 && puzzle[i] > puzzle[j])
                    {
                        inversions++;
                    }
                }
            }

            if (_gridSize % 2 != 0)
            {
                return inversions % 2 == 0;
            }
            else
            {
                int emptyRowFromBottom = _gridSize - (puzzle.IndexOf(0) / _gridSize);
                return (inversions + emptyRowFromBottom) % 2 == 0;
            }
        }

        bool IsSolved(List<int> puzzleList)
        {
            for (int i = 0; i < puzzleList.Count - 1; i++)
            {
                if (puzzleList[i] != i + 1) return false;
            }
            return true;
        }

        void CheckWinCondition()
        {
            for (int i = 0; i < _spawnedTiles.Length - 1; i++)
            {
                if (_spawnedTiles[i] == null || !_spawnedTiles[i].IsInCorrectPosition())
                {
                    return;
                }
            }
            MinigameWin();
        }
    }
}
