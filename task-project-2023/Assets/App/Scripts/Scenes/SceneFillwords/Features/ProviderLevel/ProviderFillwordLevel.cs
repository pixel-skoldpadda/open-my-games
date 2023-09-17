using System;
using System.Collections.Generic;
using App.Scripts.Infrastructure.LevelSelection;
using App.Scripts.Scenes.SceneFillwords.Config;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private readonly IServiceLevelSelection _serviceLevelSelection;
        private readonly Dictionary<int, GridFillWords> models = new();

        public ProviderFillwordLevel(ConfigLevelSelection levelSelection, ConfigFillwordLevelData fillwordLevelData, 
            IServiceLevelSelection serviceLevelSelection)
        {
            _serviceLevelSelection = serviceLevelSelection;
            
            loadLevels(levelSelection, fillwordLevelData);
        }

        public GridFillWords LoadModel(int index)
        {
            var model = models[index - 1] ?? FindNextValidModel(index);
            if (model == null)
            {
                throw new Exception("All levels are not valid.");
            }

            return model;
        }

        private GridFillWords FindNextValidModel(int index)
        {
            _serviceLevelSelection.UpdateSelectedLevel(index + 1);
            while (_serviceLevelSelection.CurrentLevelIndex != index)
            {
                var levelIndex = _serviceLevelSelection.CurrentLevelIndex;
                var model = models[levelIndex - 1];
                if (model != null)
                {
                    return model;
                }

                _serviceLevelSelection.UpdateSelectedLevel(levelIndex + 1);
            }
            return null;
        }

        private void loadLevels(ConfigLevelSelection levelSelection, ConfigFillwordLevelData fillwordLevelData)
        {
            var dictionary = LoadTextResource(fillwordLevelData.wordsDictionaryResourcePath);
            var levels = LoadTextResource(fillwordLevelData.levelsResourcePath);
            
            for (var i = 0; i < levelSelection.TotalLevelCount; i++)
            {
                models[i] = parseLevel(levels[i], dictionary);
            }
        }

        private GridFillWords parseLevel(string level, IReadOnlyList<string> dictionary)
        {
            var chars = new Dictionary<int, char>(); 
            
            var splitLevel = level.Split(new[] {' '});
            for (var i = 0; i < splitLevel.Length; i++)
            {
                if (i % 2 != 1)
                {
                    continue;
                }
                
                var word = dictionary[int.Parse(splitLevel[i - 1])];
                var positions = splitLevel[i].Split(new[] { ';' });

                if (word.Length != positions.Length)
                {
                    return null;
                }
                    
                var charArray = word.ToCharArray();
                var length = charArray.Length;
                for (var j = 0; j < length; j++)
                {
                    chars[int.Parse(positions[j])] = charArray[j];
                }
            }

            var charsCount = chars.Count;
            if (!IsPerfectSquare(charsCount))
            {
                return null;
            }

            var width = (int) MathF.Sqrt(charsCount);
            var grid = new GridFillWords(new Vector2Int(width, width));

            var maxIndex = charsCount - 1;
            foreach (var (index, ch) in chars)
            {
                if (index > maxIndex)
                {
                    return null;
                }
                grid.Set(index % width, index / width, new CharGridModel(ch));
            }

            return grid;
        }

        private string[] LoadTextResource(string path)
        {
            var textAsset = Resources.Load<TextAsset>(path);
            return textAsset.text.Split("\r\n");
        }

        private bool IsPerfectSquare(int number)
        {
            var odd = 1;
            while (number > 0)
            {
                number -= odd;
                odd += 2;
            }
            return number == 0;
        }
    }
}