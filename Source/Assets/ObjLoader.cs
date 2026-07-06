using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib.Assets
{
    public static class ObjLoader
    {
        public static void LoadMeshes(string objStr, IList<Mesh> outMeshes)
        {
            StringReader stringReader = new StringReader(objStr);
            ObjSceneData scene = new ObjSceneData();
            ObjectData activeObj = null;
            List<int> faceIndicesTempStore = new List<int>();

            for (string line = stringReader.ReadLine(); line != null; line = stringReader.ReadLine())
            {
                line = line.TrimStart(new char[] { ' ', '\t' });

                Func<string, string> errStr = (string msg) => $"{msg} : line: '{line}'";

                if (line.StartsWith('#'))
                {
                    continue;
                }

                var tokens = line.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);

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

                    val.x = parseFloat(tokens[tokenStart]) * -1.0f;
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

                    activeObj = new ObjectData(tokens[1]);

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

                    for (int i = 1; i < tokens.Length; i++)
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

                    activeObj.Faces.Add(faceData);
                }
                else
                {
                    Log.Warning($"unknown 0th token '{tokens[0]}' in obj parse, line: {line}");
                }
            }

            List<Vector3> positions = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> indices = new List<int>();

            for (int i = 0; i < scene.Objects.Count; i++)
            {
                positions.Clear();
                uvs.Clear();
                normals.Clear();
                indices.Clear();

                if (outMeshes.Count <= i)
                {
                    outMeshes.Add(new Mesh());
                }

                if (outMeshes[i] == null)
                {
                    outMeshes[i] = new Mesh();
                }

                var mesh = outMeshes[i];

                mesh.Clear();
                var objData = scene.Objects[i];

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

                mesh.name = objData.Name;
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

            public List<int> PositionIndices = new List<int>();
            public List<int> NormalIndices = new List<int>();
            public List<int> UVIndices = new List<int>();

            public ObjectData(string name)
            {
                Name = name;
            }
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
}