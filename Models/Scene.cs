namespace WOZStoryBuilder.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Scene
    {
        public int ID { get; set; }
        public string Name { get; set; }
        //public Area Area { get; set; }
        public string? DialogueText { get; set; }
        public List<SceneChoice> Choices { get; set; }
        public Scene(int id, string name, string dialogueText, List<SceneChoice> choices)
        {
            ID = id;
            Name = name;
            DialogueText = dialogueText;
            Choices = choices;
        }
    }
}
