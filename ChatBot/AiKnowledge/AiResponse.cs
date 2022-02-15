using System.Text;

namespace ChatBot.AiKnowledge
{
    public class AiResponse
    {
        public string KeyWord { get; set; }
        public string Question { get; set; }
        
        public bool Cringe { get; set; }

        public string Message
        {
            get
            {
                if (KeyWord != null && Question is null)
                {
                    return Answers.AnswerKeyWord(KeyWord);
                }
                
                if (KeyWord is null && Question != null)
                {
                    return Answers.AnswerQuestion(Question);
                }
                
                if (KeyWord != null && Question != null)
                {
                    return Answers.AnswerQuestionAndKeyWord(Question, KeyWord);
                }
                
                return null;
            }
        }
    }
}