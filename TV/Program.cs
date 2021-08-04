﻿using HtmlAgilityPack;
using HttpCode.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.Arm;
using System.Text;

namespace TV
{
    class Program
    {
        static void Main(string[] args)
        { 
            HttpHelpers httpHelpers = new HttpHelpers();
            HttpItems items = new HttpItems();
            Console.WriteLine("请输入需要抓取的链接");
            
            //items.Url = "https://m.193291.com/videodetails/38272.html";//请求地址
            items.Url = Console.ReadLine();
            items.Method = "Get";//请求方式 post 
            HttpResults hr = httpHelpers.GetHtml(items);
            if (!string.IsNullOrWhiteSpace(hr.Html)) 
            { 
               Console.WriteLine("获取网页源完成，开始解析");
               JX(hr.Html);

               Console.WriteLine("解析完成");
               Console.WriteLine("-----------------------------------");
            }
        } 
        /// <summary>
        /// 解析XML
        /// </summary>
        /// <param name="htmlCode"></param>
        public static void JX(string htmlCode)
        {

            //HtmlAgilityPack
            //源码地址：https://html-agility-pack.net/?z=codeplex 
            //下载地址2：https://codeplexarchive.blob.core.windows.net/archive/projects/htmlagilitypack/htmlagilitypack.zip
            string path = System.AppDomain.CurrentDomain.BaseDirectory;
            //var filname = "抓取文件.txt";

            HtmlDocument document = new HtmlDocument(); 
            document.LoadHtml(htmlCode);
            HtmlNode rootNode = document.DocumentNode;
            //categoryNodeList 具有相同类型的节点的集合                //标签@属性='属性名称'
            //HtmlNodeCollection categoryNodeList = rootNode.SelectNodes("//div[@id='content']//li[@id='li3_0']//span[@id='s3p0']");
            //也可以通过Xpath路径的形式获取 Xpath路径可以使用HAPExplorer.exe（通过上面的源码地址可以下载并生成工具）
            HtmlNodeCollection categoryNodeList = rootNode.SelectNodes("/html[1]/body[1]/div[5]/div[1]/ul[1]/li");


            HtmlNodeCollection aa = rootNode.SelectNodes("/html[1]/body[1]/div[2]/div[1]/a[3]");
            var filname = aa[0].InnerText.Trim()+".txt";
            foreach (var item in categoryNodeList)
            {
                var sapn = item.InnerHtml.Trim();
                var herf = sapn.Split('"')[1];
                WriteMessage(path + filname, herf);
            } 
        }

        /// <summary>
        /// 输出指定信息到文本文件
        /// </summary>
        /// <param name="path">文本文件路径</param>
        /// <param name="msg">输出信息</param>
        public static void WriteMessage(string path, string msg)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.BaseStream.Seek(0, SeekOrigin.End);
                    sw.WriteLine("{0}\n", msg, DateTime.Now);
                    sw.Flush();
                }
            }
        }
    }
}