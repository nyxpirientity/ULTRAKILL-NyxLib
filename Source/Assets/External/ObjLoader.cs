using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;
using UnityEngine.Rendering;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets;

public static class ObjLoader
{
    public static void LoadMeshes(string objStr, IList<Mesh> outMeshes)
    {
        StringReader stringReader = new StringReader(objStr);
        ObjSceneData scene = new ObjSceneData();
        ObjectData activeObj = null;
        List<int> faceIndicesTempStore = new List<int>();
        SubMeshData activeSubMesh = null;

        for (string line = stringReader.ReadLine(); line != null; line = stringReader.ReadLine())
        {
            line = line.TrimStart(new char[] { ' ', '\t' });

            Func<string, string> errStr = (string msg) => $"{msg} : line: '{line}'";

            if (line.StartsWith('#'))
            {
                continue;
            }

            var tokens = line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

            Func<int, float> parseInt = (tokenIdx) =>
            {
                if (tokens.Length <= tokenIdx)
                {
                    throw new InvalidDataException(errStr("attempt to parse int entry, seemingly not enough tokens/parameters"));
                }

                if (!int.TryParse(tokens[tokenIdx], System.Globalization.NumberStyles.Integer, CultureInfo.InvariantCulture, out var val))
                {
                    throw new InvalidDataException(errStr($"failed to parse {tokens[tokenIdx]} as int"));
                }

                return val;
            };

            Func<string, float> parseFloat = (str) =>
            {
                if (!float.TryParse(str, System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out var val))
                {
                    throw new InvalidDataException(errStr($"failed to parse {str} as float"));
                }

                return val;
            };

            Func<int, Vector3> parseVec3 = (int tokenStart) =>
            {
                if (tokens.Length <= tokenStart + 2)
                {
                    throw new InvalidDataException(errStr("attempt to parse 3D vector entry, seemingly not enough tokens/parameters"));
                }

                Vector3 val;

                val.x = parseFloat(tokens[tokenStart]);
                val.y = parseFloat(tokens[tokenStart + 1]);
                val.z = parseFloat(tokens[tokenStart + 2]) * -1.0f;

                return val;
            };

            Func<int, Vector3> parseVec2 = (int tokenStart) =>
            {
                if (tokens.Length <= tokenStart + 1)
                {
                    throw new InvalidDataException(errStr("attempt to parse 2D vector entry, seemingly not enough tokens/parameters"));
                }

                Vector2 val;

                val.x = parseFloat(tokens[tokenStart]);
                val.y = parseFloat(tokens[tokenStart + 1]);

                return val;
            };

            Action<string, List<int>> parseIndexArray = (string str, List<int> idxArr) =>
            {
                var idxTokens = str.Split('/', StringSplitOptions.RemoveEmptyEntries);

                foreach (var token in idxTokens)
                {
                    if (!int.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i))
                    {
                        throw new InvalidDataException($"attempt to parse index array, integer {token} invalid");
                    }

                    idxArr.Add(i);
                }
            };

            if (tokens[0] == "o")
            {
                if (tokens.Length <= 1)
                {
                    throw new InvalidDataException(errStr("attempt to parse 'o' object entry, seemingly not enough tokens/parameters (missing name)"));
                }

                if (activeSubMesh != null && !((activeObj?.SubMeshes?.Contains(activeSubMesh)).GetValueOrDefault(true)) && activeSubMesh.NumIndices > 0)
                {
                    activeObj.SubMeshes.Add(activeSubMesh);
                }

                activeObj = new ObjectData(tokens[1]);
                activeSubMesh = new() { Name = "DefaultMaterial" };

                scene.Objects.Add(activeObj);
            }
            else if (tokens[0] == "v")
            {
                Vector3 pos = parseVec3(1);

                scene.Positions.Add(pos);
            }
            else if (tokens[0] == "vn")
            {
                Vector3 normal = parseVec3(1);

                scene.Normals.Add(normal);
            }
            else if (tokens[0] == "vt")
            {
                Vector3 normal = parseVec2(1);

                scene.UVs.Add(normal);
            }
            else if (tokens[0] == "s")
            {
                continue;
            }
            else if (tokens[0] == "f")
            {
                FaceData faceData;

                faceData.PositionIndices = new Range(activeObj.PositionIndices.Count, 0);
                faceData.UVIndices = new Range(activeObj.UVIndices.Count, 0);
                faceData.NormalIndices = new Range(activeObj.NormalIndices.Count, 0);

                for (int i = tokens.Length - 1; i >= 1; i--)
                {
                    faceIndicesTempStore.Clear();
                    parseIndexArray(tokens[i], faceIndicesTempStore);

                    if (faceIndicesTempStore.Count != 3)
                    {
                        throw new InvalidDataException(errStr($"attempt to parse 'f' face entry but face index array did not provide position, normal and texcoord as expected"));
                    }

                    int posIdx = faceIndicesTempStore[0] - 1;
                    int uvIdx = faceIndicesTempStore[1] - 1;
                    int normIdx = faceIndicesTempStore[2] - 1;

                    activeObj.PositionIndices.Add(posIdx);
                    activeObj.UVIndices.Add(uvIdx);
                    activeObj.NormalIndices.Add(normIdx);

                    faceData.PositionIndices.Count += 1;
                    faceData.UVIndices.Count += 1;
                    faceData.NormalIndices.Count += 1;
                }

                if (faceData.PositionIndices.Count != 3)
                {
                    throw new InvalidDataException(errStr($"attempted to parse 'f' face entry but face was not a triangle, please only use triangles in obj meshes (if using blender, that lets you triangulate in the *export* menu so you don't have to manually triangulate!)"));
                }

                activeSubMesh.NumIndices += 3;
                activeObj.Faces.Add(faceData);
            }
            else if (tokens[0] == "usemtl")
            {
                if (tokens.Length <= 1)
                {
                    throw new InvalidCastException(errStr($"failed to parse 'usemtl' entry because not enough tokens to specify the material name."));
                }

                if (activeSubMesh != null && !activeObj.SubMeshes.Contains(activeSubMesh) && activeSubMesh.NumIndices > 0)
                {
                    activeObj.SubMeshes.Add(activeSubMesh);
                }

                var subMeshIdx = activeObj.SubMeshes.FindIndex((sm) => sm.Name == tokens[1]);

                if (subMeshIdx == -1)
                {
                    activeSubMesh = new() { Name = tokens[1] };
                }
                else
                {
                    activeSubMesh = activeObj.SubMeshes[subMeshIdx];
                }

                activeSubMesh.StartIndex = activeObj.Faces.Count * 3;
                activeSubMesh.NumIndices = 0;
            }
            else if (tokens[0] == "mtllib")
            {
                Log.ExpectedInfo($"ignoring mtllib 0th token");
            }
            else
            {
                Log.Warning($"unknown 0th token '{tokens[0]}' in obj parse, line: {line}");
            }
        }

