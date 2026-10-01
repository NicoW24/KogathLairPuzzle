using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Minigame
{
    public class MemoryMatchCardManager :MinigameManager
    {
        [SerializeField] MemoryMatchCardObject _prefabCardObject;
        [SerializeField] Transform _cardContainer;
        [SerializeField] Sprite _backCardSprite;
        [SerializeField] List<Sprite> _listCardSprite = new List<Sprite>();

        List<MemoryMatchCardObject> _spawnedCards = new List<MemoryMatchCardObject>();
        MemoryMatchCardObject _firstSelected = null;
        MemoryMatchCardObject _secondSelected = null;
        bool _isProcessingMatch = false;
        int _matchesFound = 0;

        public override void StartMinigame()
        {
            base.StartMinigame();
            SetupMinigame();
            _matchesFound = 0;
        }

        public override void MinigameFinish()
        {
            base.MinigameFinish();
            foreach (MemoryMatchCardObject cardObject in _spawnedCards)
            {
                cardObject.ShowBack();
                cardObject.gameObject.SetActive(false);
            }
            this.gameObject.SetActive(false);
        }

        public override void MinigameWin()
        {
            Debug.Log("Game Over - You Win!");
            OnMinigameWin?.Invoke();
            base.MinigameWin();
        }

        public override void SetupMinigame()
        {
            base.SetupMinigame();
            List<int> cardIDs = new List<int>();
            for (int i = 0; i < _listCardSprite.Count; i++)
            {
                cardIDs.Add(i);
                cardIDs.Add(i);
            }
            ShuffleList(cardIDs);

            //if not spawned instantiate
            if(_spawnedCards.Count == 0)
            {
                foreach (int id in cardIDs)
                {
                    MemoryMatchCardObject newCardObj = Instantiate(_prefabCardObject, _cardContainer);
                    newCardObj.Setup(id, _listCardSprite[id], _backCardSprite, this);
                    _spawnedCards.Add(newCardObj);
                }
            }
            else
            {
                //use old card
                int idCardCounter = 0;
                foreach (MemoryMatchCardObject cardObject in _spawnedCards)
                {
                    int cardId = cardIDs[idCardCounter];
                    cardObject.Setup(cardId, _listCardSprite[cardId], _backCardSprite, this);
                    cardObject.gameObject.SetActive(true);
                    idCardCounter++;
                }
            }
        }

        public bool CanSelectCard()
        {
            return !_isProcessingMatch;
        }
        public void OnCardSelected(MemoryMatchCardObject card)
        {
            if (_firstSelected == null)
            {
                _firstSelected = card;
            }
            else if (_secondSelected == null && card != _firstSelected)
            {
                _secondSelected = card;
                StartCoroutine(CheckMatchRoutine());
            }
        }
        IEnumerator CheckMatchRoutine()
        {
            _isProcessingMatch = true;

            yield return new WaitForSeconds(0.45f);//delay

            if (_firstSelected.cardId == _secondSelected.cardId)
            {
                //same card
                _firstSelected.SetInteractable(false);
                _secondSelected.SetInteractable(false);
                _matchesFound++;

                if (_matchesFound >= _listCardSprite.Count)
                {
                    yield return new WaitForSeconds(0.75f);//delay
                    MinigameWin();
                }
            }
            else
            {
                //not same card
                _firstSelected.ShowBack();
                _secondSelected.ShowBack();
            }

            _firstSelected = null;
            _secondSelected = null;
            _isProcessingMatch = false;
        }
        void ShuffleList<T>(List<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                T temp = list[i];
                int randomIndex = Random.Range(i, list.Count);
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }
    }
}

