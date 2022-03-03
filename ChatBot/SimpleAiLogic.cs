using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ChatBot.AiKnowledge;
using ChatBot.Properties;

namespace ChatBot
{
    public class SimpleAiLogic : IAiLogic
    {
        private static Dictionary<string, HashSet<string>> Questions;
        private static SortedDictionary<string, HashSet<string>> KeyWords;

        private Random _random = new Random();
        private AiResponse _lastResponse = new AiResponse(){Question = "субъект", KeyWord = "slardar"};
        private AiResponse _userRequest = new AiResponse();
        
        private string GetRandomFromList(string resource)
        {
            var list = resource.Split('\n').ToList();
            return list[_random.Next(list.Count - 1)];
        }
        
        public string DecideMessage(string userMessage)
        {
            var questionMark = userMessage.Contains('?');
            _userRequest = FindQuestionAndKeyWord(userMessage);

            var newResponse = new AiResponse();
            if (_userRequest.Cringe)
            {
                _lastResponse = null;
                return GetRandomFromList(Resources.Непонятно);
            }
            
            if (string.IsNullOrEmpty(_userRequest.KeyWord))
            {
                if (_userRequest.Question is null)
                {
                    return GetRandomFromList(Resources.Непонятно);
                }
                
                newResponse.Question = _userRequest.Question;
                if (_lastResponse is null)
                {
                    return GetRandomFromList(Resources.Непонятно);
                }
                newResponse.KeyWord = _lastResponse.KeyWord;
                
            }
            else if(string.IsNullOrEmpty(_userRequest.Question))
            {
                if (questionMark)
                {
                    newResponse.KeyWord = "утверждение";
                }
                else
                {
                    _lastResponse = _userRequest;
                    return GetRandomFromList(Resources.Утверждения);
                }
            }
            else
            {
                newResponse.Question = _userRequest.Question;
                newResponse.KeyWord = _userRequest.KeyWord;
            }

            _lastResponse = newResponse;
            if (newResponse.Message is null)
                return GetRandomFromList(Resources.Непонятно);
            
            return newResponse.Message;
        }


        private bool CheckAllCringeMoments(string word)
        {
            if (word.Length > 25)
            {
                return true;
            }

            if (word.Any(c => c >= 'a' && c <= 'z' || c>= 'A' && c<='Z') 
                && word.Any(c => c >= 'а' && c <= 'я' || c>= 'А' && c<='Я'))
            {
                return true;
            }
            
            if (word.Any(c => c >= '0' && c <= '9'))
            {
                if (word.Any(c => c >= 'a' && c <= 'z' || c>= 'A' && c<='Z'))
                {
                    return true;
                }

                if (word.Any(c => c >= 'а' && c <= 'я' || c>= 'А' && c<='Я'))
                {
                    return true;
                }
            }

            return false;
        }
        
        private AiResponse FindQuestionAndKeyWord(string userMessage)
        {
            var words = userMessage.Split(".!?;:()) ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (words.Any(CheckAllCringeMoments))
            {
                return new AiResponse() { Cringe = true };
            }

            var question = FindExactWord(words, Questions);
            var keyWord = FindExactWord(words, KeyWords);
            return new AiResponse() { Question = question, KeyWord = keyWord };
        }

        private string FindExactWord(string[] words, IDictionary<string, HashSet<string>> dictionary)
        {
            var priority = dictionary.Count;
            var result = string.Empty;
            foreach (var word in words)
            {
                var currentPriority = 0;
                foreach (var questionWord in dictionary)
                {
                    currentPriority++;
                    if (questionWord.Value.Contains(word))
                    {
                        if (currentPriority < priority)
                        {
                            priority = currentPriority;
                            result = questionWord.Key;
                            break;
                        }
                    }
                }
            }
            
            return result;
        }

        static SimpleAiLogic()
        {
            Questions = new Dictionary<string, HashSet<string>>();
            var lines = Resources.Вопросы.Split('\n');
            foreach (var line in lines)
            {
                var firstSplit = line.Split(':');
                var key = firstSplit[0];
                var set = new HashSet<string>(firstSplit[1].Split(new []{'_','\r'}));
                Questions[key] = set;
            }

            KeyWords = new SortedDictionary<string, HashSet<string>>();
            foreach (var resource in new[]
                { Resources.Имена_собственные, Resources.Предметы, Resources.Способности, Resources.Ключевые_слова })
            {
                var resourceLines = resource.Split('\n');
                foreach (var line in resourceLines)
                {
                    var firstSplit = line.Split(':');
                    var key = firstSplit[0];
                    var set = new HashSet<string>(firstSplit[1].Split(new []{'_','\r'}));
                    KeyWords[key] = set;
                }
            }
        }
    }
}