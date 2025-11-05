using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using WOZStoryBuilder.Models;

namespace WOZStoryBuilder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Scene> Scenes { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            this.LoadScenes();
            this.LinkScenes();
        }

        public void LinkScenes()
        {
            // Loop through all scenes
            foreach (Scene scene in Scenes)
            {
                // Loop through all scenechoices in theese scenes
                foreach (SceneChoice sceneChoice in scene.Choices)
                {
                    // try to resolve the name of the scene with a scene object
                    if (Scenes.Exists(x => x.ID == sceneChoice.SceneId))
                    {
                        // set the scene object on the scenechoice object to the found instance
                        sceneChoice.SceneObj = Scenes.Where(x => x.ID == sceneChoice.SceneId).FirstOrDefault();
                    }
                }
            }
        }

        public void LoadScenes()
        {
            string dummyDialougeText = "This is some thing";
            // Scene: Start 1 Location: Indkørsel
            Scenes = new List<Scene> {
                new Scene(0, "Starten 1", dummyDialougeText,
                new List<SceneChoice> {
                    new SceneChoice(1, "Gå ind af hovedøren"),
                    new SceneChoice(2, "Smid dine ting foran døren og gå om på terassen")
                }),

            // Scene 2: Starten 2; Location: Entré
            new Scene(1, "Starten 2", dummyDialougeText,
                new List<SceneChoice> {
                    new SceneChoice(4, "Gå ind i stuen og hils på din mand"),
                    new SceneChoice(3, "Gå direkte til dit værelse")
                }),

            // Scene 3: Starten 3; Location: Terrassen
            new Scene(2, "Starten 3", dummyDialougeText,
                new List<SceneChoice> {
                    new SceneChoice(5, "Gå ud i haven"),
                    new SceneChoice(6, "Gå ind i køkken alrummet")
                }),




            // Scene 4: Starten 4; Location: Soveværelse
            new Scene(3, "Starten 4", dummyDialougeText,
                new List<SceneChoice> {
                    new SceneChoice(2, "Åben døren og gå ud i stuen"),
                }),

            // Scene 5: Starten 5; Location: Stue
            new Scene(4, "Starten 5", dummyDialougeText,
                new List<SceneChoice> {
                    new SceneChoice(2, "Gå på toilettet"),
                }),


            // Scene 6: Starten 6; Location: Haven
            new Scene(5, "Starten 6", dummyDialougeText,
                new List<SceneChoice> {
                    new SceneChoice(7, "END"),
                }),

            // Scene 7: Starten 7; Location: Køkken alrum
            new Scene(6, "Starten 7", dummyDialougeText,
                new List<SceneChoice> {
                    new SceneChoice(7, "END"),
                }),


            //DUMMY END SCENE
            new Scene(7, "END", "!!!END!!!", new List<SceneChoice> { })
            };

        }

    }
}