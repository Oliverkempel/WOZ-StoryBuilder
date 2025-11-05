namespace WOZStoryBuilder.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SceneChoice
    {
        public string? Description { get; set; }
        public int SceneId { get; set; }
        public Scene? SceneObj { get; set; }
        public SceneChoice(int sceneId, string description)
        {
            SceneId = sceneId;
            Description = description;
        }
    }
}
