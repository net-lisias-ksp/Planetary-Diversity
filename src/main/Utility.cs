using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PlanetaryDiversity
{
    /// <summary>
    /// Generic utility functions
    /// </summary>
    public static class Utility
    {
        public static void RecalculateTangents(Mesh theMesh)
        {
            int vertexCount = theMesh.vertexCount;
            Vector3[] vertices = theMesh.vertices;
            Vector3[] normals = theMesh.normals;
            Vector2[] texcoords = theMesh.uv;
            int[] triangles = theMesh.triangles;
            int triangleCount = triangles.Length / 3;

            var tangents = new Vector4[vertexCount];
            var tan1 = new Vector3[vertexCount];
            var tan2 = new Vector3[vertexCount];

            int tri = 0;

            for (int i = 0; i < (triangleCount); i++)
            {
                int i1 = triangles[tri];
                int i2 = triangles[tri + 1];
                int i3 = triangles[tri + 2];

                Vector3 v1 = vertices[i1];
                Vector3 v2 = vertices[i2];
                Vector3 v3 = vertices[i3];

                Vector2 w1 = texcoords[i1];
                Vector2 w2 = texcoords[i2];
                Vector2 w3 = texcoords[i3];

                float x1 = v2.x - v1.x;
                float x2 = v3.x - v1.x;
                float y1 = v2.y - v1.y;
                float y2 = v3.y - v1.y;
                float z1 = v2.z - v1.z;
                float z2 = v3.z - v1.z;

                float s1 = w2.x - w1.x;
                float s2 = w3.x - w1.x;
                float t1 = w2.y - w1.y;
                float t2 = w3.y - w1.y;

                float r = 1.0f / (s1 * t2 - s2 * t1);
                var sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                var tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

                tan1[i1] += sdir;
                tan1[i2] += sdir;
                tan1[i3] += sdir;

                tan2[i1] += tdir;
                tan2[i2] += tdir;
                tan2[i3] += tdir;

                tri += 3;
            }
            for (int i = 0; i < (vertexCount); i++)
            {
                Vector3 n = normals[i];
                Vector3 t = tan1[i];

                // Gram-Schmidt orthogonalize
                Vector3.OrthoNormalize(ref n, ref t);

                tangents[i].x = t.x;
                tangents[i].y = t.y;
                tangents[i].z = t.z;

                // Calculate handedness
                tangents[i].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0f) ? -1.0f : 1.0f;
            }
            theMesh.tangents = tangents;
        }

        // Serialize a mesh to disk
        public static void SerializeMesh(Mesh mesh, string path)
        {
            // Open an output filestream
            FileStream outputStream = new FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            BinaryWriter writer = new BinaryWriter(outputStream);

            // Write the vertex count of the mesh
            writer.Write(mesh.vertices.Length);
            foreach (Vector3 vertex in mesh.vertices)
            {
                writer.Write(vertex.x);
                writer.Write(vertex.y);
                writer.Write(vertex.z);
            }
            writer.Write(mesh.uv.Length);
            foreach (Vector2 uv in mesh.uv)
            {
                writer.Write(uv.x);
                writer.Write(uv.y);
            }
            writer.Write(mesh.triangles.Length);
            foreach (int triangle in mesh.triangles)
            {
                writer.Write(triangle);
            }

            // Finish writing
            writer.Close();
            outputStream.Close();
        }

        // Deserialize a mesh from disk
        public static Mesh DeserializeMesh(string path)
        {
            FileStream inputStream = new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            BinaryReader reader = new BinaryReader(inputStream);

            // Get the vertices
            int count = reader.ReadInt32();
            Vector3[] vertices = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                Vector3 vertex;
                vertex.x = reader.ReadSingle();
                vertex.y = reader.ReadSingle();
                vertex.z = reader.ReadSingle();
                vertices[i] = vertex;
            }

            // Get the uvs
            int uv_count = reader.ReadInt32();
            Vector2[] uvs = new Vector2[uv_count];
            for (int i = 0; i < uv_count; i++)
            {
                Vector2 uv;
                uv.x = reader.ReadSingle();
                uv.y = reader.ReadSingle();
                uvs[i] = uv;
            }

            // Get the triangles
            int tris_count = reader.ReadInt32();
            int[] triangles = new int[tris_count];
            for (int i = 0; i < tris_count; i++)
                triangles[i] = reader.ReadInt32();

            // Close
            reader.Close();
            inputStream.Close();

            // Create the mesh
            Mesh m = new Mesh();
            m.vertices = vertices;
            m.triangles = triangles;
            m.uv = uvs;
            m.RecalculateNormals();
            m.RecalculateBounds();
            return m;
        }

        // Credit goes to Kragrathea.
        public static Texture2D BumpToNormalMap(Texture2D source, float strength)
        {
            strength = Mathf.Clamp(strength, 0.0F, 10.0F);
            var result = new Texture2D(source.width, source.height, TextureFormat.ARGB32, true);
            for (int by = 0; by < result.height; by++)
            {
                for (var bx = 0; bx < result.width; bx++)
                {
                    var xLeft = source.GetPixel(bx - 1, by).grayscale * strength;
                    var xRight = source.GetPixel(bx + 1, by).grayscale * strength;
                    var yUp = source.GetPixel(bx, by - 1).grayscale * strength;
                    var yDown = source.GetPixel(bx, by + 1).grayscale * strength;
                    var xDelta = ((xLeft - xRight) + 1) * 0.5f;
                    var yDelta = ((yUp - yDown) + 1) * 0.5f;
                    result.SetPixel(bx, by, new Color(yDelta, yDelta, yDelta, xDelta));
                }
            }
            result.Apply();
            return result;
        }
        
        // Whole file buffer management
        private static byte[] wholeFileBuffer = null;
        private static int sizeWholeFile = 0;
        private static int arrayLengthOffset = 0;

        public static byte[] LoadWholeFile(string path)
        {
            // If we haven't worked out if we can patch array length then do it
            if (arrayLengthOffset == 0)
                CalculateArrayLengthOffset();

            // If we can't patch array length then just use the normal function
            if (arrayLengthOffset == 1)
                return File.ReadAllBytes(path);

            // Otherwise we do cunning stuff
            FileStream file = File.OpenRead(path);
            if (file.Length > int.MaxValue)
                throw new Exception("File too large");

            int fileBytes = (int)file.Length;

            if (wholeFileBuffer == null || fileBytes > sizeWholeFile)
            {
                // Round it up to a 1MB multiple
                sizeWholeFile = (fileBytes + 0xFFFFF) & ~0xFFFFF;
                Debug.Log("[PlaneraryDiversity] LoadWholeFile reallocating buffer to " + sizeWholeFile);
                wholeFileBuffer = new byte[sizeWholeFile];
            }
            else
            {
                // Reset the length of the array to the full size
                FudgeByteArrayLength(wholeFileBuffer, sizeWholeFile);
            }

            // Read all the data from the file
            int i = 0;
            while (fileBytes > 0)
            {
                int read = file.Read(wholeFileBuffer, i, (fileBytes > 0x100000) ? 0x100000 : fileBytes);
                if (read > 0)
                {
                    i += read;
                    fileBytes -= read;
                }
            }

            // Fudge the length of the array
            FudgeByteArrayLength(wholeFileBuffer, i);

            return wholeFileBuffer;
        }

        public static byte[] LoadRestOfReader(BinaryReader reader)
        {
            // If we haven't worked out if we can patch array length then do it
            if (arrayLengthOffset == 0)
                CalculateArrayLengthOffset();

            long chunkBytes = reader.BaseStream.Length - reader.BaseStream.Position;
            if (chunkBytes > int.MaxValue)
                throw new Exception("Chunk too large");

            // If we can't patch array length then just use the normal function
            if (arrayLengthOffset == 1)
                return reader.ReadBytes((int)chunkBytes);

            // Otherwise we do cunning stuff
            int fileBytes = (int)chunkBytes;
            if (wholeFileBuffer == null || fileBytes > sizeWholeFile)
            {
                // Round it up to a 1MB multiple
                sizeWholeFile = (fileBytes + 0xFFFFF) & ~0xFFFFF;
                Debug.Log("[PlaneraryDiversity] LoadRestOfReader reallocating buffer to " + sizeWholeFile);
                wholeFileBuffer = new byte[sizeWholeFile];
            }
            else
            {
                // Reset the length of the array to the full size
                FudgeByteArrayLength(wholeFileBuffer, sizeWholeFile);
            }

            // Read all the data from the file
            int i = 0;
            while (fileBytes > 0)
            {
                int read = reader.Read(wholeFileBuffer, i, (fileBytes > 0x100000) ? 0x100000 : fileBytes);
                if (read > 0)
                {
                    i += read;
                    fileBytes -= read;
                }
            }

            // Fudge the length of the array
            FudgeByteArrayLength(wholeFileBuffer, i);

            return wholeFileBuffer;
        }

        unsafe static void CalculateArrayLengthOffset()
        {
            // Work out the offset by allocating a small array and searching backwards until we find the correct value
            int[] temp = new int[3];
            int offset = -4;
            fixed (int* ptr = &temp[0])
            {
                int* p = ptr - 1;
                while (*p != 3 && offset > -44)
                {
                    offset -= 4;
                    p--;
                }

                arrayLengthOffset = (*p == 3) ? offset : 1;
                Debug.Log("[PlaneraryDiversity] CalculateArrayLengthOffset using offset of " + arrayLengthOffset);
            }
        }

        unsafe static void FudgeByteArrayLength(byte[] array, int len)
        {
            fixed (byte* ptr = &array[0])
            {
                int* pLen = (int*)(ptr + arrayLengthOffset);
                *pLen = len;
            }
        }

        // Loads a texture
        public static Texture2D LoadTexture(string path, bool compress, bool upload, bool unreadable)
        {
            Texture2D map = null;
            if (File.Exists(path))
            {
                bool uncaught = true;
                try
                {
                    if (path.ToLower().EndsWith(".dds"))
                    {
                        // Borrowed from stock KSP 1.0 DDS loader (hi Mike!)
                        // Also borrowed the extra bits from Sarbian.
                        BinaryReader binaryReader = new BinaryReader(File.OpenRead(path));
                        uint num = binaryReader.ReadUInt32();
                        if (num == DDSHeaders.DDSValues.uintMagic)
                        {

                            DDSHeaders.DDSHeader dDSHeader = new DDSHeaders.DDSHeader(binaryReader);

                            if (dDSHeader.ddspf.dwFourCC == DDSHeaders.DDSValues.uintDX10)
                            {
                                new DDSHeaders.DDSHeaderDX10(binaryReader);
                            }

                            bool alpha = (dDSHeader.dwFlags & 0x00000002) != 0;
                            bool fourcc = (dDSHeader.dwFlags & 0x00000004) != 0;
                            bool rgb = (dDSHeader.dwFlags & 0x00000040) != 0;
                            bool alphapixel = (dDSHeader.dwFlags & 0x00000001) != 0;
                            bool luminance = (dDSHeader.dwFlags & 0x00020000) != 0;
                            bool rgb888 = dDSHeader.ddspf.dwRBitMask == 0x000000ff && dDSHeader.ddspf.dwGBitMask == 0x0000ff00 && dDSHeader.ddspf.dwBBitMask == 0x00ff0000;
                            //bool bgr888 = dDSHeader.ddspf.dwRBitMask == 0x00ff0000 && dDSHeader.ddspf.dwGBitMask == 0x0000ff00 && dDSHeader.ddspf.dwBBitMask == 0x000000ff;
                            bool rgb565 = dDSHeader.ddspf.dwRBitMask == 0x0000F800 && dDSHeader.ddspf.dwGBitMask == 0x000007E0 && dDSHeader.ddspf.dwBBitMask == 0x0000001F;
                            bool argb4444 = dDSHeader.ddspf.dwABitMask == 0x0000f000 && dDSHeader.ddspf.dwRBitMask == 0x00000f00 && dDSHeader.ddspf.dwGBitMask == 0x000000f0 && dDSHeader.ddspf.dwBBitMask == 0x0000000f;
                            bool rbga4444 = dDSHeader.ddspf.dwABitMask == 0x0000000f && dDSHeader.ddspf.dwRBitMask == 0x0000f000 && dDSHeader.ddspf.dwGBitMask == 0x000000f0 && dDSHeader.ddspf.dwBBitMask == 0x00000f00;

                            bool mipmap = (dDSHeader.dwCaps & DDSHeaders.DDSPixelFormatCaps.MIPMAP) != (DDSHeaders.DDSPixelFormatCaps)0u;
                            bool isNormalMap = ((dDSHeader.ddspf.dwFlags & 524288u) != 0u || (dDSHeader.ddspf.dwFlags & 2147483648u) != 0u);
                            if (fourcc)
                            {
                                if (dDSHeader.ddspf.dwFourCC == DDSHeaders.DDSValues.uintDXT1)
                                {
                                    map = new Texture2D((int)dDSHeader.dwWidth, (int)dDSHeader.dwHeight, TextureFormat.DXT1, mipmap);
                                    map.LoadRawTextureData(LoadRestOfReader(binaryReader));
                                }
                                else if (dDSHeader.ddspf.dwFourCC == DDSHeaders.DDSValues.uintDXT3)
                                {
                                    map = new Texture2D((int)dDSHeader.dwWidth, (int)dDSHeader.dwHeight, (TextureFormat)11, mipmap);
                                    map.LoadRawTextureData(LoadRestOfReader(binaryReader));
                                }
                                else if (dDSHeader.ddspf.dwFourCC == DDSHeaders.DDSValues.uintDXT5)
                                {
                                    map = new Texture2D((int)dDSHeader.dwWidth, (int)dDSHeader.dwHeight, TextureFormat.DXT5, mipmap);
                                    map.LoadRawTextureData(LoadRestOfReader(binaryReader));
                                }
                                else if (dDSHeader.ddspf.dwFourCC == DDSHeaders.DDSValues.uintDXT2)
                                {
                                    Debug.Log("[PlaneraryDiversity]: DXT2 not supported" + path);
                                }
                                else if (dDSHeader.ddspf.dwFourCC == DDSHeaders.DDSValues.uintDXT4)
                                {
                                    Debug.Log("[PlaneraryDiversity]: DXT4 not supported: " + path);
                                }
                                else if (dDSHeader.ddspf.dwFourCC == DDSHeaders.DDSValues.uintDX10)
                                {
                                    Debug.Log("[PlaneraryDiversity]: DX10 dds not supported: " + path);
                                }
                                else
                                    fourcc = false;
                            }
                            if (!fourcc)
                            {
                                TextureFormat textureFormat = TextureFormat.ARGB32;
                                bool ok = true;
                                if (rgb && (rgb888 /*|| bgr888*/))
                                {
                                    // RGB or RGBA format
                                    textureFormat = alphapixel
                                    ? TextureFormat.RGBA32
                                    : TextureFormat.RGB24;
                                }
                                else if (rgb && rgb565)
                                {
                                    // Nvidia texconv B5G6R5_UNORM
                                    textureFormat = TextureFormat.RGB565;
                                }
                                else if (rgb && alphapixel && argb4444)
                                {
                                    // Nvidia texconv B4G4R4A4_UNORM
                                    textureFormat = TextureFormat.ARGB4444;
                                }
                                else if (rgb && alphapixel && rbga4444)
                                {
                                    textureFormat = TextureFormat.RGBA4444;
                                }
                                else if (!rgb && alpha != luminance)
                                {
                                    // A8 format or Luminance 8
                                    textureFormat = TextureFormat.Alpha8;
                                }
                                else
                                {
                                    ok = false;
                                    Debug.Log("[PlaneraryDiversity]: Only DXT1, DXT5, A8, RGB24, RGBA32, RGB565, ARGB4444 and RGBA4444 are supported");
                                }
                                if (ok)
                                {
                                    map = new Texture2D((int)dDSHeader.dwWidth, (int)dDSHeader.dwHeight, textureFormat, mipmap);
                                    map.LoadRawTextureData(LoadRestOfReader(binaryReader));
                                }

                            }
                            if (map != null)
                                if (upload)
                                    map.Apply(false, unreadable);
                        }
                        else
                            Debug.Log("[PlaneraryDiversity]: Bad DDS header.");
                    }
                    else
                    {
                        map = new Texture2D(2, 2);
                        byte[] data = LoadWholeFile(path);
                        if (data == null)
                            throw new Exception("LoadWholeFile failed");

                        map.LoadImage(data);
                        if (compress)
                            map.Compress(true);
                        if (upload)
                            map.Apply(false, unreadable);
                    }
                }
                catch (Exception ex)
                {
                    uncaught = false;
                    Debug.Log("[PlaneraryDiversity]: failed to load " + path + " with exception " + ex.Message);
                }
                if (map == null && uncaught)
                {
                    Debug.Log("[PlaneraryDiversity]: failed to load " + path);
                }
                map.name = path.Remove(0, (KSPUtil.ApplicationRootPath + "GameData/").Length);
            }
            else
                Debug.Log("[PlaneraryDiversity]: texture does not exist! " + path);

            return map;
        }
    }
}
