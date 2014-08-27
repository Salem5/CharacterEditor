using CharacterEditor.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CharacterEditor.Models
{
    public static class FileTemplateProvider
    {
        private static string projectFileName = "ProjBCharacterProject.xml";
        private static string characterFileName = "CharacterData.xml";
        private static string frameSheetFileName = "FrameSheet.png";
        private static string animationsFileName = "Animations.xml";

        public static string AnimationsFileName
        {
            get { return FileTemplateProvider.animationsFileName; }
            set { FileTemplateProvider.animationsFileName = value; }
        }

        public static string CharacterFileName
        {
            get { return characterFileName; }
            set { characterFileName = value; }
        }

        public static string FrameSheetFileName
        {
            get { return frameSheetFileName; }
            set { frameSheetFileName = value; }
        }

        private static XDocument projectFile = new XDocument(
            new XElement(
                "projBCharacterProject",
                new XAttribute(
                    "author", "nil")
                )
            );

        private static XDocument characterFile = new XDocument(
            new XElement(
                "characterData",
                 new XAttribute("complexName", ""),
                new XElement("frameSheet",
                    new XAttribute("frameHeight", "0"),
                    new XAttribute("frameWidth", "0"),
                    new XAttribute("upperMargin", "0"),
                    new XAttribute("leftMargin", "0"),
                    new XAttribute("rightMargin", "0"),
                    new XAttribute("bottomMargin", "0"),
                    new XAttribute("hasFrameSheet", "false")
                    )
                )
            );

        private static XDocument animationsFile = new XDocument(
          new XElement(
              "animationCollection"
              )
          );

        public static XDocument ProjectFile
        {
            get
            {
                return projectFile;
            }
        }
        public static string ProjectFileName
        {
            private set { projectFileName = value; }
            get { return projectFileName; }
        }

        public static CreationResult CreateAnimationsFile(string argCharacterPath)
        {
            try
            {
                string filePath = Path.Combine(argCharacterPath, animationsFileName);

                XDocument tempXDoc = new XDocument(animationsFile);

                tempXDoc.Save(filePath);
                if (!File.Exists(filePath))
                {
                    return CreationResult.FileCreationFailed;
                }
                return CreationResult.Success;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Debugger.Break();
                return CreationResult.FileCreationFailed;
            }
        }

        public static CreationResult CreateProjectFile(string argProjectDirectoryPath, string argAuthor)
        {
            try
            {

                if (String.IsNullOrEmpty(argAuthor))
                {
                    argAuthor = Environment.UserName;
                }
                string filePath = Path.Combine(argProjectDirectoryPath, projectFileName);
                XDocument tempXDoc = new XDocument(projectFile);

                tempXDoc.Root.Attribute("author").Value = argAuthor;
                tempXDoc.Save(filePath);
                if (!File.Exists(filePath))
                {
                    return CreationResult.FileCreationFailed;
                }
                return CreationResult.Success;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Debugger.Break();
                return CreationResult.FileCreationFailed;
            }
        }

        internal static CreationResult CreateCharacterFile(string argCharacterPath, string argCharacterName)
        {
            try
            {
                string filePath = Path.Combine(argCharacterPath, characterFileName);

                XDocument tempXDoc = new XDocument(characterFile);

                tempXDoc.Root.Attribute("complexName").Value = argCharacterName;

                tempXDoc.Save(filePath);
                if (!File.Exists(filePath))
                {
                    return CreationResult.FileCreationFailed;
                }
                return CreationResult.Success;
            }

            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                //Debugger.Break();
                return CreationResult.FileCreationFailed;
            }
        }
    }
}