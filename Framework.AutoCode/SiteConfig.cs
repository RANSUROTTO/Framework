using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Linq;

namespace Framework.AutoCode
{
    public class SiteConfig
    {
        static readonly Node MyNode = new Node() { Name = "SiteConfig", Context = new StringBuilder(), List = new List<Node>() };

        static StringBuilder sb = new StringBuilder();

        static string _workItemName;

        public static void Build(string[] args)
        {
            string loanPath = args[0].Replace("\"", "");
            _workItemName = args[1];

            string path = loanPath + @"\Resources\Config.xml";
            string configPath = loanPath + @"\SiteConfig.cs";

            string xml = File.ReadAllText(path, Encoding.UTF8);

            XDocument xdoc = XDocument.Parse(xml);
            xdoc.Element("root")?.Elements("item").ToList().ForEach(t =>
            {
                string name = t.Attribute("name").Value;
                bool isenum = t.Attribute("enum") != null;
                string type = isenum ? t.Attribute("enum").Value : t.Attribute("type").Value;
                string desc = t.Attribute("desc") == null ? "" : t.Attribute("desc").Value;
                string value = t.Value;

                string field;
                string[] nodeName = GetClass(name, out field);

                Node node = GetNode(nodeName);

                if (node.Context == null) node.Context = new StringBuilder();
                node.Context.AppendLine("/// <summary>");

                if (!string.IsNullOrEmpty(desc))
                {
                    node.Context.AppendFormat("/// {0}  ", desc);
                    node.Context.AppendLine();
                }

                node.Context.AppendFormat("///类型:{0}  默认值:{1}", type, value);
                node.Context.AppendLine();
                node.Context.AppendLine("/// </summary>");

                if (!string.IsNullOrEmpty(desc))
                    node.Context.AppendFormat("[Config(\"{0}\",\"{1}\",\"{2}\")]", desc, value, type).AppendLine();

                // node.Context.AppendFormat("public const string {0} = \"{1}\";", field, name);
                node.Context.AppendFormat("public static {0} {1}", TypeName(type), field).Append("{ get{");
                node.Context.AppendFormat("return ({0})SiteAgent.GetConfig(\"{1}\");", TypeName(type), name).Append("} }");
                node.Context.AppendLine();

            });

            //生成静态类
            //Generate static classes
            sb.AppendLine("using System;");
            sb.AppendLine("using Framework.Core.Configuration;");

            /*sb.AppendFormat("using {0}.Agents;", _workItemName).AppendLine();
            sb.AppendLine();*/


            sb.AppendFormat("namespace {0}{{", _workItemName).AppendLine();
            ShowCode(MyNode);
            sb.AppendLine("}");

            //将静态类写入指定目录
            File.WriteAllText(configPath, sb.ToString());
        }

        /// <summary>
        /// 拼接节点数据为类
        /// </summary>
        /// <param name="node"></param>
        static void ShowCode(Node node)
        {
            sb.AppendFormat("public partial class {0}", node.Name);
            sb.AppendLine("{");
            if (node.Name == "SiteConfig")
            {
                sb.AppendFormat("private static SiteAgent SiteAgent").Append(@"{get{return SiteAgent.Instance;  } }").AppendLine();
            }

            //迭代循环
            node.List?.ForEach(ShowCode);

            sb.Append(node.Context?.ToString() ?? "");
            sb.AppendLine("}");
        }

        static string[] GetClass(string name, out string field)
        {
            string[] names = name.Split('.');
            field = names.Last();
            if (names.Length == 1) return new string[] { };
            return names.Take(names.Length - 1).ToArray();
        }

        static Node GetNode(params string[] names)
        {
            Node node = MyNode;
            foreach (var name in names)
            {
                if (node.List == null) node.List = new List<Node>();
                Node child = node.List.Find(t => t.Name == name);
                if (child == null)
                {
                    child = new Node() { Name = name };
                    node.List.Add(child);
                }
                node = child;
            }
            return node;
        }

        /// <summary>
        /// 转换类型标记
        /// Conversion type tag
        /// </summary>
        static string TypeName(string type)
        {
            if (type == "boolean") return "bool";
            return type;
        }

    }

    /// <summary>
    /// 节点类
    /// </summary>
    public class Node
    {
        public string Name { get; set; }

        public StringBuilder Context { get; set; }

        public List<Node> List { get; set; }
    }





}

