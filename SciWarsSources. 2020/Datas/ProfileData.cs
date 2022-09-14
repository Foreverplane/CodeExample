using MessagePack;
using UnityEngine;


[System.Serializable]
[MessagePackObject()]
public class ProfileData : IEntityData {
    [Key(0)]
    public string nickName;
    [Key(1)]
    public byte avatarIcon;
    [Key(2)]
    public int points;
    [Key(3)]
    public string guid;
    [Key(4)]
    public int experience;
    [Key(5)]
    public int credits;
    [IgnoreMember]
    public int CurrentLevel {
        get {
            var lvl = 0.116f * Mathf.Pow(experience, 0.573f);
            return (int)Mathf.Round(lvl);
        }
    }
    [IgnoreMember]
    public float NextLevelExp {
        get {
            return Mathf.Pow((CurrentLevel + 1) / 0.116f, 1 / 0.573f);
        }
    }
}
