using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Nyxpiri.ULTRAKILL.NyxLib.Assets;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class ExternalAssetManager(string path, Assembly embeddedAssembly = null, string embeddedPrefix = null)
{
    public string Path = path;

    public T GetAsset<T>(string subPath) where T : ExternalAsset, new()
    {
        if (_assets.TryGetValue(subPath, out var genericAsset))
        {
            if (genericAsset is T existingAsset)
            {
                return existingAsset;
            }
        }

        T asset = new();

        if (asset is IManagerAwareAsset awareAsset)
        {
            awareAsset.AssetManager = this;
        }

        asset.Path = $"{Path}/{subPath}";
        if (embeddedAssembly != null)
        {
            if (embeddedPrefix == null)
            {
                embeddedPrefix = "";
            }

            asset.EmbeddedPath = $"{embeddedPrefix}{subPath.Replace('/', '.').Replace('\\', '.')}";
            asset.EmbedAssembly = embeddedAssembly;
        }
        _assets[subPath] = asset;

        return asset;
    }

    public void Reload()
    {
        var assets = _assets.Values.ToList();

        foreach (var asset in assets)
        {
            asset.MarkUnloaded();
        }

        foreach (var asset in assets)
        {
            if (asset.TryLoad())
            {
                Log.Message($"Reloaded asset {asset}");
            }
            else
            {
                Log.Error($"Reload asset {asset} failed!");
            }
        }
    }

    private Dictionary<string, ExternalAsset> _assets = [];
}

public interface IManagerAwareAsset
{
    public ExternalAssetManager AssetManager { get; internal set; }
}

public abstract class ExternalAsset
{
    public string Path = null;
    public string EmbeddedPath = null;
    public Assembly EmbedAssembly = null;
    public bool Loaded => _loaded;


    public void MarkUnloaded()
    {
        _loaded = false;
    }

    public bool TryLoad()
    {
        if (Loaded)
        {
            return Loaded;
        }

        _loaded = Load();

        return Loaded;
    }

    public override string ToString()
    {
        return $"({GetType().Name} @ {{'{Path}' || [{EmbeddedPath}]}})";
    }

    protected byte[] ReadFileBytes()
    {
        if (File.Exists(Path))
        {
            return File.ReadAllBytes(Path);
        }
        else if (EmbedAssembly != null && EmbedAssembly.GetManifestResourceNames().Contains(EmbeddedPath))
        {
            using var embedStream = EmbedAssembly.GetManifestResourceStream(EmbeddedPath);

            Assert.IsNotNull(embedStream);

            using var memStream = new MemoryStream();

            embedStream.CopyTo(memStream);

            var bytes = memStream.ToArray();

            Assert.IsNotNull(bytes); // tbh just being cautious because this is largely new to me

            return bytes;
        }
        else
        {
            throw new FileNotFoundException();
        }
    }

    protected string ReadFileText()
    {
        if (File.Exists(Path))
        {
            return File.ReadAllText(Path);
        }
        else if (EmbedAssembly != null && EmbedAssembly.GetManifestResourceNames().Contains(EmbeddedPath))
        {
            var embedStream = EmbedAssembly.GetManifestResourceStream(EmbeddedPath);
            StreamReader embedReader = new(embedStream);
            return embedReader.ReadToEnd();
        }
        else
        {
            throw new FileNotFoundException();
        }
    }

    public abstract bool Load();

    private bool _loaded = false;
}

public class TextureAsset : ExternalAsset
{
    public TextureAsset() { }

    public Texture2D Texture
    {
        get
        {
            if (!TryLoad())
            {
                Log.Error($"Loading Texture {this} failed");
            }

            return _texture;
        }
    }

