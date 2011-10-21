using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MvcContrib.PortableAreas;

namespace MvcContrib.UI.InputBuilder.ViewEngine
{
    /// <summary>
    /// Stores all the embedded resources for a single assembly/area.
    /// </summary>
    public class AssemblyResourceStore
    {
        private Dictionary<string, string> resources;
        private Type typeToLocateAssembly;
        private string namespaceName;
        private PortableAreaMap map;
        private AssemblyResourceLocator resourceLocator;

        public string VirtualPath { get; private set; }

        public AssemblyResourceStore(Type typeToLocateAssembly, string virtualPath, string namespaceName)
        {
            Initialize(typeToLocateAssembly, virtualPath, namespaceName, null);
        }

        public AssemblyResourceStore(Type typeToLocateAssembly, string virtualPath, string namespaceName, PortableAreaMap map)
        {
            Initialize(typeToLocateAssembly, virtualPath, namespaceName, map);
        }

        private void Initialize(Type typeToLocateAssembly, string virtualPath, string namespaceName, PortableAreaMap map)
        {
            this.map = map;
            this.typeToLocateAssembly = typeToLocateAssembly;
            // should we disallow an empty virtual path?
            this.VirtualPath = virtualPath.ToLower();
            this.namespaceName = namespaceName.ToLower();

            var resourceNames = this.typeToLocateAssembly.Assembly.GetManifestResourceNames();
            resources = new Dictionary<string, string>(resourceNames.Length);
            foreach (var name in resourceNames)
            {
                resources.Add(name.ToLower(), name);
            }
            resourceLocator = new AssemblyResourceLocator(this.resources, this.namespaceName, this.VirtualPath);
        }

        public Stream GetResourceStream(string resourceName)
        {
            string actualResourceName = null;

            if (resourceLocator.TryGetActualResourceName(resourceName, out actualResourceName))
            {
                Stream stream = this.typeToLocateAssembly.Assembly.GetManifestResourceStream(actualResourceName);

                if (map != null &&
                    (resourceName.ToLower().EndsWith(".aspx")
                     || resourceName.ToLower().EndsWith(".master")))
                    return map.Transform(stream);
                else
                    return stream;
            }
            else
            {
                return null;
            }
        }

        public string GetFullyQualifiedTypeFromPath(string path)
        {
            return resourceLocator.GetFullyQualifiedTypeFromPath(path);
        }

        public bool IsPathResourceStream(string path)
        {
            return resourceLocator.IsPathResourceStream(path);
        }

    }
}