using UnityEngine;

namespace ShrinkMe
{
    public class SE_Shrink : SE_Stats
    {
        public void OnEnable()
        {
            m_name = "SE_Shrink";
            m_ttl = 30;
            m_cooldown = 60;
        }


        public override void UpdateStatusEffect(float dt)
        {
            if (Player.m_localPlayer != null)
            {
                var player = Player.m_localPlayer;
                player.transform.Find("Visual").localScale = new Vector3(0.25f, 0.25f, 0.25f);
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