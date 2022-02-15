using System;
using System.Collections.Generic;
using System.Linq;
using ChatBot.Properties;

namespace ChatBot.AiKnowledge
{
    public class Answers
    {
        public static Dictionary<string, Dictionary<string, List<string>>> AllAiAnswers;
        private static Random _random = new Random();

        static Answers()
        {
            AllAiAnswers = new Dictionary<string, Dictionary<string, List<string>>>();
            var resourceList = new List<string>()
            {
                Resources.Ответы_Имена_собственные,
                Resources.Ответы_Предметы,
                Resources.Ответы_Способности,
                Resources.Ответы_Ключевые_слова,
            };

            foreach (var resource in resourceList)
            {
                var lines = resource.Split('\n');
                foreach (var line in lines)
                {
                    var firstSplit = line.Split(':');
                    if (firstSplit.Length < 3)
                    {
                        continue;
                    }
                    var keyWord = firstSplit[0];
                    var question = firstSplit[1];
                    var list = firstSplit[2].Split('_').ToList();

                    if (!AllAiAnswers.ContainsKey(keyWord))
                        AllAiAnswers[keyWord] = new Dictionary<string, List<string>>();
                    AllAiAnswers[keyWord][question] = list;
                }
            }
        }

        public static string AnswerQuestion(string question)
        {
            return AnswerQuestionAndKeyWord(question, "slardar");
        }

        public static string AnswerKeyWord(string keyWord)
        {
            return AnswerQuestionAndKeyWord("субъект", keyWord);
        }

        public static string AnswerQuestionAndKeyWord(string question, string keyWord)
        {
            if (AllAiAnswers.TryGetValue(keyWord, out var kvpQuestionDictionary))
                if (kvpQuestionDictionary.TryGetValue(question, out var list))
                    return GetRandomElement(list);

            return null;
        }

        private static string GetRandomElement(IList<string> list)
        {
            return list[_random.Next(list.Count - 1)];
        }
    }
}