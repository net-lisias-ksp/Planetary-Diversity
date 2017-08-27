using KSP.Localization;
using PlanetaryDiversity.API;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PlanetaryDiversity
{
    /// <summary>
    /// This class loads config options, collects modifiers from the AppDomain, and runs them
    /// </summary>
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class DiversityController : MonoBehaviour
    {
        /// <summary>
        /// The Singleton for the DiversityController
        /// </summary>
        public static DiversityController Instance { get; set; }

        /// <summary>
        /// A list of all classes that tweak PQSMods
        /// </summary>
        public List<IPQSModTweaker> PQSModTweakers { get; set; }

        /// <summary>
        /// A list of all classes that tweak Celestial Bodies
        /// </summary>
        public List<ICelestialBodyTweaker> CBTweakers { get; set; }

        /// <summary>
        /// The configurations for the tweaks
        /// </summary>
        public Dictionary<String, ConfigNode> ConfigCache { get; set; }

        /// <summary>
        /// A list of bodies that were edited
        /// </summary>
        private List<CelestialBody> scaledSpaceUpdate { get; set; }

        /// <summary>
        /// A list of bodies that shouldn't get edited
        /// </summary>
        private ConfigNode bodyBlacklist { get; set; }

        /// <summary>
        /// Called when the Component is created
        /// </summary>
        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
            PQSModTweakers = new List<IPQSModTweaker>();
            CBTweakers = new List<ICelestialBodyTweaker>();
            ConfigCache = new Dictionary<String, ConfigNode>();
            scaledSpaceUpdate = new List<CelestialBody>();
        }

        /// <summary>
        /// Called when the component is activated
        /// </summary>
        void Start()
        {
            // Get all types who extend a Tweaker Interface and add them to the storage
            AssemblyLoader.loadedAssemblies.TypeOperation(type =>
            {
                if (typeof(IPQSModTweaker).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    IPQSModTweaker tweaker = (IPQSModTweaker)Activator.CreateInstance(type);
                    PQSModTweakers.Add(tweaker);

                    // Get the config
                    String configNodeName = tweaker.GetConfig();
                    if (!ConfigCache.ContainsKey(configNodeName))
                    {
                        ConfigNode config = GameDatabase.Instance.GetConfigs(configNodeName)[0].config;
                        ConfigCache.Add(configNodeName, config);
                    }
                }
                if (typeof(ICelestialBodyTweaker).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    ICelestialBodyTweaker tweaker = (ICelestialBodyTweaker)Activator.CreateInstance(type);
                    CBTweakers.Add(tweaker);

                    // Get the config
                    String configNodeName = tweaker.GetConfig();
                    if (!ConfigCache.ContainsKey(configNodeName))
                    {
                        ConfigNode config = GameDatabase.Instance.GetConfigs(configNodeName)[0].config;
                        ConfigCache.Add(configNodeName, config);
                    }
                }
            });

            // Get the blacklist
            bodyBlacklist = GameDatabase.Instance.GetConfigs("PD_BODY_BLACKLIST")[0].config;

            // Register the callback for manipulating the system
            GameEvents.onGameSceneSwitchRequested.Add(OnGameSceneSwitchRequested);
            GameEvents.onLevelWasLoaded.Add(OnLevelWasLoaded);
        }

        /// <summary>
        /// Gets called when the users switches from one game scene to another one.
        /// </summary>
        void OnGameSceneSwitchRequested(GameEvents.FromToAction<GameScenes, GameScenes> action)
        {
            // Are we loading a game?
            if (action.from == GameScenes.MAINMENU && action.to == GameScenes.SPACECENTER)
            {
                for (Int32 i = 0; i < PQSModTweakers.Count; i++)
                {
                    // Tweaker
                    IPQSModTweaker tweaker = PQSModTweakers[i];

                    // Check the config
                    ConfigNode config = ConfigCache[tweaker.GetConfig()];

                    // Is the tweak group enabled?
                    if (!config.HasValue("enabled"))
                        continue;
                    if (!Boolean.TryParse(config.GetValue("enabled"), out Boolean isEnabled) || !isEnabled)
                        continue;

                    // Is the tweak itself enabled?
                    String setting = tweaker.GetSetting();
                    if (setting != null)
                    {
                        if (!config.HasValue(setting))
                            continue;
                        if (!Boolean.TryParse(config.GetValue(setting), out isEnabled) || !isEnabled)
                            continue;
                    }

                    // Tweak it!
                    for (Int32 j = 0; j < PSystemManager.Instance.localBodies.Count; j++)
                    {
                        // Get the Body
                        CelestialBody body = PSystemManager.Instance.localBodies[j];

                        // Is this body blacklisted?
                        if (bodyBlacklist != null)
                        {
                            if (bodyBlacklist.GetValues("blacklist").Any(b => body.bodyName == b))
                                continue;
                        }

                        // Get the PQSMods
                        PQSMod[] mods = body.GetComponentsInChildren<PQSMod>(true);

                        // Was the body edited?
                        Boolean edited = false;

                        // Tweak them
                        foreach (PQSMod mod in mods)
                        {
                            if (tweaker.Tweak(body, mod))
                            {
                                edited = true;
                                mod.OnSetup();
                            }
                        }       
                        
                        // The body was edited, we should update it's scaled space
                        if (edited && !scaledSpaceUpdate.Any(b => b.name == body.name))
                        {
                            scaledSpaceUpdate.Add(body);
                        }
                    }
                }

                // Tweak Celestial Bodies
                for (Int32 i = 0; i < CBTweakers.Count; i++)
                {
                    // Tweaker
                    ICelestialBodyTweaker tweaker = CBTweakers[i];

                    // Check the config
                    ConfigNode config = ConfigCache[tweaker.GetConfig()];

                    // Is the tweak group enabled?
                    if (!config.HasValue("enabled"))
                        continue;
                    if (!Boolean.TryParse(config.GetValue("enabled"), out Boolean isEnabled) || !isEnabled)
                        continue;

                    // Is the tweak itself enabled?
                    String setting = tweaker.GetSetting();
                    if (setting != null)
                    {
                        if (!config.HasValue(setting))
                            continue;
                        if (!Boolean.TryParse(config.GetValue(setting), out isEnabled) || !isEnabled)
                            continue;
                    }

                    // Tweak it!
                    for (Int32 j = 0; j < PSystemManager.Instance.localBodies.Count; j++)
                    {
                        // Get the Body
                        CelestialBody body = PSystemManager.Instance.localBodies[j];

                        // Tweak it
                        tweaker.Tweak(body);
                    }
                }
            }

            // Are we leaving the game?
            if (action.to == GameScenes.MAINMENU)
            {
                if (PSystemManager.Instance?.localBodies == null)
                    return;

                // Kill the PQS so it definitly rebuilds when we load a game
                for (Int32 i = 0; i < PSystemManager.Instance.localBodies.Count; i++)
                {
                    CelestialBody body = PSystemManager.Instance.localBodies[i];
                    body.pqsController?.ClearCache();
                    body.pqsController?.ResetSphere();
                }
            }
        }

        /// <summary>
        /// Gets called when the spacecenter scene was loaded
        /// </summary>
        void OnLevelWasLoaded(GameScenes scene)
        {
            // Should we update the Scaled Space?
            if (scaledSpaceUpdate.Count != 0 && scene == GameScenes.SPACECENTER)
            {
                guiEnabled = true;
                StartCoroutine(UpdateScaledSpace());

                FlightDriver.SetPause(true, false);
                InputLockManager.SetControlLock("planetaryDiversityCache");
            }
        }

        private Boolean guiEnabled;

        /// <summary>
        /// Renders an UI
        /// </summary>
        void OnGUI()
        {
            if (!guiEnabled)
                return;

            GUILayout.Window("PlanetaryDiversity".GetHashCode(), new Rect(100, 100, 300, 200), (id) => {
                GUILayout.BeginVertical();
                GUILayout.BeginHorizontal();
                GUILayout.Label(Localizer.Format("#LOC_PlanetaryDiversity_GUI_Label"));
                GUILayout.EndHorizontal();
                GUILayout.BeginScrollView(new Vector2(0, Single.MaxValue));
                GUIStyle green = new GUIStyle(GUI.skin.label);
                green.normal.textColor = XKCDColors.AcidGreen;
                GUIStyle red = new GUIStyle(GUI.skin.label);
                red.normal.textColor = Color.yellow;
                for (Int32 i = 0; i < scaledSpaceUpdate.Count; i++)
                {
                    // Get the body
                    CelestialBody body = scaledSpaceUpdate[i];

                    // is this the current body?
                    if (current != body.bodyDisplayName)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("   " + body.bodyDisplayName.Replace("^N", "") + " (100.00 %)", green);
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("   " + body.bodyDisplayName.Replace("^N", "") + " (" + percent.ToString("0.00") + " %)", red);
                        GUILayout.EndHorizontal();
                        break;
                    }
                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();
            }, Localizer.Format("#LOC_PlanetaryDiversity_GUI_Title"));
        }

        private Double percent;
        private String current;

        /// <summary>
        /// A coroutine that updates the scaled space in the background
        /// </summary>
        private IEnumerator UpdateScaledSpace()
        {
            // Path to the cache
            String CacheDirectory = KSPUtil.ApplicationRootPath + "saves/" + HighLogic.SaveFolder + "/diversity/";

            for (Int32 a = 0; a < scaledSpaceUpdate.Count; a++)
            {
                // Get the body
                CelestialBody body = scaledSpaceUpdate[a];
                current = body.bodyDisplayName;

                // Mesh
                Directory.CreateDirectory(CacheDirectory + "mesh");
                String CacheFile = CacheDirectory + "mesh/" + body.bodyName + ".bin";
                if (File.Exists(CacheFile))
                {
                    Mesh scaledMesh = Utility.DeserializeMesh(CacheFile);
                    Utility.RecalculateTangents(scaledMesh);
                    body.scaledBody.GetComponent<MeshFilter>().sharedMesh = scaledMesh;
                }

                // Otherwise we have to generate the mesh
                else
                {
                    const Double rJool = 6000000.0;
                    const Single rScaled = 1000.0f;

                    // Compute scale between Jool and this body
                    float scale = (float)(body.Radius / rJool);
                    body.scaledBody.transform.localScale = new Vector3(scale, scale, scale);

                    // Apply the mesh to ScaledSpace
                    MeshFilter meshfilter = body.scaledBody.GetComponent<MeshFilter>();
                    SphereCollider collider = body.scaledBody.GetComponent<SphereCollider>();
                    meshfilter.sharedMesh = Utility.ComputeScaledSpaceMesh(body, body.pqsController);
                    yield return null;
                    Utility.RecalculateTangents(meshfilter.sharedMesh);
                    collider.radius = rScaled;
                    if (body.pqsController != null)
                    {
                        body.scaledBody.gameObject.transform.localScale = Vector3.one * (float)(body.pqsController.radius / rJool);
                    }
                    yield return null;

                    // Serialize
                    Utility.SerializeMesh(meshfilter.sharedMesh, CacheFile);
                    yield return null;
                }

                // Textures
                Directory.CreateDirectory(CacheDirectory + "textures/" + body.bodyName);
                String TextureDirectory = CacheDirectory + "textures/" + body.bodyName + "/";
                if (File.Exists(TextureDirectory + "color.png") && File.Exists(TextureDirectory + "normal.png"))
                {
                    Texture2D colorMap = Utility.LoadTexture(TextureDirectory + "color.png", false, true, true);
                    yield return null;
                    Texture2D normalMap = Utility.LoadTexture(TextureDirectory + "normal.png", false, true, true);
                    yield return null;

                    // Apply them to the ScaledVersion
                    body.scaledBody.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", colorMap);
                    body.scaledBody.GetComponent<MeshRenderer>().material.SetTexture("_BumpMap", normalMap);
                    yield return null;
                }
                else
                {
                    // Get PQS
                    PQS pqs = body.pqsController;
                    pqs.SetupExternalRender();

                    // Get the mods
                    Action<PQS.VertexBuildData> modOnVertexBuildHeight = (Action<PQS.VertexBuildData>)Delegate.CreateDelegate(
                        typeof(Action<PQS.VertexBuildData>),
                        pqs,
                        typeof(PQS).GetMethod("Mod_OnVertexBuildHeight", BindingFlags.Instance | BindingFlags.NonPublic));
                    Action<PQS.VertexBuildData> modOnVertexBuild = (Action<PQS.VertexBuildData>)Delegate.CreateDelegate(
                        typeof(Action<PQS.VertexBuildData>),
                        pqs,
                        typeof(PQS).GetMethod("Mod_OnVertexBuild", BindingFlags.Instance | BindingFlags.NonPublic));
                    PQSMod[] mods = pqs.GetComponentsInChildren<PQSMod>().Where(m => m.sphere == pqs && m.modEnabled).ToArray();

                    // Create the Textures
                    Texture2D colorMap = new Texture2D(pqs.mapFilesize, pqs.mapFilesize / 2, TextureFormat.ARGB32, true);
                    Texture2D heightMap = new Texture2D(pqs.mapFilesize, pqs.mapFilesize / 2, TextureFormat.RGB24, true);

                    // Arrays
                    Color[] colorMapValues = new Color[pqs.mapFilesize * (pqs.mapFilesize / 2)];
                    Color[] heightMapValues = new Color[pqs.mapFilesize * (pqs.mapFilesize / 2)];

                    // Wait a some time
                    yield return null;

                    // Loop through the pixels
                    for (int y = 0; y < (pqs.mapFilesize / 2); y++)
                    {
                        for (int x = 0; x < pqs.mapFilesize; x++)
                        {
                            // Update Message
                            percent = ((double)((y * pqs.mapFilesize) + x) / ((pqs.mapFilesize / 2) * pqs.mapFilesize)) * 100;

                            // Create a VertexBuildData
                            PQS.VertexBuildData data = new PQS.VertexBuildData
                            {
                                directionFromCenter = (QuaternionD.AngleAxis((360d / pqs.mapFilesize) * x, Vector3d.up) * QuaternionD.AngleAxis(90d - (180d / (pqs.mapFilesize / 2)) * y, Vector3d.right)) * Vector3d.forward,
                                vertHeight = pqs.radius
                            };

                            // Build from the Mods 
                            modOnVertexBuildHeight(data);
                            modOnVertexBuild(data);

                            // Adjust the height
                            double height = (data.vertHeight - pqs.radius) * (1d / pqs.mapMaxHeight);
                            if (height < 0)
                                height = 0;
                            else if (height > 1)
                                height = 1;

                            // Adjust the Color
                            Color color = data.vertColor;
                            if (!pqs.mapOcean)
                                color.a = 1f;
                            else if (height > pqs.mapOceanHeight)
                                color.a = 0f;
                            else
                                color = pqs.mapOceanColor.A(1f);

                            // Set the Pixels
                            colorMapValues[(y * pqs.mapFilesize) + x] = color;
                            heightMapValues[(y * pqs.mapFilesize) + x] = new Color((Single)height, (Single)height, (Single)height);
                        }
                        yield return null;
                    }

                    // Apply the maps
                    colorMap.SetPixels(colorMapValues);
                    colorMap.Apply();
                    heightMap.SetPixels(heightMapValues);
                    yield return null;

                    // Close the Renderer
                    pqs.CloseExternalRender();

                    // Bump to Normal Map
                    Texture2D normalMap = Utility.BumpToNormalMap(heightMap, 7);

                    // Serialize them to disk
                    File.WriteAllBytes(TextureDirectory + "color.png", colorMap.EncodeToPNG());
                    File.WriteAllBytes(TextureDirectory + "normal.png", normalMap.EncodeToPNG());
                    yield return null;

                    // Apply them to the ScaledVersion
                    body.scaledBody.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", colorMap);
                    body.scaledBody.GetComponent<MeshRenderer>().material.SetTexture("_BumpMap", normalMap);
                    yield return null;
                }
                
                // OnDemand
                if (Templates.IsKopernicusInstalled)
                {
                    Type onDemandType = Templates.Types.FirstOrDefault(t => t.Name == "ScaledSpaceDemand");
                    Component onDemand = body.scaledBody.GetComponent(onDemandType);
                    if (onDemand == null)
                        onDemand = body.scaledBody.AddComponent(onDemandType);
                    FieldInfo texture = onDemandType.GetField("texture");
                    FieldInfo normals = onDemandType.GetField("normals");
                    String RelativeDirectory = TextureDirectory.Replace(KSPUtil.ApplicationRootPath, "../");
                    texture.SetValue(onDemand, RelativeDirectory + "color.png");
                    normals.SetValue(onDemand, RelativeDirectory + "normal.png");
                }
                percent = 0;
                yield return null;
            }
            guiEnabled = false;
            scaledSpaceUpdate.Clear();

            FlightDriver.SetPause(false, false);
            InputLockManager.RemoveControlLock("planetaryDiversityCache");
        }
    }
}
