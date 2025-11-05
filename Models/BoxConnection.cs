namespace WOZStoryBuilder.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Shapes;

    using WOZStoryBuilder.Components;

    public class BoxConnection
    {
        public StoryBoard_SceneBox From;
        public StoryBoard_SceneBox To;
        public Line Line;
        public Polygon Arrow;
    }
}
