using System;
using GoogleMobileAds.Api;
using UnityEngine;

namespace AD
{
    public class RewAd: MonoBehaviour
    {
        private string RewardedUnityId = "ca-app-pub-1711916930861691/9936148580";
        private RewardedAd rewardedAd;

        private void OnEnable()
        {
            rewardedAd=new RewardedAd(RewardedUnityId);
            AdRequest adRequest=new AdRequest.Builder().Build();
            rewardedAd.LoadAd(adRequest);

            rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        }

        private void HandleUserEarnedReward(object sender, Reward e)
        {
            GameManager.Instance.AddAdReward();
        }


        public void ShowAd()
        {
            if (rewardedAd.IsLoaded())
            {
                rewardedAd.Show();
            }
        }
    }
}