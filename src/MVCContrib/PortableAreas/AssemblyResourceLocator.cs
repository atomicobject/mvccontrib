using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcContrib.PortableAreas
{
    /// <summary>
    /// Lookup to find an embedded resource from an assembly.<para/>
    /// This class abstract the fact that embedded resource in C# project and those in VB project
    /// are not embedded the same way.  VB compiler does not prefix embedded resource name with the 
    /// path of the original file.
    /// </summary>
    internal class AssemblyResourceLocator
    {
        private readonly Dictionary<string, string> resources;
        private readonly string namespaceName;
        private readonly string virtualPath;

        public AssemblyResourceLocator(Dictionary<string, string> resources, string namespaceName, string virtualPath)
        {
            this.virtualPath = virtualPath;
            this.resources = resources;
            this.namespaceName = namespaceName;
        }

        /// <summary>
        /// From the virtual path received, look for the associated embedded resource and returns if exists. <para/>
        /// This method supports both C# and VB embedded resource.
        /// </summary>
        /// 
        /// <remarks>
        /// This method looks first for a standard embedded resource (C#) and second for an embedded resource
        /// with the same name, but anywhere in the subnamespace structure (VB).
        /// </remarks>
        /// 
        /// <param name="path">
        /// The virtual path of a resource
        /// </param>
        /// <param name="resourceName">
        /// Returns the real embedded resource name associated to the virtual resource path.
        /// </param>
        public bool TryGetActualResourceName(string path, out string resourceName)
        {
            bool exists;

            resourceName = GetFullyQualifiedTypeFromPath(path);
            exists = (ResourcesContainsType(resourceName) ||
                      TryGetPartialResourceName(path, ref resourceName));
            if (exists)
                resourceName = resources[resourceName];
            return exists;
        }

        /// <summary>
        /// Returns the fully qualified path for an embedded resource from the virtual path recveived<para/>
        /// The path returned by this method is invalid if the virtual path point towards a VB PortableArea.  
        /// Unless everything in this VB assembly is at the root.
        /// </summary>
        public string GetFullyQualifiedTypeFromPath(string path)
        {
            string resourceName = path.ToLower().Replace("~", this.namespaceName);
            // we can make this more succinct if we don't have to check for emtpy virtual path (by preventing in constuctor)
            if (!string.IsNullOrEmpty(virtualPath))
                resourceName = resourceName.Replace(virtualPath, "");
            return resourceName.Replace("/", ".");
        }

        /// <summary>
        /// Returns true if a virtual path points towards an existing embedded resource. <para/>
        /// This method looks first for a standard embedded resource (C#) and second for an embedded resource
        /// with the same name, but anywhere in the subnamespace structure (VB).
        /// </summary>
        public bool IsPathResourceStream(string path)
        {
            return ResourcesContainsFullyQualifiedType(path) ||
                   ResourcesContainsPartiallyQualifiedType(path);
        }

        private bool ResourcesContainsFullyQualifiedType(string path)
        {
            return ResourcesContainsType(GetFullyQualifiedTypeFromPath(path));
        }

        private bool TryGetPartialResourceName(string path, ref string resourceName)
        {
            var keepGoingToRootNamespace = true;
            var rootLevel = 0;
            var exists = false;
            var partial = "";

            while (keepGoingToRootNamespace && !exists)
            {
                partial = GetPartiallyQualifiedTypeFromPath(path, rootLevel, ref keepGoingToRootNamespace);
                exists = ResourcesContainsType(partial);
                rootLevel++;
            }
            if (exists)
                resourceName = partial;
            return exists;
        }

        private bool ResourcesContainsPartiallyQualifiedType(string path)
        {
            string resourceName = "";
            var exists = TryGetPartialResourceName(path, ref resourceName);

            return (exists && ResourcesContainsType(resourceName));
        }

        private bool ResourcesContainsType(string typeString)
        {
            return resources.ContainsKey(typeString);
        }

        private string GetPartiallyQualifiedTypeFromPath(string path, int rootLevel, ref bool keepGoing)
        {
            var fullPath = GetFullyQualifiedTypeFromPath(path);
            return RemoveIntermediatePathFrom(fullPath, rootLevel, ref keepGoing);
        }

        private string RemoveIntermediatePathFrom(string fullPath, int rootLevel, ref bool keepGoing)
        {
            var prefix = this.namespaceName;
            var splitPrefix = prefix.Split('.');

            keepGoing = (splitPrefix.Length - 1 > rootLevel);
            if (keepGoing)
            {
                prefix = splitPrefix.Take(splitPrefix.Length - rootLevel).Aggregate(JoinNamespaces());
            }
            return JoinNamespaces(prefix, GetFileNameOnly(fullPath));
        }

        private string GetFileNameOnly(string fullPath)
        {
            var reverseFullPath = (from s in fullPath.Split('.').Reverse() select s);
            return reverseFullPath.Take(2)
                                  .Reverse()
                                  .Aggregate(JoinNamespaces());
        }

        private Func<string, string, string> JoinNamespaces()
        {
            return (x, y) => JoinNamespaces(x, y);
        }

        private string JoinNamespaces(string x, string y)
        {
            return string.Concat(x, ".", y);
        }
    }
}