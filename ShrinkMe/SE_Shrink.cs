using System.Linq;
using UnityEngine;

namespace ShrinkMe
{
    public class SE_Shrink : SE_Stats
    {
        internal static int randomnum;
        public void OnEnable()
        {
            m_name = "SE_Shrink";
            m_icon = ShrinkMe.HaldorPipe?.Prefab.GetComponent<ItemDrop>().m_itemData.m_shared.m_icons.First();
            
        }

        public override void Setup(Character character)
        {
            int dice = 12;
            randomnum = dice.RollDice();
            base.Setup(character);
        }

        public override void UpdateStatusEffect(float dt)
        {
            if (Player.m_localPlayer != null)
            {
                var player = Player.m_localPlayer;
                player.transform.Find("Visual").localScale = randomnum == ShrinkMe.luckyno.Value ? new Vector3(ShrinkMe.biggestsize.Value, ShrinkMe.biggestsize.Value, ShrinkMe.biggestsize.Value) : new Vector3(ShrinkMe.smallestshrink.Value, ShrinkMe.smallestshrink.Value, ShrinkMe.smallestshrink.Value);
                
            }
            base.UpdateStatusEffect(dt);
        }

        public override void Stop()
        {
            if (Player.m_localPlayer != null)
            {
                var player = Player.m_localPlayer;
                player.transform.Find("Visual").localScale = new Vector3(0.95f, 0.95f, 0.95f);
            }
            base.Stop();
        }
    }
}