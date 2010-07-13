using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace Graffiti.Package
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("**********************************************************************");
            Console.WriteLine("");
            Console.WriteLine(" Graffiti CMS Package Utility");
            Console.WriteLine(" Usage:   package_name path_to_package unpack_to version(optional)");
            Console.WriteLine(" Example: package \"CoolPack\" \"C:\\MyGraffitiPlugInPackage\" \"/\" \"1.5\"");
            Console.WriteLine("          (\"/\" will unpack this file to the graffiti root directory)");

            bool hasErrors = false;

            if (args.Length == 0)
            {
                WriteError("No parameters were supplied.");
                hasErrors = true;
            }

            if (args.Length == 1)
            {
                if (!args[0].EndsWith("?"))
                {
                    WriteError("Not enough parameters were supplied.");
                    hasErrors = true;
                }
                else
                {
                    Console.WriteLine("");
                    Console.WriteLine("**********************************************************************");
                    return;
                }
            }

            if (args.Length == 2)
            {
                WriteError("Not enough parameters were supplied.");
                hasErrors = true;
            }

            if (args.Length > 4)
            {
                WriteError("Too many parameters were supplied.");
                hasErrors = true;
            }

            if (!hasErrors)
            {
                string packageName = args[0];
                string pathName = args[1];
                string unpackTo = args[2];
                
                string packageVersion = null;

                if (args.Length > 3)
                    packageVersion = args[3];

                // check if the path exists
                if (Directory.Exists(pathName))
                {
                    string packageFile = packageName;

                    if (packageName.ToLower().EndsWith(".xml"))
                        packageName = packageName.Remove(packageName.Length - 4, 4);

                    if (!packageFile.ToLower().EndsWith(".xml"))
                        packageFile += ".xml";

                    Console.WriteLine("");
                    Console.WriteLine(" Writing Package " + packageName + " ...");

                    string xml = ToXML(packageName, pathName, unpackTo, packageVersion);

                    string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string file = Path.Combine(path, packageFile);

                    try
                    {
                        if (File.Exists(file))
                            File.Delete(file);

                        StreamWriter sw = File.CreateText(file);
                        sw.Write(xml);
                        sw.Close();

                        Console.WriteLine("");
                        Console.WriteLine(" Package created succesfully!");
                    }
                    catch (Exception exc)
                    {
                        WriteError("Creating file " + packageFile + " failed. " + exc.Message);
                    }
                }
                else
                {
                    WriteError("The path specified does not exist.");
                }
            }

            Console.WriteLine("");
            Console.WriteLine("**********************************************************************");
        }

        private static void WriteError(string error)
        {
            Console.WriteLine("");
            Console.WriteLine(" Error:   " + error);
        }

        private static void WriteFileAdd(string file)
        {
            Console.WriteLine(" Adding File:   " + file);
        }

        public static string ToXML(string name, string path, string unpackTo, string packageVersion)
        {
            Console.WriteLine("");

            DirectoryInfo di = new DirectoryInfo(path);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            XmlTextWriter writer = new XmlTextWriter(sw);
            writer.Formatting = Formatting.Indented;
            writer.WriteRaw("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            writer.WriteStartElement("package");
            writer.WriteAttributeString("name", name);
            writer.WriteAttributeString("location", unpackTo);

            if (packageVersion != null)
                writer.WriteAttributeString("version", packageVersion);

            DirectoryToXML(di, writer, true);

            writer.WriteEndElement();

            return sb.ToString();
        }

        private static void DirectoryToXML(DirectoryInfo di, XmlTextWriter writer, bool root)
        {
            if (!root)
            {
                writer.WriteStartElement("folder");
                writer.WriteAttributeString("name", di.Name);
            }

            FileInfo[] files = di.GetFiles();
            if (files != null && files.Length > 0)
            {
                writer.WriteStartElement("files");

                foreach (FileInfo fi in files)
                {
                    WriteFileAdd(fi.FullName);

                    writer.WriteStartElement("file");
                    writer.WriteAttributeString("name", fi.Name);

                    writer.WriteAttributeString("type", "base64");

                    using (FileStream fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read))
                    {
                        byte[] ba = new byte[fs.Length];
                        fs.Read(ba, 0, ba.Length);
                        fs.Close();

                        string base64 = Convert.ToBase64String(ba);
                        writer.WriteCData(base64);
                    }

                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
            }

            DirectoryInfo[] children = di.GetDirectories();

            if (children != null && children.Length > 0)
            {
                writer.WriteStartElement("folders");
                foreach (DirectoryInfo diChild in children)
                {
                    DirectoryToXML(diChild, writer, false);
                }
                writer.WriteEndElement();
            }

            if(!root)
                writer.WriteEndElement();
        }
    }
}
