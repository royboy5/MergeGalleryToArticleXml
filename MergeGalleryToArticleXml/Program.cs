using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace MergeGalleryToArticleXml
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length < 2)
            {                                
                Console.WriteLine("Please enter a numeric argument.");
                Console.WriteLine("MergeGalleryToArticleXml <folder year> <output filename>");
            }

            else
            {
                string year = args[0];
                string xmlFilename = year + @"\export.xml";
                string filename = args[1];
                int count = 0;
                int galleryCount = 0;

                Console.WriteLine("Reading xml file...");

                XDocument xmldoc = XDocument.Load(xmlFilename);

                var articles = from x in xmldoc.Descendants("article")
                               select new
                               {
                                   Gallery = x.Descendants("gallery_link").First().Value
                               };

                foreach (var article in articles)
                {
                    if (article.Gallery != "")
                    {
                        galleryCount++;

                        string fn = System.IO.Path.GetFileName(new Uri(article.Gallery).LocalPath);
                        string galleryFile = year + @"\galleries\" + fn;

                        Console.WriteLine("filename: " + fn);

                        Console.WriteLine("{1}: Gallery Link: {0}", article.Gallery, count);

                        XDocument galleryDoc = XDocument.Load(galleryFile);
                        XElement gallery = galleryDoc.Element("gallery");

                        xmldoc.Element("articles").Elements("article").ElementAt(count).Add(gallery);
                        xmldoc.Save(year + "\\" + filename);

                    }
                    count++;
                }

                Console.WriteLine("Number of galleries {0}", galleryCount);
            }
        }
    }
}
