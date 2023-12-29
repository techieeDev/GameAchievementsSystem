using UnityEngine;
using UnityEngine.UI;
public class AchievementData : MonoBehaviour
{

    [Range(11290, 21290)]
    public int AchievementId;

    public string _achievement_name;
    public Sprite _achievement_sprite;

    public AchievementType achievementType;
    public bool isUnlocked;
    public bool canFadeIn;
    internal bool isSameId;
    internal bool isTimerOn;

    private float _color_t_ = 0.1f;
    internal float timer = 7;
    internal float clock = 0;

    void Start()
    {
        GameEvents.current.onItemPickup += AchievementUnlock;

        GameEvents.current.onEntranceAchievement += AchievementUnlock;

        GameEvents.current.onItemPickup += GetId;

        GameEvents.current.onEntranceAchievement += GetId;

        AchievementCheck(AchievementManager.current.AchievementStore, AchievementManager.current.AchievemntIds);
    }
    void ShowItemIcon()
    {
        if (canFadeIn)
        {
            if (isSameId)
            {
                Image AchievementPopup = GameObject.Find("AchievementPopup").GetComponent<Image>();

                AchievementPopup.sprite = _achievement_sprite;

                

                if (Time.time > clock && timer > 0)
                {
                    isTimerOn = true;
                    Debug.Log(timer);
                    clock = Time.time + 1;
                    timer--;
                }
                else if (isTimerOn && timer > 0)
                {
                    AchievementPopup.color = Color.Lerp(AchievementPopup.color, Color.white, _color_t_);
                }
                else if(timer <= 0)
                {
                    AchievementPopup.color = Color.Lerp(AchievementPopup.color, new Color(0, 0, 0, 0), _color_t_ * 0.08f);
                    isTimerOn = false;
                }
            }
        }
    }


    void AchievementUnlock(AchievementData data)
    {
        if(data.AchievementId == AchievementId)
        {
            Debug.Log(AchievementId);
            
            if (isUnlocked == false)
            {
                AudioSource _sfxplayer = GameObject.Find("AchievementSFX").GetComponent<AudioSource>();

                isUnlocked = true;
                canFadeIn = true;

                _sfxplayer.Play();
                
                AchievementManager.current.RegisterAchievement(isUnlocked, AchievementId);

                AchievementManager.current.SaveLoadedAchievements("_achievements.save", "_ids.save");

                GameEvents.current.onItemPickup -= AchievementUnlock;
            }
        }

        
    }


    void AchievementCheck(bool[] x, int[] _IDs)
    {
        if(x != null && _IDs != null)
        {
            for (int i = 0; i < 100; i++)
            {
                if (_IDs[i] == AchievementId)
                {
                    isUnlocked = x[i];
                    break;
                }
            }
        }

    }

    void GetId(AchievementData data)
    {
        if(data.AchievementId == AchievementId)
        {
            isSameId = true;
        }
        else
        {
            isSameId = false;
        }
        
    }

    public void Update()
    {
        ShowItemIcon();
    }


}



public enum AchievementType
{
    InteractionUnlock,
    EntranceUnlock,
}