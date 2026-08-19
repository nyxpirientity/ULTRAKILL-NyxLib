using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Nyxpiri.ULTRAKILL.NyxLib.Assets;
using Nyxpiri.ULTRAKILL.NyxLib.Diagnostics.Debug;
using UnityEngine;

namespace Nyxpiri.ULTRAKILL.NyxLib;

public class ExternalAssetManager(string path)
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

        asset.Path = $"{Path}/Path";

        return asset;
    }

    public void Reload()
    {
        foreach (var asset in _assets.Values)
        {
            asset.MarkUnloaded();
        }

        foreach (var asset in _assets.Values)
        {
            if (!asset.TryLoad())
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
        return $"({GetType().Name} @ '{Path}')";
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
            TryLoad();

            return _texture;
        }
    }

    public override bool Load()
    {
        if (_texture == null)
        {
            Assets.ImageLoader.TryLoadImageOrDefault(Path, out _texture, out bool success);

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

        return Assets.ImageLoader.TryLoadImage(_texture, Path);
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

        if (!File.Exists(Path))
        {
            Log.Error($"failed to load animscript asset due file at {Path} not existing");
            return false;
        }

        try
        {
            _animation.ParseScript(File.ReadAllText(Path));
        }
        catch (FormatException e)
        {
            Log.Error($"failed to parse animscript due to format exception: {e.Message}\nfull exception info{e}");
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

    public override bool Load()
    {
        if (!File.Exists(Path))
        {
            Log.Error($"Failed to load mesh at path {Path} due to file not existing");
            ClearMeshes();
            return false;
        }

        try
        {
            Assets.ObjLoader.LoadMeshes(File.ReadAllText(Path), _meshes);
            return true;
        }
        catch (System.Exception e)
        {
            ClearMeshes();
            Log.Error($"failed to load mesh at '{Path}', exception: {e}");
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
            _material = Assets.Materials.CreateMaterial(default, default, default, default, default, default, default, default);
        }

        if (_material == null)
        {
            return false;
        }

        if (!File.Exists(Path))
        {
            Log.Error($"Failed to load material at path {Path} due to file not existing");
            return false;
        }

        try
        {
            var text = File.ReadAllText(Path);
            JsonMaterial.ApplyTo(_material, AssetManager, text);
        }
        catch (System.Exception e)
        {
            Log.Error($"Failed to load material at path {Path}, exception caught {e}");
        }

        return true;
    }

    Material _material = null;
}