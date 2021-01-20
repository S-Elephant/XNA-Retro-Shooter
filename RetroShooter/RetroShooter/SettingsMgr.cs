using System.IO;
using System.IO.IsolatedStorage;
using System.Xml.Linq;
using XNALib;
using System;

namespace RetroShooter
{
    public class SettingsMgr
    {
        public static SettingsMgr Instance;
        const string Path = "Settings.xml";
#if XBOX
        IsolatedStorageFile FileStorage = IsolatedStorageFile.GetUserStoreForApplication();
#endif

        #region Settings
        public bool IsFullScreen = true;
        public bool EnableMusic = true;
        public eControlType ControlType1 = eControlType.Keyboard;
        public bool ShowSpawnTimer = false;
        public bool SkipTutorial = false;
        public bool ShowRoundGUI = true;
        public byte RoundGUIAlpha = 160;
        #endregion

        public SettingsMgr()
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
                    new XElement("IsFullScreen", IsFullScreen.ToString()),
                    new XElement("EnableMusic", EnableMusic.ToString()),
                    new XElement("ControlType1", ControlType1.ToString()),
                    new XElement("ShowSpawnTimer", ShowSpawnTimer.ToString()),
                    new XElement("SkipTutorial", ShowSpawnTimer.ToString()),
                    new XElement("ShowRoundGUI", ShowRoundGUI.ToString()),
                    new XElement("RoundGUIAlpha", RoundGUIAlpha.ToString())
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
                IsFullScreen = bool.Parse(settingsNode.SelectChildElement("IsFullScreen").Value);
                EnableMusic = bool.Parse(settingsNode.SelectChildElement("EnableMusic").Value);
                ControlType1 = (eControlType)Enum.Parse(typeof(eControlType), settingsNode.SelectChildElement("ControlType1").Value);
                ShowSpawnTimer = bool.Parse(settingsNode.SelectChildElement("ShowSpawnTimer").Value);
                SkipTutorial = bool.Parse(settingsNode.SelectChildElement("SkipTutorial").Value);
                ShowRoundGUI = bool.Parse(settingsNode.SelectChildElement("ShowRoundGUI").Value);
                RoundGUIAlpha = byte.Parse(settingsNode.SelectChildElement("RoundGUIAlpha").Value);
            }
            catch { }
        }
    }
}
