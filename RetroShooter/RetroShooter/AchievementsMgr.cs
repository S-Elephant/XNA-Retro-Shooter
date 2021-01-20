using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using XNALib;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace RetroShooter
{
    public class AchievementsMgr
    {
        public static readonly Texture2D Lvl30Tex = Common.str2Tex("Achievements/lvl30");
        public static readonly Texture2D Lvl60Tex = Common.str2Tex("Achievements/lvl60");
        public static readonly Texture2D Kill100Tex = Common.str2Tex("Achievements/kill100");
        public static readonly Texture2D Kill500Tex = Common.str2Tex("Achievements/kill500");
        public static readonly Texture2D Kill1000Tex = Common.str2Tex("Achievements/kill1000");

        public static AchievementsMgr Instance;
        const string Path = "Achievements.xml";
#if XBOX
        IsolatedStorageFile FileStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif

        #region Achievements
        public bool BeatedLvl30 = false;
        public bool BeatedLvl60 = false;
        public bool Killed100 = false;
        public bool Killed500 = false;
        public bool Killed1000 = false;
        public eControlType ControlType1 = eControlType.Keyboard;
        #endregion

        public AchievementsMgr()
        {
#if WINDOWS
            if (File.Exists(Path))
#endif
#if XBOX
            if(FileStorage.FileExists(Path))
#endif
                Load();
            else
                Save();
        }

        
        public void Save()
        {
            try
            {
                XDocument doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), new XElement("root"));

                XElement settingsNode = new XElement("Settings");
                doc.Root.Add(settingsNode);

                settingsNode.Add(
                    new XElement("BeatedLvl30", BeatedLvl30.ToString()),
                    new XElement("BeatedLvl60", BeatedLvl60.ToString()),
                    new XElement("Killed100", Killed100.ToString()),
                    new XElement("Killed500", Killed500.ToString()),
                    new XElement("Killed1000", Killed1000.ToString())
                    );


                // Save
#if WINDOWS
            doc.Save(Path, SaveOptions.None);
#endif
#if XBOX
                if (FileStorage.FileExists(Path))
                    FileStorage.DeleteFile(Path);
                IsolatedStorageFileStream stream = FileStorage.CreateFile(Path);
                doc.Save(stream);
                stream.Close();
#endif
            }
            catch { }
        }

        public void Load()
        {
            try
            {
#if WINDOWS
            XDocument doc = XDocument.Load(Path);
#endif
#if XBOX
                StreamReader stream = new StreamReader(new IsolatedStorageFileStream(Path, FileMode.Open, FileStorage));
                XDocument doc = XDocument.Load(stream);
                stream.Close();
#endif
                XElement settingsNode = doc.Root.SelectChildElement("Settings");
                BeatedLvl30 = bool.Parse(settingsNode.SelectChildElement("BeatedLvl30").Value);
                BeatedLvl60 = bool.Parse(settingsNode.SelectChildElement("BeatedLvl60").Value);
                Killed100 = bool.Parse(settingsNode.SelectChildElement("Killed100").Value);
                Killed500 = bool.Parse(settingsNode.SelectChildElement("Killed500").Value);
                Killed1000 = bool.Parse(settingsNode.SelectChildElement("Killed1000").Value);
            }
            catch { }
        }
    }
}
