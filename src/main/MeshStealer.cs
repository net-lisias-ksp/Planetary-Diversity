using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PlanetaryDiversity
{
    [KSPAddon(KSPAddon.Startup.PSystemSpawn, true)]
    public class MeshStealer : MonoBehaviour
    {
        /// <summary>
        /// The scaled space mesh of jool
        /// </summary>
        public static Mesh ReferenceGeosphere { get; set; }

        void Start()
        {
            // If Kopernicus is loaded, we have to use it's ReferenceGeosphere, because we have no chance to get the unmodified version before it might get changed by Kopernicus
            Type[] types = AssemblyLoader.loadedAssemblies.SelectMany(s => s.assembly.GetTypes()).ToArray();
            Type templates = types.FirstOrDefault(t => t.Name == "Templates" && t.Namespace == "Kopernicus");
            if (templates != null)
            {
                ReferenceGeosphere = templates.GetProperty("ReferenceGeosphere").GetValue(null, null) as Mesh;
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
