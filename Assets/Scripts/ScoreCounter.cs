using System;

namespace TestProject
{
    public sealed class ScoreCounter
    {
        public event Action<int> OnScoreChanged;
        public event Action<int> OnPickedItemsChanged;
        public event Action OnResetCurrent;
        public event Action OnResetAll;
        public event Action OnGoalAchieved;

        private int _currentSessionTotalScore;
        
        public ScoreCounter(int totalScore)
        {
            TotalScore = totalScore;
        }
        
        public int CurrentLevelScore { get; private set; }
        public int TotalScore { get; private set; }
        public int PickedItems { get; private set; }
        
        public int LevelGoal { get; private set; }

        public void Increase(int score, int pickedItems)
        {
            CurrentLevelScore += score;
            PickedItems += pickedItems;
            _currentSessionTotalScore += score;
            
            if (TotalScore < _currentSessionTotalScore)
                TotalScore = _currentSessionTotalScore;
            
            OnScoreChanged?.Invoke(CurrentLevelScore);
            OnPickedItemsChanged?.Invoke(PickedItems);
            
            if(PickedItems >= LevelGoal)
                OnGoalAchieved?.Invoke();
        }

        public void SetLevelGoal(int value)
        {
            LevelGoal = value;
        }

        public void ResetCurrentSession()
        {
            _currentSessionTotalScore = 0;
            ResetCurrentLevel();
        }
        
        public void ResetCurrentLevel()
        {
            CurrentLevelScore = PickedItems = 0;
            OnResetCurrent?.Invoke();
        }

        public void ResetAll()
        {
            TotalScore = CurrentLevelScore = PickedItems = _currentSessionTotalScore = 0;
            OnResetAll?.Invoke();
        }
    }
}