    public override bool Load()
    {
        byte[] fileBytes;

        try
        {
            fileBytes = ReadFileBytes();
        }
        catch (System.Exception e)
        {
            if (e is FileNotFoundException)
            {
                Log.Error($"failed to load {this} due to file not being found");
                return false;
            }

            Log.Error($"failed to load {this} due to exception {e}");
            return false;
        }

        if (_texture == null)
        {
            Assets.ImageLoader.LoadImageOrDefault(fileBytes, out _texture, out bool success);

            if (!success)
            {
                return false;
            }
        }

        if (_texture == null)
        {
            return false;
        }

        _texture.filterMode = FilterMode;

        return Assets.ImageLoader.TryLoadImage(_texture, fileBytes);
    }

    Texture2D _texture = null;
    FilterMode FilterMode = FilterMode.Point;
}

public class AnimScriptAsset : ExternalAsset
{
    public AnimScript.Animation Animation
    {
        get
        {
            TryLoad();

            return _animation;
        }
    }

    public override bool Load()
    {
        if (_animation == null)
        {
            _animation = ScriptableObject.CreateInstance<AnimScript.Animation>();
        }

        if (_animation == null)
        {
            return false;
        }

        string fileText;

        try
        {
            fileText = ReadFileText();
        }
        catch (System.Exception e)
        {
            if (e is FileNotFoundException)
            {
                Log.Error($"failed to load animscript asset {this} due to file not being found");
                return false;
            }

            Log.Error($"failed to load animscript asset {this} due to exception {e}");
            return false;
        }

        try
        {
            _animation.ParseScript(fileText);
        }
        catch (FormatException e)
        {
            Log.Error($"failed to parse animscript {this} due to format exception: {e.Message}\nfull exception info{e}");
            return false;
        }

        return true;
    }

    AnimScript.Animation _animation = null;
}

public class ObjAsset : ExternalAsset
{
    public IReadOnlyList<Mesh> Meshes
    {
        get
        {
            TryLoad();

            return _meshes;
        }
    }

    public Mesh FindMesh(string meshName)
    {
        foreach (var mesh in Meshes)
        {
            if (mesh.name == meshName)
            {
                return mesh;
            }
        }

        Log.Message($"ExternalAsset making a new {meshName}");
        var newMesh = new Mesh();
        newMesh.name = meshName;
        _meshes.Add(newMesh);

        return newMesh;
    }

    public override bool Load()
    {
        string fileText;

        try
        {
            fileText = ReadFileText();
        }
        catch (System.Exception e)
        {
            ClearMeshes();

            if (e is FileNotFoundException)
            {
                Log.Error($"failed to load obj mesh asset {this} due to file not being found");
                return false;
            }

            Log.Error($"failed to load obj mesh asset {this} due to exception {e}");
            return false;
        }

        try
        {
            Assets.ObjLoader.LoadMeshes(fileText, _meshes);
            return true;
        }
        catch (System.Exception e)
        {
            ClearMeshes();
            Log.Error($"failed to load mesh {this}, exception: {e}");
            return false;
        }
    }

    private void ClearMeshes()
    {
        foreach (var mesh in _meshes)
        {
            Mesh.Destroy(mesh);
        }

        _meshes.Clear();
    }

    List<Mesh> _meshes = new List<Mesh>();
}

public class MaterialAsset : ExternalAsset, IManagerAwareAsset
{
    public Material Material
    {
        get
        {
            if (!Loaded)
            {
                TryLoad();
            }

            return _material;
        }
    }

    public ExternalAssetManager AssetManager { get; set; }

    public override bool Load()
    {
        if (_material == null)
        {
            _material = Materials.CreateLitMaterial();
        }

        if (_material == null)
        {
            return false;
        }

        string fileText;

        try
        {
            fileText = ReadFileText();
        }
        catch (System.Exception e)
        {
            if (e is FileNotFoundException)
            {
                Log.Error($"failed to load material asset {this} due to file not being found");
                return false;
            }

            Log.Error($"failed to load material asset {this} due to exception {e}");
            return false;
        }

        try
        {
            if (!JsonMaterial.ApplyTo(_material, AssetManager, fileText))
            {
                return false;
            }
        }
        catch (System.Exception e)
        {
            Log.Error($"Failed to load material at path {Path}, exception caught {e}");
        }

        return true;
    }

    Material _material = null;
}