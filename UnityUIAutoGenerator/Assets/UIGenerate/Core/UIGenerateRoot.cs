using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UIGenerate.Core;
using UnityEditor;
using UnityEngine;

namespace UIGenerate.Editor
{
    [ExecuteInEditMode]
    public class UIGenerateRoot : SerializedMonoBehaviour
    {
#if UNITY_EDITOR
        
        const string CSPath = "UIGenerate/Generate";
        const string LuaPath = "UIGenerate/Generate";

        [ShowInInspector]
        List<UIGenerateTag.ComponentInfo> allComponentInfos = new List<UIGenerateTag.ComponentInfo>();

        private void Start()
        {
            AddGenerateTagOnChridren();
        }

        [LabelText("获取需要生成代码的组件")]
        [Button(ButtonSizes.Medium)]
        void AddGenerateTagOnChridren()
        {
            allComponentInfos.Clear();
            var childrenTrans = gameObject.GetComponentsInChildren<Transform>();
            childrenTrans.ToList().ForEach(trans =>
            {
                var tag = trans.gameObject.GetComponent<UIGenerateTag>();
                if (tag == null)
                    tag = trans.gameObject.AddComponent<UIGenerateTag>();
                tag.SetInfos();

                var creates = tag.ComponentInfos.FindAll(c => c.create);
                allComponentInfos.AddRange(creates);
            });
        }

        [LabelText("生成cs代码")]
        [Button(ButtonSizes.Medium)]
        void CreateCsCode()
        {
            AddGenerateTagOnChridren();
            StringBuilder sb = new StringBuilder();
            sb.Append("//This script is tool generated. Do not modify it manually\n\n");
            sb.Append("using UnityEngine;\n");
            sb.Append("using UIGenerate.Core;\n");
            sb.Append("\n");

            var className = "UIFinder" + Regex.Replace(gameObject.name, @"^\w", t => t.Value.ToUpper());

            sb.Append($"public class {className} : UIFinderBase\n");
            sb.Append("{\n");

            allComponentInfos.ForEach(c =>
            {
                var propertyName = Regex.Replace(c.name, @"^\w", t => t.Value.ToUpper());
                sb.Append($"\tpublic {c.component.GetType().FullName} {propertyName} {{ get; private set; }}\n");
            });

            sb.Append("\n");
            sb.Append("\tpublic override void Init(Transform trans)\n");
            sb.Append("\t{\n");
            sb.Append("\t\tbase.Init(trans);\n");
            allComponentInfos.ForEach(c =>
            {
                var propertyName = Regex.Replace(c.name, @"^\w", t => t.Value.ToUpper());
                if (c.component.name == gameObject.name)
                {
                    sb.Append($"\t\t{propertyName} = Base.GetComponent<{c.component.GetType().FullName}>();\n");
                }
                else
                {
                    sb.Append($"\t\t{propertyName} = trans.Find(\"{c.path}\").GetComponent<{c.component.GetType().FullName}>();\n");
                }
            });

            sb.Append("\t}\n");
            sb.Append("}");

            SaveFile(sb, className, "cs", CodeType.cs);
        }

        [LabelText("生成lua代码")]
        [Button(ButtonSizes.Medium)]
        void CreateLuaCode()
        {
            AddGenerateTagOnChridren();
            StringBuilder sb = new StringBuilder();
            sb.Append("--This script is tool generated. Do not modify it manually\n\n");

            var className = "UIFinder" + Regex.Replace(gameObject.name, @"^\w", t => t.Value.ToUpper());

            sb.Append($"local {className}  = {{ }}\n");
            sb.Append("\n");

            allComponentInfos.ForEach(c =>
            {
                var propertyName = Regex.Replace(c.name, @"^\w", t => t.Value.ToUpper());
                sb.Append($"{className}.{propertyName} = nil\n");
            });

            sb.Append("\n");
            sb.Append($"function {className}:Init(trans)\n");
            allComponentInfos.ForEach(c =>
            {
                var propertyName = Regex.Replace(c.name, @"^\w", t => t.Value.ToUpper());
                if (c.component.name == gameObject.name)
                {
                    sb.Append($"\tself.{propertyName} = trans:GetComponent<{c.component.GetType().Name}>();\n");
                }
                else
                {
                    sb.Append($"\tself.{propertyName} = trans:Find(\"{c.path}\"):GetComponent(\"{c.component.GetType().Name}\")\n");
                }
            });
            sb.Append($"end\n");
            sb.Append("\n");
            sb.Append($"return {className}");

            SaveFile(sb, className, "lua", CodeType.lua);
        }

        private void SaveFile(StringBuilder sb, string flieName, string fileType, CodeType codeType)
        {
            var codePtah = CSPath;
            switch (codeType)
            {
                case CodeType.cs: codePtah = CSPath; break;
                case CodeType.lua: codePtah = LuaPath; break;
                default: break;
            }
            string dirPath = $"{Application.dataPath}/{codePtah}";
            if (!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);

            string codeFullPath = $"{dirPath}/{flieName}.{fileType}";

            File.WriteAllText(codeFullPath, sb.ToString());

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("提示", "生成成功", "确定");
        }

        enum CodeType { cs, lua }
        
#endif
    }
}
