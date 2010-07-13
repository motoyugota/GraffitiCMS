using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Graffiti.DocGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("**********************************************************************");
            Console.WriteLine("");
            Console.WriteLine(" Graffiti CMS Document Generator");
            Console.WriteLine(" Usage:   xml_file type_name");
            Console.WriteLine(" Example: docgen Graffiti.Core.xml Graffiti.Core.Macros");
            Console.WriteLine("");

            if (args.Length < 1)
            {
                WriteError("Too few parameters.");
                return;
            }

            if (args.Length > 2)
            {
                WriteError("Too many parameters.");
                return;
            }

            string xmlFile = args[0];
            string type = args[1];

            //string xmlFile = "Graffiti.Core.xml";
            //string type = "Graffiti.Core.Post";

            XmlDocument doc = new XmlDocument();

            try
            {    
                doc.Load(xmlFile);
            }
            catch (Exception e)
            {
                WriteError("Could not load file '" + xmlFile + "'. ");
                return;
            }

            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);

            XmlNode node = doc.SelectSingleNode("doc", mgr);

            Document document = new Document();

            foreach (XmlNode memberNode in node.SelectNodes("members/member"))
            {
                if (memberNode.Attributes["name"].Value.ToLower() == "t:" + type.ToLower())
                {        
                    document.TypeSummary = memberNode.SelectSingleNode("summary").InnerText;
                }

                string className = type.EndsWith(".") ? type : type + ".";

                if (memberNode.Attributes["name"].Value.ToLower().StartsWith("m:" + className.ToLower()) ||
                    memberNode.Attributes["name"].Value.ToLower().StartsWith("p:" + className.ToLower()))
                {
                    DocumentElement docElement = new DocumentElement();

                    string name = memberNode.Attributes["name"].Value;

                    if (name.Contains("#ctor"))
                        break;

                    docElement.MemberName = name.Substring(type.ToLower().Length + 3);
                    docElement.MemberSummary = memberNode.SelectSingleNode("summary").InnerText;

                    if (memberNode.SelectSingleNode("example") != null)
                    {
                        string example = memberNode.SelectSingleNode("example").InnerText.Replace("\r\n", "<br />").Replace("[TAB]", "&nbsp;&nbsp;&nbsp;").Trim();
                        // remove the first <br />
                        if(example.StartsWith("<br />"))
                            example = example.Substring(7);
                        
                        docElement.MemberExample = example;
                    }
                     

                    bool moreThanOneParam = false;
                    if (docElement.MemberName.Contains(",")) // has more than one param
                        moreThanOneParam = true;

                    int param = 1;
                    int lastIndex = 0;

                    string lastParamName = "";

                    foreach (XmlNode paramsNode in memberNode.SelectNodes("param"))
                    {
                       
                        string paramName = " " + paramsNode.Attributes["name"].Value;

                        if (!moreThanOneParam)
                           docElement.MemberName = docElement.MemberName.Insert(docElement.MemberName.IndexOf(")"), paramName);
                        else
                        {
                            lastIndex = docElement.MemberName.IndexOf(",", lastIndex + 2 + lastParamName.Length);

                            if(lastIndex > 0)
                                docElement.MemberName = docElement.MemberName.Insert(docElement.MemberName.IndexOf(","), paramName);
                            else
                                docElement.MemberName = docElement.MemberName.Insert(docElement.MemberName.IndexOf(")"), paramName);

                            lastParamName = paramName;
                        }

                        param++;
                    }

                    docElement.MemberName = docElement.MemberName.Replace(",", ", ");

                    document.DocumentElements.Add(docElement);
                }
            }

            string fileName = type + ".html";

            try
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);

                StreamWriter sw = File.CreateText(fileName);
                
                // write the data
                sw.WriteLine("******** BODY");
                sw.WriteLine("<p>" + document.TypeSummary.Trim() + "</p><p>Listed below are the available methods and properties.</p>");
                sw.WriteLine("******** END BODY");

                sw.WriteLine("");

                sw.WriteLine("******** EXTENDED BODY");

                int boxId = 0;

                foreach(DocumentElement de in document.DocumentElements)
                {
                    sw.WriteLine("<br /><p><strong>" + ReplaceCommonTypes(de.MemberName) + "</strong><br />" + de.MemberSummary.Trim() + "</p>");

                    if (!String.IsNullOrEmpty(de.MemberExample))
                    {
                        sw.WriteLine("<a class=\"a\" onclick=\"toggleBox('exampleBox" + boxId.ToString() + "', this);\">show example &darr;</a>");
                        sw.WriteLine("<div id=\"exampleBox" + boxId + "\" class=\"example\" style=\"display: none;\">");
                        sw.WriteLine(de.MemberExample);
                        sw.WriteLine("</div>");
                    }

                    boxId++;
                }
                sw.WriteLine("******** END EXTENDED BODY");

                sw.Close();

                Console.WriteLine(" " + fileName + " created succesfully!");
            }
            catch (Exception exc)
            {
                WriteError("Creating file " + fileName + " failed. " + exc.Message);
            }

            Console.WriteLine("");
            Console.WriteLine("**********************************************************************");
        }

        private static void WriteError(string error)
        {
            Console.WriteLine("");
            Console.WriteLine(" Error:   " + error);
            Console.WriteLine("");
            Console.WriteLine("**********************************************************************");
        }

        private static string ReplaceCommonTypes(string value)
        {
            return value.Replace("System.String", "string")
                    .Replace("System.Boolean", "bool")
                    .Replace("System.Int32", "int")
                    .Replace("System.Decimal", "decimal");
        }
    }

    internal class Document
    {
        private string _typeSummary;
        private List<DocumentElement> _documentElements = new List<DocumentElement>();

        internal string TypeSummary
        {
            get { return _typeSummary; }
            set { _typeSummary = value; }
        }

        internal List<DocumentElement> DocumentElements
        {
            get { return _documentElements; }
            set { _documentElements = value; }
        }
    }

    internal class DocumentElement
    {
        private string _memberName;
        private string _memberSummary;
        private string _memberExample;

        internal string MemberName
        {
            get { return _memberName; }
            set { _memberName = value; }
        }

        internal string MemberSummary
        {
            get { return _memberSummary; }
            set { _memberSummary = value; }
        }

        internal string MemberExample
        {
            get { return _memberExample; }
            set { _memberExample = value; }
        }
    }
}
