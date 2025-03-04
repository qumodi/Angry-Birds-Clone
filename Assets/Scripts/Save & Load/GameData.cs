using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]
public class LevelStats
{
    public int LevelScore;
    public int StarsCount;
    public bool Unlocked;

    public LevelStats(int LevelScore, int StarsCount, bool Unlocked = false)
    {
        this.LevelScore = LevelScore;
        this.StarsCount = StarsCount;
        this.Unlocked = Unlocked;
    }

}

[System.Serializable]
public class Section
{
    public int levelCount;
    public bool Unlocked = false;
    //public int unlockedLevelCount;
    public List<LevelStats> Levels;

    public Section()
    {
        this.levelCount = 21;
        //this.unlockedLevelCount = 1;

        Levels = new List<LevelStats>(levelCount);
        for (int i = 0; i < levelCount; i++)
        {
            Levels.Add(new LevelStats(0, 0));
        }
    }

    public Section(int LevelCount)//,int UnlockedLevelCount
    {
        this.levelCount = LevelCount;
        //this.unlockedLevelCount = UnlockedLevelCount;

        Levels = new List<LevelStats>();
        for (int i = 0; i < levelCount; i++)
        {
            Levels.Add(new LevelStats(0, 0));
        }
    }
}

//[System.Serializable]
public class GameData
{
    private int SectionsCount = 3;
    //Sections Data
    //Section 1
    public List<Section> Sections;
    //private int[] levels = new int[] { 15, 15, 15, 15 };

    const int levelsCount = 42;
    private int[] levels = new int[] { levelsCount,levelsCount,levelsCount };
    public GameData()
    {
        Sections = new List<Section>();
        for (int i = 0; i < SectionsCount; i++)
        {
            Sections.Add(new Section(levels[i]));

            if(i == 0)
            {
                UnlockSectionByIdx(0);
            }
            UnlockLevelByIdx(i, 0);
            //UnlockLevelByIdx(i, 15);
        }
    }

    public LevelStats GetLevel(int SectionIdx, int LevelIdx)
    {
        if (SectionIdx < 0 || SectionIdx >= Sections.Count)
        {
            return null;
        }
        if (LevelIdx < 0 || LevelIdx >= Sections[SectionIdx].levelCount)
        {
            return null;
        }
        return Sections[SectionIdx].Levels[LevelIdx];
    }

    public void UnlockLevelByIdx(int SectionIdx, int LevelIdx)
    {
        if (GetLevel(SectionIdx, LevelIdx) != null)
        {
            Sections[SectionIdx].Levels[LevelIdx].Unlocked = true;
        }
    }

    public void UnlockLevelByNumber(int SectionNum, int LevelNum)
    {
        int SectionIdx = SectionNum - 1;
        int LevelIdx = LevelNum - 1;
        if (GetLevel(SectionIdx, LevelIdx) != null)
        {
            Sections[SectionIdx].Levels[LevelIdx].Unlocked = true;
        }
    }

    public void UnlockSectionByIdx(int SectionIdx)
    {
        Sections[SectionIdx].Unlocked = true;
    }
    public void UnlockSectionByNum(int SectionNum)
    {
        int SectionIdx = SectionNum - 1;
        Sections[SectionIdx].Unlocked = true;
    }
}


