using System.Collections.Generic;
using System.IO;
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
            var fileNames = Directory.GetFiles(searchWordLevelData.levelsDirectoryPath, "*.json");
            for (var i = 0; i < fileNames.Length; i++)
            {
                var json = File.ReadAllText(fileNames[i]);
                _levels[i] = JsonUtility.FromJson<LevelInfo>(json);
            }
        }

        public LevelInfo LoadLevelData(int levelIndex)
        {
            return _levels[levelIndex - 1];
        }
    }
}