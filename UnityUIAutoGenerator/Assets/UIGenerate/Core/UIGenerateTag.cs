using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UIGenerate.Editor;
using UnityEngine;

namespace UIGenerate.Core
{
    [TypeInfoBox("UI查找代码生成标记")]
    public class UIGenerateTag : SerializedMonoBehaviour
    {
#if UNITY_EDITOR

        [LabelText("是否通过父物体命名")]
        public bool namedParent;

        [LabelText("该子组件的路径")]
        public string path;

        [LabelText("生成代码的物体名")]
        public string value = "";

        [LabelText("需要生成代码组件信息")]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
        public List<ComponentInfo> ComponentInfos = new List<ComponentInfo>();

        public void SetInfos()
        {
            if (string.IsNullOrEmpty(value))
            {
                value = namedParent ? $"{transform.parent.name}_{gameObject.name}" : gameObject.name;
            }

            if (ComponentInfos.Count == 0)
            {
                var list = gameObject.GetComponents<Component>().ToList();
                list.Remove(this);
                ComponentInfos = list.Select(c => new ComponentInfo(c, path)).ToList();
            }

            path = GetPath();
            ComponentInfos.ForEach(c => c.path = path);
        }

        string GetPath()
        {
            string path = $"/{transform.name}";
            void CreatePath(Transform trans)
            {
                var parent = trans.parent;
                if (parent != null)
                {
                    var root = parent.gameObject.GetComponent<UIGenerateRoot>();
                    if (root == null)
                    {
                        path = $"/{parent.name}{path}";
                        CreatePath(parent);
                    }
                }
            }
            if (gameObject.GetComponent<UIGenerateRoot>() == null)
            {
                CreatePath(transform);
            }

            path = path.Substring(1);
            return path;
        }

        [InlineProperty]
        [Serializable]
        public class ComponentInfo
        {
            [LabelText("需要生成代码的组件")]
            public Component component;

            public bool create = false;

            [LabelText("生成代码的组件名")]
            [ShowIf("create", true)]
            public string name;

            [LabelText("查找组件的路径")]
            public string path;

            public ComponentInfo(Component component, string path)
            {
                this.component = component;
                var tempName = $"{component.GetType().Name}_{component.name}";
                name = Regex.Replace($"{tempName}", @"^\w", t => t.Value.ToUpper());
                this.path = path;
            }

#endif
        }
    }
}