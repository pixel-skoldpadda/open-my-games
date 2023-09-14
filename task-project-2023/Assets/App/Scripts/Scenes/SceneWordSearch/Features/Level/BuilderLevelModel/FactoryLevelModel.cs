using System.Collections.Generic;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            var chars = new List<char>(words[0].ToCharArray());
            for (var i = 1; i < words.Count; i++)
            {
                var charArray =  words[i].ToCharArray();

                var copy = new List<char>(chars);
                foreach (var ch in charArray)
                {
                    if (copy.Contains(ch))
                    {
                        copy.Remove(ch);
                    }
                    else
                    {
                        chars.Add(ch);
                    }
                }
            }
            return chars;
        }
    }
}