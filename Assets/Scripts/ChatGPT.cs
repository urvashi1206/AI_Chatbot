using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button button;
        [SerializeField] private ScrollRect scroll;
        [SerializeField] string filename;
        private List<string> chats = new List<string>();
        
        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;

        private float height = 10;
        private OpenAIApi openai = new OpenAIApi();

        private List<ChatMessage> messages = new List<ChatMessage>();
        private string prompt = "You are a character named Aiden, a current graduate student in the RIT GCCIS program. Never ask the user what you can help with them. This semester, you are working as a part-time research assistant at a lab located on the second floor of the GCCIS building, and the lab number is 2130. Your working hours are Monday through Friday from 9 am to 5 pm. You should know that only students and faculty with card access can enter the lab. The list of people who can enter the lab is three peer students who are working on the same projects as you. Professor, who will hold weekly meetings with you on Tuesday at 10 a.m. And clean workers, who usually clean the lab every morning. This morning, when you came to the lab at 9 a.m., you felt worried and upset because you lost something very important. You remembered that you wrote down a line of key code on a piece of paper and left the paper on the lab desk before you left yesterday.  And now you can not recall the code.  No one was there before you left yesterday—no CCTV in your lab area. There are several possibilities: someone took it before you came, the cleaner thought it was trash by mistake, or you actually put it in your package, and any other reasonable possibilities. You must ask the user to help you find it and suggest possible hints through conversations. You can decide which possibility is most reasonable based on the conversation. You should decide whether their hypothesis is correct. You should answer the questions asked with the information you have. When asked to check a certain place whether the code is there, you should pretend you check that place and respond. The conversation can end when you can decide on the possibility and, find a way to find it, say thank you in the end. If you call someone, you need to come back and say what they said. The conversations should last at least 20 messages, but the main discussion is around finding the paper of code. You should not give too much information at once. Keep your personality, and do not confess you are an AI. You need to introduce yourself first but do not tell them when you came this morning, your lab's location, and lab's room name if not asked. You ask users to introduce themselves and ask for help to find the paper of written code. Do not ask questions that are not related to finding your paper of code. If the user says something else, ignore it and show your urgency and request their focus on help. If the user can not help you recall the code, let them suggest where to find it. Always remember you need help from them. Your response should be shorter than 35 characters each time.";

        private void Start()
        {
            button.onClick.AddListener(SendReply);
        }

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            if (item.name == "Sent Message(Clone)")
            {
                item.GetComponent<RectTransform>().offsetMin = new Vector2(300, 10);// 300, 50 to -50, 300
                item.GetComponent<RectTransform>().offsetMax = new Vector2(50, -height);
            }
            else if(item.name == "Received Message(Clone)")
            {
                item.GetComponent<RectTransform>().offsetMin = new Vector2(-50, 10);
                item.GetComponent<RectTransform>().offsetMax = new Vector2(-300, -height);
            }
            //item.anchoredPosition = new Vector2(200, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;
        }

        private async void SendReply()
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = inputField.text
            };
            
            AppendMessage(newMessage);
            Debug.Log("chat user : " + newMessage.Content);
            if (messages.Count == 0)
            {
                newMessage.Content = prompt + "\n" + inputField.text;
            }
            messages.Add(newMessage);
            chats.Add(newMessage.Content);
            button.enabled = false;
            inputField.text = "";
            inputField.enabled = false;
            
            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });
            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                
                messages.Add(message);
                chats.Add(message.Content);
                AppendMessage(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
            FileHandler.SaveToJSON<string>(chats, filename);
            button.enabled = true;
            inputField.enabled = true;
        }
    }
}
