using System.Collections.Generic;
using App.Scripts.Scenes.SceneWordSearch.Config;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        private readonly Dictionary<int, LevelInfo> _levels = new();
        
        public ProviderWordLevel(ConfigSearchWordLevelData searchWordLevelData)
        {
            LoadAllLevels(searchWordLevelData);
        }

        private void LoadAllLevels(ConfigSearchWordLevelData searchWordLevelData)
        {
            var textAssets = Resources.LoadAll<TextAsset>(searchWordLevelData.levelsResourcesPath);
            for (var i = 0; i < textAssets.Length; i++)
            {
                _levels[i] = JsonUtility.FromJson<LevelInfo>(textAssets[i].text);
            }
        }

        public LevelInfo LoadLevelData(int levelIndex)
        {
            return _levels[levelIndex - 1];
        }
    }
}