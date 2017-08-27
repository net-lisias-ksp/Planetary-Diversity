using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PlanetaryDiversity
{
    [KSPAddon(KSPAddon.Startup.PSystemSpawn, true)]
    public class Templates : MonoBehaviour
    {
        /// <summary>
        /// The scaled space mesh of jool
        /// </summary>
        public static Mesh ReferenceGeosphere { get; set; }

        /// <summary>
        /// Whether Kopernicus is installed and we have access to its OnDemand features
        /// </summary>
        public static Boolean IsKopernicusInstalled { get; set; }

        /// <summary>
        /// All loaded types
        /// </summary>
        public static Type[] Types { get; set; }

        void Start()
        {
            // If Kopernicus is loaded, we have to use it's ReferenceGeosphere, because we have no chance to get the unmodified version before it might get changed by Kopernicus
            Types = AssemblyLoader.loadedAssemblies.SelectMany(s => s.assembly.GetTypes()).ToArray();
            Type templates = Types.FirstOrDefault(t => t.Name == "Templates" && t.Namespace == "Kopernicus");
            if (templates != null)
            {
                ReferenceGeosphere = templates.GetProperty("ReferenceGeosphere").GetValue(null, null) as Mesh;
                IsKopernicusInstalled = true;
            }
            else
            {
                // We need to get the body for Jool (to steal it's mesh)
                PSystemBody Jool = Utility.FindBody(PSystemManager.Instance.systemPrefab.rootBody, "Jool");

                // Return it's mesh
                ReferenceGeosphere = Jool.scaledVersion.GetComponent<MeshFilter>().sharedMesh;
            }
            Destroy(this);
        }
    }
}