        if (activeSubMesh != null && !activeObj.SubMeshes.Contains(activeSubMesh) && activeSubMesh.NumIndices > 0)
        {
            activeObj.SubMeshes.Add(activeSubMesh);
        }

        List<Vector3> positions = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> indices = new List<int>();

        for (int i = 0; i < scene.Objects.Count; i++)
        {
            var objData = scene.Objects[i];

            Mesh mesh = null;

            foreach (var pmesh in outMeshes)
            {
                if (pmesh.name == objData.Name)
                {
                    mesh = pmesh;
                    break;
                }
            }

            if (mesh == null)
            {
                mesh = new Mesh();
                outMeshes.Add(mesh);
            }

            mesh.Clear();
            mesh.name = objData.Name;

            positions.Clear();
            uvs.Clear();
            normals.Clear();
            indices.Clear();
            positions.Capacity = objData.PositionIndices.Count;
            uvs.Capacity = objData.UVIndices.Count;
            normals.Capacity = objData.NormalIndices.Count;

            for (int j = 0; j < objData.Faces.Count; j++)
            {
                var face = objData.Faces[j];

                var posIndices = objData.PositionIndices.GetRange(face.PositionIndices.Start, face.PositionIndices.Count);
                var uvIndices = objData.UVIndices.GetRange(face.UVIndices.Start, face.UVIndices.Count);
                var normIndices = objData.NormalIndices.GetRange(face.NormalIndices.Start, face.NormalIndices.Count);

                positions.Add(scene.Positions[posIndices[0]]);
                positions.Add(scene.Positions[posIndices[1]]);
                positions.Add(scene.Positions[posIndices[2]]);

                uvs.Add(scene.UVs[uvIndices[0]]);
                uvs.Add(scene.UVs[uvIndices[1]]);
                uvs.Add(scene.UVs[uvIndices[2]]);

                normals.Add(scene.Normals[normIndices[0]]);
                normals.Add(scene.Normals[normIndices[1]]);
                normals.Add(scene.Normals[normIndices[2]]);

                indices.Add(indices.Count);
                indices.Add(indices.Count);
                indices.Add(indices.Count);
            }

            mesh.vertices = positions.ToArray();
            mesh.uv = uvs.ToArray();
            mesh.normals = normals.ToArray();
            mesh.triangles = indices.ToArray();

            SubMeshDescriptor[] descs = new SubMeshDescriptor[objData.SubMeshes.Count];
            for (int j = 0; j < objData.SubMeshes.Count; j++)
            {
                SubMeshData subMesh = objData.SubMeshes[j];
                SubMeshDescriptor descriptor = new(subMesh.StartIndex, subMesh.NumIndices);
                descs[j] = descriptor;
            }

            mesh.subMeshCount = descs.Length;
            mesh.SetSubMeshes(descs);
            mesh.UploadMeshData(false);
            mesh.RecalculateBounds();
        }
    }

    class ObjSceneData
    {
        public List<ObjectData> Objects = new List<ObjectData>();

        public List<Vector3> Positions = new List<Vector3>();
        public List<Vector3> Normals = new List<Vector3>();
        public List<Vector3> UVs = new List<Vector3>();
    }

    class ObjectData
    {
        public string Name;

        public List<FaceData> Faces = new List<FaceData>();
        public List<SubMeshData> SubMeshes = new List<SubMeshData>();

        public List<int> PositionIndices = new List<int>();
        public List<int> NormalIndices = new List<int>();
        public List<int> UVIndices = new List<int>();

        public ObjectData(string name)
        {
            Name = name;
        }
    }

    class SubMeshData
    {
        public string Name;
        public int StartIndex = 0;
        public int NumIndices = 0;
    }

    struct FaceData
    {
        public Range PositionIndices;
        public Range NormalIndices;
        public Range UVIndices;
    }

    struct Range
    {
        public Range(int start, int count)
        {
            Start = start;
            Count = count;
        }

        public int Start;
        public int Count;
    }
}