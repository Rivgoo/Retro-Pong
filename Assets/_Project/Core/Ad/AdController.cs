﻿using GoogleMobileAds.Api;
using System;
using UnityEngine;

namespace Core
{
    public class AdController : MonoBehaviour
    {
        public event Action ShowedAd;
        public event Action ClosedAd;

        [SerializeField] private bool _isDebugAd;

        [Space]
        [SerializeField] private GameLoop _gameLoop;

        [Space]
        [SerializeField] private RewardedAdController _rewardedAd;
        [SerializeField] private InterstitialAdController _interstitialAd;

        public void ShowRewardAd(Action reward)
        {
            if (_isDebugAd)
            {
                reward?.Invoke();
                return;
            }

            _rewardedAd.ShowAd(reward);
        }
        public void ShowInterstitialAd()
        {
            if (_isDebugAd) return;

            _interstitialAd.ShowAd();
        }

        private void Start()
        {
            MobileAds.Initialize(initStatus => { });
            MobileAds.RaiseAdEventsOnUnityMainThread = true;

            _gameLoop.OnStartLoop += InitializeAd;
        }

        private void InitializeAd()
        {
            _gameLoop.OnStartLoop -= InitializeAd;

            if (_isDebugAd) return;

            _rewardedAd.LoadAd();
            _interstitialAd.LoadAd();

            _rewardedAd.ShowedAd += () => ShowedAd?.Invoke();
            _interstitialAd.ShowedAd += () => ShowedAd?.Invoke();

            _rewardedAd.ClosedAd += () => ClosedAd?.Invoke();
            _interstitialAd.ClosedAd += () => ClosedAd?.Invoke();
        }
    }
}
