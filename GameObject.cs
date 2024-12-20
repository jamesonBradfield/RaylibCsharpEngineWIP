using Raylib_cs;
using ImGuiNET;
namespace Engine
{
    public class GameObject
    {
        public string name;
        protected bool selected = false;
        protected bool expanded = false;
        private List<GameObject>? _Children = new List<GameObject>();
        private GameObject? _Parent;
        private List<Component> components = new List<Component>();

        public List<GameObject>? Children
        {
            get => _Children;
            set
            {
                Transform.SetChildren(value);
                _Children = value;
            }
        }

        public GameObject? Parent
        {
            get => _Parent;
            set
            {
                Transform.SetParent(value);
                _Parent = value;
            }
        }

        public Transform2D Transform
        {
            get => _Transform;
            set => _Transform = value;
        }
        private Transform2D _Transform;
        public GameObject(GameObject Parent, Transform2D Transform)
        {
            this.name = "GameObject";
            this.Transform = Transform;
            this.Parent = Parent;
            this.Parent.Children.Add(this);
            this.AddComponent<Transform2D>(Transform);
        }

        public GameObject(Transform2D Transform)
        {
            this.name = "GameObject";
            this.Transform = Transform;
            this.Parent = null;
            this.AddComponent<Transform2D>(Transform);
        }

        public GameObject()
        {
            this.name = "GameObject";
            this.Transform = new Transform2D();
            this.Parent = null;
            this.AddComponent<Transform2D>(Transform);
        }

        public Component GetComponent<T>()
          where T : Component
        {
            try
            {
                Component component = components.Find(x => x is T);
                return component;
            }
            catch
            {
                throw new SystemException("Component not found");
            }
        }
        public List<Component> GetComponents() { return components; }

        public virtual void AddComponent<T>(T component)
                where T : Component
        { components.Add(component); }

        public virtual void Initialize()
        {
            foreach (Component component in components)
            {
                component.Initialize();
            }
        }

        public virtual void Draw(Camera2D? camera)
        {
            foreach (Component component in components)
            {
                component.Draw(camera);
            }
        }

        public virtual void Draw(Camera3D? camera)
        {
            foreach (Component component in components)
            {
                component.Draw(camera);
            }
        }

        public virtual void DrawInspector()
        {
            if (this.Children is not null && this.Children.Any<GameObject>())
            {
                ImGui.Columns(2);
                ImGui.Selectable(this.name, ref selected);
                ImGui.NextColumn();
                if (ImGui.ArrowButton("expand", ImGuiDir.Down))
                {
                    expanded = !expanded;
                }
            }
            else
            {
                ImGui.Columns(1);
                ImGui.Selectable(this.name, ref selected);
            }
            if (expanded)
            {
                ImGui.Indent(20);
                if (_Children is not null)
                {
                    foreach (GameObject Child in _Children)
                    {
                        Child.DrawInspector();
                    }
                }
            }
        }

        public virtual void EarlyUpdate()
        {
            foreach (Component component in components)
            {
                component.EarlyUpdate();
            }
        }

        public virtual void Update()
        {
            foreach (Component component in components)
            {
                component.Update();
            }
        }

        public virtual void LateUpdate()
        {
            foreach (Component component in components)
            {
                component.DrawInspector();
            }
        }
    }
}
